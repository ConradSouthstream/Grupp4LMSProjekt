using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.KursLitteratur
{
    public class LitteraturListViewModel
    {
        public string Titel { get; set; }

        public string Forfattare { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public string OldSortBy { get; set; }

        public bool OrderAuthorByName { get; set; }

        public bool OrderAuthorByAge { get; set; }

        // Dropdown

        /// <summary>
        /// Amnen för dropdown
        /// </summary>
        public List<SelectListItem> Amnen { get; set; }

        public IEnumerable<KursLitteraturListViewModel> Litteratur { get; set; }
    }
}
