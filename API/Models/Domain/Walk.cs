namespace API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }  

        public string? WalkImangeUrl { get; set; }  

        public Guid DifficultyId { get; set; }  
        public Guid RegionId { get; set; }

        // thuộc tính điều hướng 

        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }
}
