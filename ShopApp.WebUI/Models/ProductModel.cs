using ShopApp.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        //[Display(Name = "Name", Prompt = "Enter product name")]
        //[Required(ErrorMessage = "Name zorunlu bir alan")]
        //[StringLength(60, MinimumLength = 5, ErrorMessage = "Ürün ismi 5 ile 60 karakter aralığında olmalıdır.")]
        public string Name { get; set; }
        
        //[Required(ErrorMessage = "Url zorunlu bir alan")]
        public string Url { get; set; }
        
        //[Required(ErrorMessage = "Price zorunlu bir alan")]
        //[Range(1, 15000, ErrorMessage = "Fiyat için 1-15000 aralığında değer girmelisiniz.")]
        public double? Price { get; set; }
        
        //[Required(ErrorMessage = "Description zorunlu bir alan")]
        //[StringLength(1000, MinimumLength = 5, ErrorMessage = "Ürün açıklama 5 ile 1000 karakter aralığında olmalıdır.")]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}
