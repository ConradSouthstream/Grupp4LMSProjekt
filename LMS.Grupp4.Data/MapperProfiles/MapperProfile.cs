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
                .ForMember(dest => dest.ModulNamn, from => from.MapFrom(m => m.Modul.Namn))
                .ForMember(dest => dest.ModulId, from => from.MapFrom(m => m.Modul.Id))
                .ForMember(dest => dest.ModulStartDatum, from => from.MapFrom(m => m.Modul.StartDatum))
                .ForMember(dest => dest.ModulSlutDatum, from => from.MapFrom(m => m.Modul.SlutDatum))
                .ForMember(dest => dest.AktivitetTypNamn, from => from.MapFrom(a => a.AktivitetTyp.Namn));

            CreateMap<Aktivitet, AktivitetDetailsViewModel>()
                .ForMember(dest => dest.ModulNamn, from => from.MapFrom(m => m.Modul.Namn))
                .ForMember(dest => dest.ModulId, from => from.MapFrom(m => m.Modul.Id))
                .ForMember(dest => dest.ModulStartDatum, from => from.MapFrom(m => m.Modul.StartDatum))
                .ForMember(dest => dest.ModulSlutDatum, from => from.MapFrom(m => m.Modul.SlutDatum))
                .ForMember(dest => dest.AktivitetTypNamn, from => from.MapFrom(a => a.AktivitetTyp.Namn));

            CreateMap<Aktivitet, AktivitetEditViewModel>()
                .ForMember(dest => dest.ModulNamn, from => from.MapFrom(m => m.Modul.Namn))
                .ForMember(dest => dest.ModulId, from => from.MapFrom(m => m.Modul.Id))
                .ForMember(dest => dest.ModulStartDatum, from => from.MapFrom(ms => ms.Modul.StartDatum))
                .ForMember(dest => dest.ModulSlutDatum, from => from.MapFrom(ms => ms.Modul.SlutDatum));

            CreateMap<AktivitetEditViewModel, Aktivitet>();

            CreateMap<AktivitetCreateViewModel, Aktivitet>();

            CreateMap<Aktivitet, AktivitetDeleteViewModel>()
                .ForMember(dest => dest.ModulNamn, from => from.MapFrom(m => m.Modul.Namn))
                .ForMember(dest => dest.ModulId, from => from.MapFrom(m => m.Modul.Id))
                .ForMember(dest => dest.ModulStartDatum, from => from.MapFrom(m => m.Modul.StartDatum))
                .ForMember(dest => dest.ModulSlutDatum, from => from.MapFrom(m => m.Modul.SlutDatum))
                .ForMember(dest => dest.AktivitetTypNamn, from => from.MapFrom(a => a.AktivitetTyp.Namn));
        }
    }
}
