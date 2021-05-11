using LMS.Grupp4.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.Admin
{
    public class AdminListUsersViewModel
    {
        public List<Anvandare> Larare { get; set; }
        public List<Anvandare> Elever { get; set; }

    }
}
