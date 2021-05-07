﻿using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.DokumentViewModel
{
    public class DocumentInput
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public int DokumentTypId { get; set; }
        public int? AnvandareId { get; set; }
        public Anvandare Anvandare { get; set; }
        public IFormFile File { get; set; }


    }
}
