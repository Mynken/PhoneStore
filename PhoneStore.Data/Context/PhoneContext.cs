using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PhoneStore.Data.Entities;

namespace PhoneStore.Data.Context
{
    class PhoneContext : DbContext
    {
        public DbSet<Phone> Phones { get; set; }
    }
}
