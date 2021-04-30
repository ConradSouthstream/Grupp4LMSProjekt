namespace LMS.Grupp4.Core.Enum
{
    /// <summary>
    /// Enum somm används för att bestämma typ av meddelande, TempData["typeOfMessage"], 
    /// som finns i TempData["message"]. 
    /// TempData hämtas med ett anrop till metoden GetMessageFromTempData som finns i BaseController .
    /// Visas i _MessageView. View hämtar data från ViewBag.TypeOfMessage och ViewBag.Message  .
    /// Man kan även spara informationen direkt i ViewBag.TypeOfMessage och ViewBag.Message
    /// </summary>
    public enum TypeOfMessage
    {
        Error = 0,
        Info = 1
    }
}
