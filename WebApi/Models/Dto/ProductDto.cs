namespace WebApi.Models.Dto
{
    public sealed class ProductDto
    {
        public long ProductId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public long CategoryId { get; set; }
        public long SupplierId { get; set; }
        public IFormFile? Img { get; set; }
    }
}
