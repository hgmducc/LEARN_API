namespace API.Models.DTO
{
    public class AddRegionRequestDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        // dấu hỏi chấm cho phép truyền vào null
        public string? RegionImageUrl { get; set; }
    }
}
