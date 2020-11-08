using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using InternetMagazine.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IO;
using System.IO.Pipes;

namespace InternetMagazine.Controllers
{
    [Authorize]
    public class BuyController : Controller
    {

        private ClassContext _db;

        public BuyController(ClassContext db)
        {
            _db = db;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index(int? category, string name, string typeSort = "None", string attributeSort = "None")
        {
            
                foreach (var p in _db.Product)
                {
                    if (p.NameImage != null)
                    {
                        var bytes = System.IO.File.ReadAllBytes(p.NameImage);
                        p.Image = bytes;
                    }
                }
            await _db.SaveChangesAsync();

            IQueryable<Product> products = _db.Product.Include(c => c.Category);
            if (category != null && category != 0)
                products = products.Where(p => p.Id_category == category);
            switch (attributeSort + typeSort)
            {
                case "NameAsc":
                    products = products.OrderBy(s => s.Name);
                    break;
                case "NameDesc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "PriceAsc":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "PriceDesc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
            }

            IndexViewModel viewModel = new IndexViewModel
            {
                SortViewModel = new SortViewModel(typeSort, attributeSort),
                FilterViewModel = new FilterViewModel(_db.Category.ToList(), category, name),
                Products = products
            };

            return View(viewModel);
        }

        [HttpGet]
        [ActionName("DeleteFromBasket")]
        public async Task<IActionResult> ConfirmDeleteFromBasket(int? id)
        {
            if (id != null)
            {
                Basket b = await _db.Basket.Include(p => p.Product).FirstOrDefaultAsync(p => p.Id_order == id);
                if (b != null)
                    return View(b);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromBasket(int? id)
        {
            if (id != null)
            {
                Basket b = await _db.Basket.Include(p => p.Product).FirstOrDefaultAsync(p => p.Id_order == id);
                if (b != null)
                {
                    _db.Basket.Remove(b);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("GoInBasket");
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> AboutProduct(int? id)
        {
            if (id != null)
            {
                Product p = await _db.Product.Include(pr => pr.Reviews).ThenInclude(c => c.Customer).FirstOrDefaultAsync(pr => pr.Id_Product == id);
                if (p != null)
                    return View(p);
            }

            return NotFound();
        }

        public async Task<IActionResult> AddInBasket(int? id, int CountProduct)
        {
            if (id != null)
            {
                Product p  = await _db.Product.FirstOrDefaultAsync(pr => pr.Id_Product == id);
                if (p != null)
                    return View(p);
            }
            return NotFound();
        }

        public async Task<IActionResult> OrderSuccessed()
        {
            WriteOfOrder wo = await _db.WriteOfOrder.LastOrDefaultAsync(c => c.Customer.Login == User.Identity.Name);
            return View(wo);
        }
        
        public async Task<IActionResult> AddComment(int? id, string comment, int rating)
        {
            Product p = await _db.Product.FirstOrDefaultAsync(pr => pr.Id_Product == id);
            Customer c = await _db.Customer.FirstOrDefaultAsync(cu => cu.Login == User.Identity.Name);

            if (comment != null)
            {
                _db.Review.Add(new Review()
                {
                    Id_Customer = c.Id_Customer,
                    Id_product = p.Id_Product,
                    Comment = comment,
                    DateOfWrite = DateTime.Now,
                    Mark = rating
                });

                await _db.SaveChangesAsync();
            }
            return RedirectToAction("AboutProduct", new { id = id});
        }

        [HttpPost]
        public async Task<IActionResult> AddInBasket(Product p, int? id, int CountProduct)
        {
            if (id != null)
            {
                Customer c = await _db.Customer.FirstOrDefaultAsync(cu => cu.Login == User.Identity.Name);
                p = await _db.Product.FirstOrDefaultAsync(pr => pr.Id_Product == id);
                Basket b = await _db.Basket.FirstOrDefaultAsync(bs => bs.Id_product == p.Id_Product && bs.Id_customer == c.Id_Customer);
                if (b != null)
                {
                    if (b.Count + CountProduct <= p.Count)
                    {
                        b.Count += CountProduct;
                        b.SummOrder = p.Price * b.Count;
                        _db.Basket.Update(b);
                    }
                    else ModelState.AddModelError("", "Выбранное вами количество товара превышает количество товара на складе!");
                }
                else
                {
                    _db.Basket.Add
                    (new Basket()
                    {
                        Id_customer = c.Id_Customer,
                        Id_product = p.Id_Product,
                        Count = CountProduct,
                        SummOrder = p.Price * CountProduct
                    });
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> GoInBasket(Customer c)
        {
            var basket = _db.Basket.Include(b => b.Product).Where(b => b.Customer.Login == User.Identity.Name).AsNoTracking();
            return View(await basket.ToListAsync());
        }


        public async Task<IActionResult> MyOrders()
        {
            var orders = _db.WriteOfOrder.Include(o => o.Orders)
                .ThenInclude(p => p.Product)
                .Where(p => p.Customer.Login == User.Identity.Name).AsNoTracking();
            return View(await orders.ToListAsync());
        }
        public IActionResult ClearBasket()
        {
            return View();
        }

       
        public IActionResult GetOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetOrder(OrderViewModel o)
        {
            Random r = new Random();
            var baskets = _db.Basket.Include(ba => ba.Product).Where(ba => ba.Customer.Login == User.Identity.Name).AsNoTracking();
            WriteOfOrder order = await _db.WriteOfOrder.LastOrDefaultAsync();
            Basket basket = await baskets.FirstOrDefaultAsync();
            int number_symbol = r.Next(65, 90);
            char symbol = (char)number_symbol;
            string number = r.Next(1, 999999).ToString();
            string n = "";

            for (int i = 0; i < 6 - number.Length; i++)
            {
                n += "0";
            }

            string Identity = symbol + "-" + n + number;
            int AddPay;

            if (o.TypeOfDelivery == "Самовывоз") AddPay = 0;
            else AddPay = 200;

            WriteOfOrder new_write = new WriteOfOrder()
            {
                Id_customer = basket.Id_customer,
                IdentityNumber = Identity,
                DateOfOrder = DateTime.Now,
                Address = o.Address,
                StateOrder = "В пути",
                TypeOfPay = o.TypeOfPay,
                TypeOfDelivery = o.TypeOfDelivery,
                AdditionalCharges = AddPay
            };

            _db.WriteOfOrder.Add(new_write);
            
            foreach (var item in baskets)

            {
                _db.Order.Add(new Order()
                {
                    Id_Record = new_write.Id_record,
                    Id_product = item.Id_product,
                    Count = item.Count,
                    SummOrder = item.SummOrder
                });
                Product p = await _db.Product.FirstOrDefaultAsync(pr => pr.Id_Product == item.Id_product);
                p.Count -= item.Count;
                _db.Product.Update(p);
                _db.Basket.Remove(item);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("OrderSuccessed");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
