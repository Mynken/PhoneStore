using PhoneStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneStore.Web.Models
{
    public class PhonesListViewModel
    {
        public IEnumerable<Phone> Phones { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}