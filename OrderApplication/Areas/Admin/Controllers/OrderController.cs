using Microsoft.AspNetCore.Mvc;
using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities.ViewModels;

namespace OrderApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWrok;
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWrok = unitOfWork; 
        }
        public IActionResult Index()
        {
            var orderList = _unitOfWrok.OrderProduct.GetAll(x => x.OrderStatus != "Delivered");
            return View(orderList);
        }
    }
}
