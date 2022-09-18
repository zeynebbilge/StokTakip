using StokTakip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakip.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        StokTakipDBEntities1 db = new StokTakipDBEntities1();
        [Authorize]
        public ActionResult Index()
        {
            var list = db.Product.ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult Add()
        {
            List<SelectListItem> liste = (from x in db.Category.ToList()
                                         select new SelectListItem
                                         {
                                             Text = x.Name,
                                             Value = x.Id.ToString()
                                         }).ToList();
            ViewBag.category = liste;
            return View();
        }
        [HttpPost]
        public ActionResult Add(Product product)
        {
            db.Product.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdatePage(int id)
        {
            var urun = db.Product.Where(x => x.Id == id).FirstOrDefault();
            List<SelectListItem> liste = (from x in db.Category.ToList()
                                          select new SelectListItem
                                          {
                                              Text = x.Name,
                                              Value = x.Id.ToString()
                                          }).ToList();
            ViewBag.category = liste;
            return View(urun);
        }
        [HttpPost]
        public ActionResult UpdateProduct(Product productData)
        {
            //var product = db.Product.Find(productData.Id);
            //product.Name = productData.Name;
            //product.Description = productData.Description;
            //product.Price = productData.Price;
            //product.Stock = productData.Stock;
            //product.CategoryId = productData.CategoryId;
            db.Entry(productData).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var product = db.Product.Where(x => x.Id == id).FirstOrDefault();
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}