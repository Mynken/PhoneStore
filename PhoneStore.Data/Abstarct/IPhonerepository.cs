using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneStore.Data.Entities;
namespace PhoneStore.Data.Abstarct
{
    public interface IPhonerepository
    {
        IEnumerable<Phone> Phones { get; }
        void SavePhone(Phone phone);
        Phone DeletePhone(int phoneId);
    }
}
