using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;
using OrderApplication.Entities.ViewModels;

namespace OrderApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unifOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unifOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unifOfWork = unifOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var productList = _unifOfWork.Product.GetAll();
            return View(productList);
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(Product product)
        //{
        //  //  if (!ModelState.IsValid)
        //  ///  {
        //        _unifOfWork.Product.Add(product);
        //        _unifOfWork.Save();
        //  //  }
        //    return RedirectToAction("Index");
        //}
        //Create ve Update actionları birleştirildi.
        public IActionResult Crup(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unifOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            
            if (id == null || id <= 0)
            {
                return View(productVM);
            }
            productVM.Product = _unifOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (productVM.Product == null)
            {
                return View(productVM);
            }

            return View(productVM);
        }
        [HttpPost]
        public IActionResult Crup(ProductVM productVM,IFormFile file)
        {
            //var prod = _unifOfWork.Product.GetFirstOrDefault(x => x.Id == id);int? id
            //prod.Price= productVM.Product.Price;
            // prod.Name = productVM.Product.Name;
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file!=null)
            {
                //reesim ekleme işlemini get thumbnail kullanarak boyutlandırarak yükle..
                string fileName= Guid.NewGuid().ToString();
                var uploadRoot = Path.Combine(wwwRootPath, @"img\products");
                var extension = Path.GetExtension(file.FileName);
                if (productVM.Product.Picture != null)
                {
                    var oldPath = Path.Combine(wwwRootPath, productVM.Product.Picture);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(uploadRoot,fileName + extension),
                    FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                productVM.Product.Picture = @"\img\products\" + fileName + extension;
            }
            if (productVM.Product.Id <= 0)
            {
                _unifOfWork.Product.Add(productVM.Product);
            }
            else
            {

                _unifOfWork.Product.Update(productVM.Product);

            }
            _unifOfWork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0 )
            {
                return NotFound();
            }
            var product = _unifOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            _unifOfWork.Product.Remove(product);
            _unifOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
