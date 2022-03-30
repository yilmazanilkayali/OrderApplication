using Microsoft.AspNetCore.Mvc;
using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities.ViewModels;
using System.Security.Claims;

namespace OrderApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartVM CartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Sisteme giriş yapan kullanıcının sepetteki ürünlerini listeler.
        public IActionResult Index()
        {
             var claimsIdetity = (ClaimsIdentity)User.Identity;
            var claim =claimsIdetity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM = new CartVM()
            {
                CartList = _unitOfWork.Cart.GetAll(p => p.AppUserId == claim.Value,includeProperties:"Product"),
                OrderProduct = new()
            };
            foreach (var cart in CartVM.CartList)
            {
                cart.Price = cart.Product.Price * cart.Count; 
                CartVM.OrderProduct.OrderPrice += cart.Price ;

            }
            return View(CartVM);
        }
    }
}
