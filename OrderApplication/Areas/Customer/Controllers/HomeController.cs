using Microsoft.AspNetCore.Mvc;
using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;
using System.Security.Claims;

namespace OrderApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
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

            _unitOfWork.Cart.Add(cart);
            _unitOfWork.Save();

            //if (cardDb == null)
            //{
                
            //    //kaydet sesionda sepeti ürün adedini sakla
            //}
            //else
            //{
            //    //countu gelen kadar arttır,kaydet
            //}
            return RedirectToAction("Index");
        }
    }
}
