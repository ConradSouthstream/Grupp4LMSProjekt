using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.ViewModels.Aktivitet;
using LMS.Grupp4.Core.ViewModels.Elev;

namespace LMS.Grupp4.Data.MapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            AktivtetMap();
            ElevMap();
        }

        /// <summary>
        /// Automapper för Elev och dess ViewModels
        /// </summary>
        private void ElevMap()
        {            
            CreateMap<Anvandare, ElevDetailsViewModel>()
                .ForMember(dest => dest.ElevId, from => from.MapFrom(a => a.Id));

            CreateMap<Kurs, KursElevDetailsViewModel>()
                .ForMember(dest => dest.KursId, from => from.MapFrom(k => k.Id));

            CreateMap<Modul, ModulElevDetailsViewModel>()
                .ForMember(dest => dest.ModulId, from => from.MapFrom(m => m.Id));

            CreateMap<Aktivitet, AktivitetElevDetailsViewModel>()
                .ForMember(dest => dest.AktivitetId, from => from.MapFrom(a => a.Id))
                .ForMember(dest => dest.ModulNamn, from => from.MapFrom(m => m.Modul.Namn))
                .ForMember(dest => dest.KursNamn, from => from.MapFrom(k => k.Modul.Kurs.Namn))
                .ForMember(dest => dest.AktivitetTypNamn, from => from.MapFrom(a => a.AktivitetTyp.Namn));

            CreateMap<Anvandare, AnvandarDetaljerViewModel>();
        }


        /// <summary>
        /// Automapper för Aktivitet och dess ViewModels
        /// </summary>
        private void AktivtetMap()
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