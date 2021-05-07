using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.ViewModels.DokumentViewModel;

namespace LMS.Grupp4.Data.MapperProfiles
{
    public class DokumentProfile : Profile
    {
        public DokumentProfile()
        {

            CreateMap<Dokument, DocumentInput>().ReverseMap();
            CreateMap<Dokument, DokumentKursViewModel>().ReverseMap();

        }

    }
}
