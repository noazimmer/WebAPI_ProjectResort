using AutoMapper;
using DAL.DTO;

using MODELS;

namespace DAL.PROFILES
{
    public class ProFilesProj: Profile
    {
        public ProFilesProj() 
        {
            CreateMap<User, UserDTO>().ReverseMap(); 
        }
        
    }
}
