using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.ViewModels.Aktivitet;

namespace LMS.Grupp4.Data.MapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Aktivitet, AktivitetListViewModel>()
                .ForMember(dest => dest.ModulNamn, from => from.MapFrom(m => m.Modul.ModulNamn))
                .ForMember(dest => dest.ModulStartTid, from => from.MapFrom(m => m.Modul.StartTid))
                .ForMember(dest => dest.ModulSlutTid, from => from.MapFrom(m => m.Modul.SlutTid))
                .ForMember(dest => dest.AktivitetTypNamn, from => from.MapFrom(a => a.AktivitetTyp.AktivitetTypNamn));

            CreateMap<Aktivitet, AktivitetDetailsViewModel>()
                .ForMember(dest => dest.ModulNamn, from => from.MapFrom(m => m.Modul.ModulNamn))
                .ForMember(dest => dest.ModulStartTid, from => from.MapFrom(m => m.Modul.StartTid))
                .ForMember(dest => dest.ModulSlutTid, from => from.MapFrom(m => m.Modul.SlutTid))
                .ForMember(dest => dest.AktivitetTypNamn, from => from.MapFrom(a => a.AktivitetTyp.AktivitetTypNamn));

            CreateMap<Aktivitet, AktivitetEditViewModel>()
                .ForMember(dest => dest.ModulNamn, from => from.MapFrom(m => m.Modul.ModulNamn))
                .ForMember(dest => dest.ModulStartTid, from => from.MapFrom(ms => ms.Modul.StartTid))
                .ForMember(dest => dest.ModulSlutTid, from => from.MapFrom(ms => ms.Modul.SlutTid));

            CreateMap<AktivitetEditViewModel, Aktivitet>();
        }
    }
}
