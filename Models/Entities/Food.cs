namespace Cater4Us_Backend.Models.Entities
{
    public class Food
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? FoodName { get; set; }
        public string? FoodDescription { get; set; }
        public string? Allergens { get; set; }

        // Property to store file path or URL to the image
        public string? ImageUrl { get; set; }

        // Alternatively, if you are using file identifiers
        // public string? ImageFileId { get; set; }

        public string? FoodType { get; set; }
        public int? StockCount { get; set; }
        public bool? OutofStock { get; set; }
        public float? PricePerPiece { get; set; }
    }
}
