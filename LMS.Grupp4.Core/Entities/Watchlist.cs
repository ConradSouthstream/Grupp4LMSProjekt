using System;

namespace LMS.Grupp4.Core.Entities
{
    public class Watchlist
    {
     public int Id { get; set; }   
     public int DokumentId { get; set; }
     public Dokument Dokument { get; set; }
     public string AnvandareId { get; set; }
     public Anvandare Anvandare { get; set; }
     public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}