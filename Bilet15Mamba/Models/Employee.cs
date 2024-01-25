namespace Bilet15Mamba.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        //relation
        public int PositionId { get; set; }
        public Position Position { get; set; }

        //Image
        public string ImageUrl { get; set; }

        //Social
        public string? FbLink { get; set; }
        public string? TwLink { get; set; }
        public string? LinLink { get; set; }
        public string? IgLink { get; set; }
    }
}
