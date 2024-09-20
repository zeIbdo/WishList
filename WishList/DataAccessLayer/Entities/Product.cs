namespace WishList.DataAccessLayer.Entities
{
    public class Product:BaseEntity
    {
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public Category? Categor { get; set; }

    }
}
