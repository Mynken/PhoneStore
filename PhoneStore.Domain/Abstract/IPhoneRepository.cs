using PhoneStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Domain.Abstract
{
    public interface IPhoneRepository
    {
        IEnumerable<Phone> Phones { get; }
    }
}
