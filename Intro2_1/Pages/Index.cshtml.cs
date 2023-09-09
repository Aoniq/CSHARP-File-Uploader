using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Intro2_1.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public IFormFile File { get; set; }



    public string ResultMessage { get; set; }



    public async Task<IActionResult> OnPostAsync()
    {
        if (File != null && File.Length > 0)
        {
            try
            {
                // Stap 1: Controleer of het bestand een JPEG-bestand is
                string fileExtension = Path.GetExtension(File.FileName);
                if (fileExtension != ".jpeg" && fileExtension != ".jpg")
                {
                    ResultMessage = "Alleen JPEG-bestanden (.jpeg of .jpg) zijn toegestaan.";
                }
                else
                {
                    // Bepaal het pad naar de uploadsmap
                    string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                    // Zorg ervoor dat de uploadsmap bestaat, zo niet, maak deze aan
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    // Genereer een unieke bestandsnaam om conflicten te voorkomen
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                    // Volledig pad naar het opgeslagen bestand
                    string filePath = Path.Combine(uploadsPath, uniqueFileName);

                    // Verplaats het geüploade bestand naar de uploadsmap
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await File.CopyToAsync(stream);
                    }

                    ResultMessage = "Bestand is met succes geüpload en opgeslagen in de uploadsmap.";

                }
            }
            catch (Exception ex)
            {
                ResultMessage = "Er is een fout opgetreden tijdens het verwerken van het bestand: " + ex.Message;
            }
        }
        else
        {
            ResultMessage = "Selecteer een bestand om te uploaden, anders gaat het niet werken!";
        }




        return Page();
    }
}

