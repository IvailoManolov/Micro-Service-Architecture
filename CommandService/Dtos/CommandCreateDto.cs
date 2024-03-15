using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Dtos
{
    public class CommandCreateDto
    {
        [Required]
        public string HowTo { get; set; }

        [Required]
        public string CommandLine { get; set; }
    }
}
