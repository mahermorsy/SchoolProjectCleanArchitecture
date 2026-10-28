using System;
using System.Collections.Generic;
using System.Text;
using XUnitTest.Models;

namespace XUnitTest.Moqtest
{
    public class PhoneMoqService : IPhoneMoqService
    {
        private List<Phone> _PhonesList = new List<Phone>();

        public PhoneMoqService(List<Phone> PhonesList) 
        {
            _PhonesList = PhonesList;
        }
        public List<Phone> GetAll()     
        {
            // return an empty list for a simple mock
            return _PhonesList;
        }

        public Phone GetPhoneById(int id)
        {
            // return null (or a constructed Phone) as a simple mock
            var Phone = _PhonesList.FirstOrDefault(x => x.Id == id);

            return Phone;
        }

        public bool AddPhone(Phone phone)
        {
            // simulate success
            _PhonesList.Add(phone);
            return true;
        }

        public bool UpdatePhone(Phone phone)
        {
            // simulate success
            return true;
        }

        public bool DeletePhone(int id)
        {
            // simulate success
            Phone phone = _PhonesList.FirstOrDefault(x => x.Id == id);

            if (phone != null)
            {
                _PhonesList.Remove(phone);
            }
            return true;
        }
    }
}
