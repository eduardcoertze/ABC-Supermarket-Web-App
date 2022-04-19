using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _20112255.CLDV6212.Models;
using _20112255.CLDV6212.BlobHandler;
using _20112255.CLDV6212.TableHandler;

namespace _20112255.CLDV6212.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {
            //if statment to edit our product
            if (!string.IsNullOrEmpty(id))
            {
                //sets the table name for Azure table storage
                TableManager TableManagerObj = new TableManager("Product");

                //retrieves the product which will be edited
                List<Product> ProductListObj = TableManagerObj.RetrieveEntity<Product>("RowKey eq  '" + id + "'");
               
                Product ProductObj = ProductListObj.FirstOrDefault();
                return View(ProductObj);
            }
            return View(new Product());
        }

        // HTTP Post request for GET: Home
        [HttpPost]
        public ActionResult Index(string id, HttpPostedFileBase uploadFile, FormCollection formData)
        {
            Product ProductObj = new Product();
            ProductObj.productName = formData["productNane"] == "" ? null :
           formData["productName"];
            ProductObj.productDescription = formData["productDescription"] == "" ? null :
           formData["productDescription"];
            double productPrice;
            if (double.TryParse(formData["productPrice"], out productPrice))
            {
                ProductObj.productPrice = double.Parse(formData["productPrice"] == "" ? null :
               formData["productPrice"]);
            }
            else
            {
                return View(new Product());
            }
            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }

            //blob container creation
            BlobManager BlobManagerObj = new BlobManager("pictures");
            string FileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);
            ProductObj.FilePath = FileAbsoluteUri.ToString();

            //Insert statement
            if (string.IsNullOrEmpty(id))
            {
                ProductObj.PartitionKey = "Product";
                ProductObj.RowKey = Guid.NewGuid().ToString();
                TableManager TableManagerObj = new TableManager("Product");
                TableManagerObj.InsertEntity<Product>(ProductObj, true);
            }
            else
            {
                ProductObj.PartitionKey = "Product";
                ProductObj.RowKey = id;
                TableManager TableManagerObj = new TableManager("Product");
                TableManagerObj.InsertEntity<Product>(ProductObj, false);
            }
            return RedirectToAction("Get");
        }

        //get 'Products'
        public ActionResult Get()
        {
            TableManager TableManagerObj = new TableManager("Product");
            List<Product> ProductListObj = TableManagerObj.RetrieveEntity<Product>(null);
            return View(ProductListObj);
        }

        //Delete 'Products'
        public ActionResult Delete(string id)
        {
            //return the Car to be deleted
            TableManager TableManagerObj = new TableManager("Product");
            List<Product> ProductListObj = TableManagerObj.RetrieveEntity<Product>("RowKey eq  '" + id + "'");
           
            Product ProductObj = ProductListObj.FirstOrDefault();
            //delete the Car
            TableManagerObj.DeleteEntity<Product>(ProductObj);
            return RedirectToAction("Get");
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}