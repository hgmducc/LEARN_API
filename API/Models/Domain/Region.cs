namespace API.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        // dấu hỏi chấm cho phép truyền vào null
        public string? RegionImageUrl { get; set; }
    }

}
