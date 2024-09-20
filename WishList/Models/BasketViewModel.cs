namespace WishList.Models
{
    public class BasketViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
