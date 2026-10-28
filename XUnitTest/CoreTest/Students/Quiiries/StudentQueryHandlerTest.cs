using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using SchoolProject.Core.Features.Students.Queries.Handlers;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Core.Mapping.Students;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Enums;
using SchoolProject.Service.Abstracts;
using EntityFrameworkCore.Testing.Common;//AsyncEnumerable<T> to Support Async List IQueryable<T> in Unit Tests
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, MaxParallelThreads = 6)]
namespace XUnitTest.CoreTest.Students.Quiiries
{
    public class StudentQueryHandlerTest
    {
        #region Fields
        private readonly Mock<IStudentService> _studentServiceMock;
        private readonly IMapper _mapperMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly StudentProfile _studentProfile;
        #endregion

        #region Constructors
        public StudentQueryHandlerTest()
        {
            _studentServiceMock = new Mock<IStudentService>();
            _studentProfile = new StudentProfile();
            _mapperMock = new MapperConfiguration(x => x.AddProfile(_studentProfile)).CreateMapper();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            //var MapperConfiguration = new MapperConfiguration(x => x.AddProfile(_studentProfile));
            //_mapperMock = new Mapper(MapperConfiguration);

        }
        #endregion

        #region Methods[Fact]
        [Fact]
        public async Task Test_StudentQueryHandler_GetList_NotNull_Or_Empty()
        {
            // Arrange
            var StudentList = new List<Student>
            {
                new Student { StudID = 1, NameAr = "Student 1", NameEn = "Student 1", Address = "Address 1", Phone = "1234567890", DID = 1 },
                new Student { StudID = 2, NameAr = "Student 2", NameEn = "Student 2", Address = "Address 2", Phone = "0987654321", DID = 2 }
            };
            var query = new GetStudentListQuery();
            var queryHandler = new StudentQueryHandler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            // Act
            _studentServiceMock.Setup(x => x.GetStudentsListAsync()).ReturnsAsync(StudentList);
            var result = await queryHandler.Handle(query, CancellationToken.None);
            // Assert
            result.Data.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeOfType<List<GetStudentListResponse>>();

        }

        [Theory]
        //[InlineData(1, null)]
        //[InlineData(2, null)]
        [InlineData(3, null)]

        public async Task Test_StudentQueryHandler_Returns_NotFound404(int studentId, CancellationToken cancellationToken)
        {
            // Arrange
            var StudentList = new List<Student>
            {
                new Student { StudID = 1, NameAr = "Student 1", NameEn = "Student 1", Address = "Address 1", Phone = "1234567890", DID = 1 , Department = new Department()
                {
                    DID = 1, DNameAr = "Department 1", DNameEn = "Department 1" }
                },
                new Student { StudID = 2, NameAr = "Student 2", NameEn = "Student 2", Address = "Address 2", Phone = "0987654321", DID = 2 , Department = new Department()
                {
                    DID = 2, DNameAr = "Department 2", DNameEn = "Department 2" }
                }
            };
            var query = new GetStudentByIDQuery(studentId );
            var queryHandler = new StudentQueryHandler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            // Act
            _studentServiceMock.Setup(x => x.GetStudentByIDWithIncludeAsync(studentId)).ReturnsAsync(StudentList.FirstOrDefault(s => s.StudID == studentId));

            var result = await queryHandler.Handle(query, cancellationToken);
            // Assert
            //result.Data.Should().NotBeNull();
            //result.Succeeded.Should().BeTrue();
            //result.Data.Should().BeOfType<GetSingleStudentResponse>();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Theory]
        //[InlineData(1, null)]
        //[InlineData(2, null)] ClassMemberData
        //[ClassData(typeof(ClassDataPass))]
        [MemberData(nameof(ClassMemberData.GetParamData), MemberType = typeof(ClassMemberData))]
        public async Task Test_StudentQueryHandler_Returns_Found200(int studentId, CancellationToken cancellationToken)
        {
            // Arrange
            var StudentList = new List<Student>
            {
                new Student { StudID = 1, NameAr = "Student 1", NameEn = "Student 1", Address = "Address 1", Phone = "1234567890", DID = 1 , Department = new Department()
                {
                    DID = 1, DNameAr = "Department 1", DNameEn = "Department 1" }
                },
                new Student { StudID = 2, NameAr = "Student 2", NameEn = "Student 2", Address = "Address 2", Phone = "0987654321", DID = 2 , Department = new Department()
                {
                    DID = 2, DNameAr = "Department 2", DNameEn = "Department 2" }
                }
            };
            var query = new GetStudentByIDQuery(studentId);
            var queryHandler = new StudentQueryHandler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            // Act
            _studentServiceMock.Setup(x => x.GetStudentByIDWithIncludeAsync(query.Id)).ReturnsAsync(StudentList.FirstOrDefault(s => s.StudID == studentId));

            var result = await queryHandler.Handle(query, cancellationToken);
            // Assert
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeOfType<GetSingleStudentResponse>();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.StudID.Should().Be(studentId);

        }

        [Theory]
        [InlineData(1, 10, null)]
        public async Task Test_StudentQueryHandle_PaginationList(int studentId, int pageNumber, CancellationToken cancellationToken)
        {
            var StudentList =new AsyncEnumerable<Student>(new List<Student>
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
                    DID = 3, DNameAr = "Department 3", DNameEn = "Department 3" }
                }}
            );

            var query = new GetStudentPaginatedListQuery() { PageNumber = 1, PageSize = 10, OrderBy = StudentOrderingEnum.StudID, Search = "Student 1" };
            var queryHandler = new StudentQueryHandler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            _studentServiceMock.Setup(x => x.FilterStudentPaginatedQuerable(query.OrderBy, query.Search))
                .Returns(StudentList.AsQueryable());

            var result = await queryHandler.Handle(query, cancellationToken);

            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeOfType<List<GetStudentPaginatedListResponse>>();
            //result.Data.Items.Should().ContainSingle(s => s.StudID == studentId);
        }

        #endregion
    }
}
