using PhoneStore.Data.Abstarct;
using PhoneStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Data.Context
{
    public class EFGameRepository : IPhonerepository
    {
        PhoneContext context = new PhoneContext();

        public IEnumerable<Phone> Phones
        {
            get { return context.Phones; }
        }

        public void SavePhone(Phone phone)
        {
            if (phone.PhoneId == 0)
                context.Phones.Add(phone);
            else
            {
                Phone dbEntry = context.Phones.Find(phone.PhoneId);
                if (dbEntry != null)
                {
                    dbEntry.Name = phone.Name;
                    dbEntry.Description = phone.Description;
                    dbEntry.Price = phone.Price;
                    dbEntry.Category = phone.Category;
                    dbEntry.ImageData = phone.ImageData;
                    dbEntry.ImageMimeType = phone.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Phone DeletePhone(int phoneId)
        {
            Phone dbEntry = context.Phones.Find(phoneId);
            if(dbEntry != null)
            {
                context.Phones.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
