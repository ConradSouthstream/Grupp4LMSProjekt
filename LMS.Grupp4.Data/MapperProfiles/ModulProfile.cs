using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.ViewModels.Modul;

namespace LMS.Grupp4.Data.MapperProfiles
{
    public class ModulProfile : Profile
    {
        public ModulProfile()
        {
            CreateMap<Modul, ModulCreateViewModel>().ReverseMap();

        }
    }
}
