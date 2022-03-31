using Microsoft.AspNetCore.Mvc;
using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;
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
        public IActionResult Order()
        {
            var claimsIdetity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdetity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM = new CartVM()
            {
                CartList = _unitOfWork.Cart.GetAll(p => p.AppUserId == claim.Value, includeProperties: "Product"),
                OrderProduct = new()
            };
            CartVM.OrderProduct.AppUser = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Id == claim.Value);
            CartVM.OrderProduct.Name = CartVM.OrderProduct.AppUser.FullName;
            CartVM.OrderProduct.CellPhone = CartVM.OrderProduct.AppUser.CellPhone;
            CartVM.OrderProduct.Address = CartVM.OrderProduct.AppUser.Address;
            CartVM.OrderProduct.PostalCode = CartVM.OrderProduct.AppUser.PostalCode;

            foreach (var cart in CartVM.CartList)
            {
                cart.Price = cart.Product.Price * cart.Count;
                CartVM.OrderProduct.OrderPrice += cart.Price;

            }
            return View(CartVM);
        }
        [HttpPost]
        [ActionName("Order")]//Order actionunun postunu döndürür,oderposta parametre girmeden.
        public IActionResult OrderPost(CartVM cartVM)
        {
            var claimsIdetity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdetity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM = new CartVM()
            {
                CartList = _unitOfWork.Cart.GetAll(p => p.AppUserId == claim.Value, includeProperties: "Product"),
                OrderProduct = new()
            };
            AppUser appUser = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Id == claim.Value);
            CartVM.OrderProduct.AppUser = appUser;
            CartVM.OrderProduct.OrderDate = DateTime.Now;
            CartVM.OrderProduct.AppUserId = claim.Value;
            CartVM.OrderProduct.Name = cartVM.OrderProduct.Name;
            CartVM.OrderProduct.CellPhone = cartVM.OrderProduct.CellPhone;
            CartVM.OrderProduct.Address = cartVM.OrderProduct.Address;
            CartVM.OrderProduct.PostalCode = cartVM.OrderProduct.PostalCode;
            CartVM.OrderProduct.OrderStatus = "Ordered";
            foreach (var cart in CartVM.CartList)
            {
                cart.Price = cart.Product.Price * cart.Count;
                CartVM.OrderProduct.OrderPrice += cart.Price;

            }
            _unitOfWork.OrderProduct.Add(CartVM.OrderProduct);
            _unitOfWork.Save();

            foreach (var cart in CartVM.CartList)
            {
                OrderDetail orderDetails = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    OrderProductId = CartVM.OrderProduct.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Save();
            }
            List<Cart> Cars = _unitOfWork.Cart.GetAll(u => u.AppUserId == CartVM.OrderProduct.AppUserId).ToList();

            _unitOfWork.Cart.RemoveRange(Cars);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index),"Home",new {area="Customer"});
        }

        public IActionResult Increase(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(x => x.Id == cartId);
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Decrease(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(x => x.Id == cartId);
            if (cart.Count > 0) 
            {

                cart.Count -= 1;
                _unitOfWork.Save();
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
