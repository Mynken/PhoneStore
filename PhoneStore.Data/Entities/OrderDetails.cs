using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Data.Entities
{
    public class OrderDetails
    {
        [Required(ErrorMessage = "Please write your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please write your addres")]

        [Display(Name = "Adress1")]
        public string Line1 { get; set; }

        [Display(Name = "Adress2")]
        public string Line2 { get; set; }

        [Display(Name = "Adress3")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Please write city")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please write country")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
