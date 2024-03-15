using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    /*
    * 
     Representation when anyone wants to add data to our platform.
    *
    */
    public class PlatformCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public string Cost { get; set; }
    }
}