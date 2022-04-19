using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;

namespace _20112255.CLDV6212.Models
{
    public class Product : TableEntity
    {

        //Product varible declarations
        public Product() { }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public double productPrice { get; set; }
        public string FilePath { get; set; }

    }
}