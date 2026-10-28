using System;
using System.Collections.Generic;
using System.Text;
using XUnitTest.Models;

namespace XUnitTest.Moqtest
{
    public interface IPhoneMoqService
    {
        public List<Phone> GetAll();
        public Phone GetPhoneById(int id);
        public bool AddPhone(Phone phone); 
        public bool UpdatePhone(Phone phone);
        public bool DeletePhone(int id);
    }
}
