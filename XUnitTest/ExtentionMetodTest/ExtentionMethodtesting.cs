using EntityFrameworkCore.Testing.Common;
using FluentAssertions;
using Moq;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTest.Wrapper.Interfaces;

namespace XUnitTest.ExtentionMetodTest
{
   
    public class ExtentionMethodtesting
    {
        private readonly Mock<IPaginationwrapper<Student>> _paginationWrapper;

        public ExtentionMethodtesting(IPaginationwrapper<Student> paginationWrapper)
        {
            _paginationWrapper = new Mock<IPaginationwrapper<Student>>();
        }

        [Theory]
        [InlineData(1, 10)]
        public async Task Test_ToPaginatedListAsync_Returns_PaginatedResult(int pageNumber, int pageSize)
        {
            // Arrange
            var StudentList = new AsyncEnumerable<Student>(new List<Student>
            {   new Student { StudID = 1, NameAr = "Student 1", NameEn = "Student 1", Address = "Address 1", Phone = "1234567890", DID = 1 , Department = new Department()
                {
                    DID = 1, DNameAr = "Department 1", DNameEn = "Department 1" }
                },
                new Student { StudID = 2, NameAr = "Student 2", NameEn = "Student 2", Address = "Address 2", Phone = "0987654321", DID = 2 , Department = new Department()
                {
                    DID = 2, DNameAr = "Department 2", DNameEn = "Department 2" }
                },
                new Student { StudID = 3, NameAr = "Student 3", NameEn = "Student 3", Address = "Address 3", Phone = "1111111111", DID = 3 , Department = new Department()
                {
                    DID = 3, DNameAr = "Department 3", DNameEn = "Department 3"  }
                }
            }); 
     
            // Act PaginatedResult

            _paginationWrapper.Setup(P => P.ToPaginatedListAsync(StudentList, pageNumber, pageSize))
                .ReturnsAsync(new PaginatedResult<Student>(StudentList.ToList()));

            var result = await _paginationWrapper.Object.ToPaginatedListAsync(StudentList, pageNumber, pageSize);

            // Assert
            result.Data.Should().NotBeNullOrEmpty();   
         
        }
    }
}
