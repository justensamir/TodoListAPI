using TodosAPI.DTOS;
using TodosAPI.Models;
using AutoMapper;
namespace TodosAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>();
            CreateMap<TodoDTO, Todo>();
            CreateMap<Todo, TodoDTO>();
        }
    }
}
