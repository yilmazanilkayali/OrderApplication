using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;
using System.Security.Claims;

namespace OrderApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private int maxCount = 10;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> produtList = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(produtList);
        }
        public IActionResult Details(int productId)
        {
            Cart cart = new Cart()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(p=>p.Id == productId, includeProperties:"Category"),
            };
            return View(cart);
        }
        [HttpPost]
        public IActionResult Details(Cart cart)
        {
            //sisteme giriş yapan kullanıcıyı tutan yapı claim
            var claimIdentity = (ClaimsIdentity)User.Identity; 
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //kullanıcı id si yakalar
            cart.AppUserId = claim.Value;

            Cart cardDb = _unitOfWork.Cart.GetFirstOrDefault(p => p.AppUserId == claim.Value && p.ProductId == cart.ProductId);

           

            if (cardDb == null )
            {

                //kaydet sesionda sepeti ürün adedini sakla
                //cartcount sorgusu her seferinde tekrar yapılmaması için if içerisinde kullanıldı bu sebeple save metodu 2 kez çağırıldı.
                _unitOfWork.Cart.Add(cart);
                _unitOfWork.Save();
                int cartCount = _unitOfWork.Cart.GetAll(u => u.AppUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32("SessionCartCount", cartCount);


            }
            else if(cart.Count < maxCount  && cardDb.Count < maxCount)
            {
                //countu gelen kadar arttır,kaydet
                cardDb.Count += cart.Count;
                _unitOfWork.Save();

            }




            return RedirectToAction("Index");
        }
    }
}
