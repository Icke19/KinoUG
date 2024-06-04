using AutoMapper;
using KinoUG.Server.DTO;
using KinoUG.Server.Models;

namespace KinoUG.Server.Helper
{
    public class AutoMapperProfiles:Profile
    {
       public AutoMapperProfiles()
        {
            CreateMap<User,UserDTO>();
            CreateMap<RegisterDTO, User>();
            CreateMap<Movie, MovieDTO>();
        }
    }
}
