using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.ContactUs
{
    public interface IContactUs
    {
        long InsertContact(ContactUsEntity entity);
    }
}
