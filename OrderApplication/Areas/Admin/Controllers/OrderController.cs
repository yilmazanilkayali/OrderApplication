using Microsoft.AspNetCore.Mvc;
using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities.ViewModels;

namespace OrderApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWrok;
        private string orderStatusDelivered = "Delivered";
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
        public IActionResult Details(int Id)
        {
            OrderVM = new OrderVM()
            {
                OrderProduct = _unitOfWrok.OrderProduct.GetFirstOrDefault(u => u.Id == Id,includeProperties:"AppUser"),
                OrderDetails = _unitOfWrok.OrderDetails.GetAll(d => d.OrderProductId == Id,includeProperties:"Product")
            };
            return View(OrderVM);
        }
        [HttpPost]
        public IActionResult Delivered(OrderVM orderVM)
        {
            var orderProduct = _unitOfWrok.OrderProduct.GetFirstOrDefault(o => o.Id == orderVM.OrderProduct.Id);
            orderProduct.OrderStatus = orderStatusDelivered;

            _unitOfWrok.OrderProduct.Update(orderProduct);
            _unitOfWrok.Save();

            return RedirectToAction("Details","Order",new {Id = orderVM.OrderProduct.Id});
        }
    }
}
