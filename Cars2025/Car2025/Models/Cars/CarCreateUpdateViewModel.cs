namespace Car2025.Models.Cars
{
    public class CarCreateUpdateViewModel
    {
        public int? Id { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public int ProductionYear { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
