
namespace WebApplication1.DTOs
{
    /*
     * 
      Representation when anyone wants to read data from our platform.
     *
     */
    public class PlatformReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Publisher { get; set; }

        public string Cost { get; set; }
    }
}