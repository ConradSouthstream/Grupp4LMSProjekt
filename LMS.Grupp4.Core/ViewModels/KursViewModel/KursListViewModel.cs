using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Grupp4.Core.Entities;


namespace LMS.Grupp4.Core.ViewModels.KursViewModel
{
    public class KursListViewModel
    {
        public Kurs Kurs { get; set; }
        public bool IsTeacher { get; set; }
    }
}
