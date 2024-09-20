using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using WishList.DataAccessLayer;
using WishList.DataAccessLayer.Entities;
using WishList.Models;

namespace WishList.Controllers
{
    public class HomeController : Controller
    {


        private readonly AppDbContext _dbContext;



        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categories = _dbContext.Categories.ToList();
            var products = _dbContext.Products.ToList();

            var model = new HomeViewModel()
            {
                Categories = categories,
                Products = products

            };
            return View(model);
        }
        public IActionResult Basket()
        {
            List<BasketViewModel> products = new List<BasketViewModel>();
            var basket = Request.Cookies["basket"];
            if (!string.IsNullOrWhiteSpace(basket))
                products = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            return Json(products);
        }
        public async Task<IActionResult> AddToBasket(int? id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null) return BadRequest();

            var products = new List<BasketViewModel>();

            if (string.IsNullOrEmpty(Request.Cookies["basket"]))
            {
                products.Add(new BasketViewModel
                {
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    ProductId = product.Id,
                    Count = 1
                });
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketViewModel>>(Request.Cookies["basket"]);

                var exists = products.Find(x => x.ProductId == id);
                if (exists == null)
                {
                    products.Add(new BasketViewModel
                    {
                        Name = product.Name,
                        ImageUrl = product.ImageUrl,
                        Price = product.Price,
                        ProductId = product.Id,
                        Count = 1
                    });
                }
                else
                {
                    exists.Count++;
                    exists.ImageUrl = product.ImageUrl;
                    exists.Price = product.Price;
                    exists.Name = product.Name;
                }
            }

            var jsonPr = JsonConvert.SerializeObject(products);
            Response.Cookies.Append("basket", jsonPr);

            return RedirectToAction(nameof(Index));
        }

    }
}
