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
                _unitOfWork.Cart.Add(cart);
            }
            else if(cart.Count < maxCount  && cardDb.Count < maxCount)
            {
                //countu gelen kadar arttır,kaydet
                cardDb.Count += cart.Count; 
            }
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
