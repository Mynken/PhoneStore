using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PhoneStore.Data.Entities
{
    public class Phone
    {  
        //Short description
        [HiddenInput(DisplayValue = false)]
        public int PhoneId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please write the name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please write description")]
        public string Description { get; set; }

        [Display(Name = "Price (zl)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please write price")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please write Category")]
        public string Category { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        //Full description
        public string OperationSystem { get; set; }

        public string Color { get; set; }

        public string MultipleSim { get; set; }

        public string Cpu { get; set; }

        public string SizeWeight { get; set; }

        public string Display { get; set; }

        public string Camera { get; set; }
        //стандарты сети
        public string Standarts { get; set; }

        public string SimSize { get; set; }

        public string Memory { get; set; }
        //Датчики, форматы мультимедиа, Жесты и управление
        public string Entertainment { get; set; }

        public string Internet { get; set; }

        public string WirelessTechnology { get; set; }

        public string Battery { get; set; }

        public string PackageContent { get; set; }

        public string Guarantee { get; set; }

    }
}
