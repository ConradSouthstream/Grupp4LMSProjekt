using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web
{
    public class CheckDateAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var modul = (Modul)validationContext.ObjectInstance;
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));


            //ApplicationDbContext _context =  new ApplicationDbContext();
            var kurs = _context.Kurser.Where(m => m.Id == modul.KursId).FirstOrDefault();

            //var kurs = _context.Database.ExecuteSqlCommand("Select * from Kurser WHERE id = @modulId");
            if (modul.StartDatum >= kurs.StartDatum && modul.SlutDatum <=kurs.SlutDatum)
            {
                if (modul.Kurs.Moduler.Count == 0)
                {
                    return ValidationResult.Success;
                }
                foreach (var item in modul.Kurs.Moduler)
                {
                    if (modul.StartDatum != item.StartDatum && modul.SlutDatum != item.SlutDatum)
                    {
                        return ValidationResult.Success;

                    }
                    return new ValidationResult("Modul kan inte överlapp en annan Modul Datum");
                }

            }
            return new ValidationResult("Modul kan inte överlappa Kurs Datum !!");

        }

    }
}
