using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Techcareer.Models;

namespace Techcareer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController()
    {
    
    }

    public IActionResult Index(string q,string category)
    {
        var products =Repository.Products;
        if(!String.IsNullOrEmpty(q)){
            ViewBag.q = q;
            products= products.Where(p=>p.Name!.ToLower().Contains(q)).ToList();
        }
        if(!String.IsNullOrEmpty(category) && category != "0"){
            products= products.Where(p=>p.CategoryId == int.Parse(category)).ToList();
        }

        // ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name",category);
        var model = new ProductViewModel{
            Products =products,
            Categories= Repository.Categories,
            SelectedCategory=category
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories=ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");
        return View();
    }
    [HttpPost]

    // ************* DOSYA YÜKLEME ŞABLONU *************************** 
     public async Task<ActionResult> Create(Product model,IFormFile imageFile)
    {
        const int maxFileSize = 2*1024;
        var allowenExtensions = new[] {".jpeg",".png",".jpeg"};
        if(imageFile != null){
            if(imageFile.Length > maxFileSize){
                ModelState.AddModelError("","Dosya boyutu 2 MB den küçük olmalıdır");
            }
            var extensions = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if(!allowenExtensions.Contains(extensions)){
                ModelState.AddModelError("","Geçerli bir resim seçiniz");
            }
            else {
                var randomFileName =string.Format($"{Guid.NewGuid().ToString()}{extensions}");
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);

                try
                {
                    using(var stream = new FileStream(path,FileMode.Create)){
                        await imageFile.CopyToAsync(stream);
                    }
                    model.Image=randomFileName;
                }
                catch 
                {
                    ModelState.AddModelError("","Dosya yüklenirken bir hata oluştu");
                }
            }
        } else {
            ModelState.AddModelError("","Bir dosya seçiniz");
        }
// ******************


        if(ModelState.IsValid){
            model.ProductId=Repository.Products.Count+1;
         Repository.CreateProduct(model);
        return RedirectToAction("Index");   
        }
        ViewBag.Categories=ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");
        return View(model);
    }

}
