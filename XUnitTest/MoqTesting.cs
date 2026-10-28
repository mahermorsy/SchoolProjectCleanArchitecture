using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTest.Models;
using XUnitTest.Moqtest;

namespace XUnitTest
{
    public class MoqTesting
    {
        //public PhoneMoqService mockService = new PhoneMoqService(new List<Phone>());
        private readonly Mock<List<Phone>> _mockService = new();
        private readonly PhoneMoqService _phoneMoqService;

        public MoqTesting() 
        {
            _phoneMoqService = new PhoneMoqService(_mockService.Object);
        }
        [Fact]
        public void Add_Phone()
        {
            // Arrange
            var newPhone = new Phone { Id = 1, Name = "Test_Phone" , Price = 100};
            var newPhone2 = new Phone { Id = 2, Name = "Test_Phone2", Price = 200 };
            // Act
            bool result = _phoneMoqService.AddPhone(newPhone);
            bool result2 = _phoneMoqService.AddPhone(newPhone2);

            //_mockService.Setup(p => p.Add(newPhone));
            //_mockService.Setup(p => p.Add(newPhone2));

            var PhoneList = _phoneMoqService.GetAll();
            // Assert

            //Assert.True(result,"Failed to add phone");


            PhoneList.Should().NotBeNull();

            PhoneList.Count.Should().Be(2);

        }

        [Fact]
        public void Remove_Phone()
        {   
            // Arrange
            var newPhone = new Phone { Id = 1, Name = "Test_Phone", Price = 100 };
            var newPhone2 = new Phone { Id = 2, Name = "Test_Phone2", Price = 200 };
            // Act
            bool Addresult = _phoneMoqService.AddPhone(newPhone);
            bool Addresult2 = _phoneMoqService.AddPhone(newPhone2);
            var RemoveResult = _phoneMoqService.DeletePhone(newPhone.Id);
            var PhoneList = _phoneMoqService.GetAll();
            // Assert
            RemoveResult.Should().BeTrue();
            PhoneList.Count.Should().Be(1);
            //Assert.True(result,"Failed to add phone");

        }
    }
}
