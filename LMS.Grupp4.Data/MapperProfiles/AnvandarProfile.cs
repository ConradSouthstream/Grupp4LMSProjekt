using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.ViewModels.Elev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.Elev
{
    class AnvandarProfile :Profile
    {
        public AnvandarProfile()
        {
            CreateMap<Anvandare, AnvandarDetaljerViewModel>().ReverseMap();
        }
    }
}
