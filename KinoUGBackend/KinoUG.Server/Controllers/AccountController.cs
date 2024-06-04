using KinoUG.Server.Data;
using KinoUG.Server.DTO;
using KinoUG.Server.Models;
using KinoUG.Server.Repository.Interfaces;
using KinoUG.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KinoUG.Server.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(DataContext context, ITokenService tokenService, UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            var assignRoleResult = await _userManager.AddToRoleAsync(user, Roles.User);

            if (!assignRoleResult.Succeeded)
            {
                return new BadRequestObjectResult(assignRoleResult.Errors);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

            var htmlContent = GenerateConfirmationEmailHtml(user.Name, confirmationLink);
            var textContent = GenerateConfirmationEmailText(confirmationLink);

            try
            {
                await _emailService.SendEmailAsync(user.Email, "Confirm your email", htmlContent, textContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Email sending failed: {ex.Message}");
            }

            return Ok("Registration successful. Please check your email to confirm your account.");
        }

        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid email.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully!");
            }

            return BadRequest("Error confirming email.");
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Incorrect email or password." });
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(new { message = "Email not confirmed. Please check your inbox." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Incorrect email or password." });
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserPassword", model.Password);

            var token = await _tokenService.GenerateJwtToken(user, TimeSpan.FromMinutes(600));

            return Ok(new { Token = token });
        }

        [HttpGet]
        [Route("session-info")]
        public IActionResult SessionInfo()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var password = HttpContext.Session.GetString("userName");

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Session has expired or user not logged in.");
            }

            return Ok(new
            {
                Email = email,
                Password = password
            });
        }

        private string GenerateConfirmationEmailHtml(string userName, string confirmationLink)
        {
            return $@"
            <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            color: #333;
                        }}
                        .container {{
                            width: 80%;
                            margin: 0 auto;
                            padding: 20px;
                            border: 1px solid #ccc;
                            border-radius: 10px;
                            background-color: #f9f9f9;
                        }}
                        .header {{
                            text-align: center;
                            padding-bottom: 20px;
                        }}
                        .content {{
                            padding: 20px;
                            text-align: center;
                            color: #000000;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            margin: 20px 0;
                            border: none;
                            border-radius: 5px;
                            background-color: #28a745;
                            color: #ffffff;;
                            text-decoration: none;
                            font-size: 16px;
                        }}
                        .footer {{
                            text-align: center;
                            padding-top: 20px;
                            font-size: 12px;
                            color: #777;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Witamy w KinoUG!</h2>
                        </div>
                        <div class='content'>
                            <p>Cześć {userName},</p>
                            <p>Dziękujemy za zarejestrowanie się na platformie KinoUG. Potwierdź swoje konto klikając na poniższy przycisk:</p>
                            <a href='{confirmationLink}' class='button'>Potwierdź</a>
                        </div>
                        <div class='footer'>
                            <p>Jeśli nie rejestrowałeś się na naszej platformie, prosimy zignorować tą wiadomość.</p>
                            <p>&copy; 2024 KinoUG. Wszelkie prawa zastrzeżone.</p>
                        </div>
                    </div>
                </body>
            </html>";
        }

        private string GenerateConfirmationEmailText(string confirmationLink)
        {
            return $@"
            Część,

            Dziękujemy za zarejestrowanie się na platformie KinoUG. Potwierdź swoje konto klikając na przycisk poniżej

            {confirmationLink}

            Jeśli nie rejestrowałeś się na naszej platformie, prosimy zignorować tą wiadomość.

            Życzymy miłego dnia,
            KinoUG";
        }
    }
}
