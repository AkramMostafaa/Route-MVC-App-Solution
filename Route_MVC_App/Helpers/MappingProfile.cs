using AutoMapper;
using Route.DAL.Models;
using Route_MVC_App.PL.ViewModels;

namespace Route_MVC_App.PL.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeViewModel,Employee>().ReverseMap();
            CreateMap<DepartmentViewModel,Department>().ReverseMap();
        }
    }
}
