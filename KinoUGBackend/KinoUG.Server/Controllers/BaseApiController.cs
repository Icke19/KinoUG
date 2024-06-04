using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KinoUG.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Kino")]
    public class BaseApiController : ControllerBase
    {
    
    }
}
