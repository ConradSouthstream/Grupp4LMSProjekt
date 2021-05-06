using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.ViewModels.Modul;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace LMS.Grupp4.Data.MapperProfiles
{
    public class ModulProfile : Profile
    {
        public ModulProfile()
        {
            CreateMap<Modul, ModulCreateViewModel>().ReverseMap();
            CreateMap<Modul, ModulDetaljerViewModel>().ForMember(des=>des.KursNamn ,from=>from.MapFrom(m=>m.Kurs.Namn));
            CreateMap<SelectListItem, Char>();
        }
    }
}
