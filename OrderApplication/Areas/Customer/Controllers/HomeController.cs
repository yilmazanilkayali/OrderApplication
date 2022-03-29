using Microsoft.AspNetCore.Mvc;
using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;

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
    }
}
