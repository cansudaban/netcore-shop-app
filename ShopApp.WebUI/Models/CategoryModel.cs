﻿using ShopApp.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Kategori için 5-100 arası değer girmelisiniz.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kategori url zorunludur")]
        public string Url { get; set; }
        public List<Product> Products { get; set; }
    }
}
