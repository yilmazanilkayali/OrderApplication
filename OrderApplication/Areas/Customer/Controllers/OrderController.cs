using Microsoft.AspNetCore.Mvc;
using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;
using System.Security.Claims;

namespace OrderApplication.Areas.Customer.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<OrderProduct> orderProducts;
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            orderProducts = _unitOfWork.OrderProduct.GetAll(u => u.AppUserId == claim.Value);
            return View();
        }
    }
}
