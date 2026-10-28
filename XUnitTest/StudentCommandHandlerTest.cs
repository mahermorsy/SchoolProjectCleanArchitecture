using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using SchoolProject.Core.Features.Students.Commands.Handlers;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Mapping.Students;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTest
{
    public class StudentCommandHandlerTest
    {
        #region Fields
        private readonly Mock<IStudentService> _studentServiceMock;
        private readonly IMapper _mapperMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly StudentProfile _studentProfile;
        #endregion

        #region Constructors
        public StudentCommandHandlerTest()
        {
            _studentServiceMock = new Mock<IStudentService>();
            _studentProfile = new StudentProfile();
            _mapperMock = new MapperConfiguration(x => x.AddProfile(_studentProfile)).CreateMapper();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();
        }
        #endregion

        [Fact]
        public async Task Test_Handle_AddStudent_ReturnsExpectedResult201()
        {
            // Arrange
            var Handler = new StudentCommandHandler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var newstudent = new AddStudentCommand() { NameEn = "John Doe", NameAr = "جون دو"};
            _studentServiceMock.Setup(x => x.AddAsync(It.IsAny<Student>())).Returns(Task.FromResult("Success"));
            // Act
            var Result  = await Handler.Handle(newstudent, CancellationToken.None);
            // Assert
            Result.Succeeded.Should().BeTrue();
            Result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            //Verify that the AddAsync method was called once
            _studentServiceMock.Verify(x => x.AddAsync(It.IsAny<Student>()), Times.Once);

        }

        [Fact]
        public async Task Test_Handle_AddStudent_ReturnsExpectedResult400()
        {
            // Arrange
            var Handler = new StudentCommandHandler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var newstudent = new AddStudentCommand() { NameEn = "John Doe", NameAr = "جون دو" };
            _studentServiceMock.Setup(x => x.AddAsync(It.IsAny<Student>())).Returns(Task.FromResult("Failed"));
            // Act
            var Result = await Handler.Handle(newstudent, CancellationToken.None);
            // Assert
            Result.Succeeded.Should().BeFalse();
            Result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            //Verify that the AddAsync method was called once
            _studentServiceMock.Verify(x => x.AddAsync(It.IsAny<Student>()), Times.Once);

        }

        [Fact]
        public async Task Test_Handle_EditStudent_ReturnsExpectedResult404()
        {
            // Arrange
            var Handler = new StudentCommandHandler(_studentServiceMock.Object, _mapperMock, _localizerMock.Object);
            var Updatestudent = new EditStudentCommand() { Id = 6, NameEn = "John Doe", NameAr = "جون دو" };
            Student? student = null;
            int CallbackId = 0;

            _studentServiceMock.Setup(x => x.GetByIDAsync(Updatestudent.Id)).Returns(Task.FromResult((Student)null)).Callback<int>(id => { CallbackId = id;  });


            // Act
            var Result = await Handler.Handle(Updatestudent, CancellationToken.None);

            // Assert
            Result.Succeeded.Should().BeFalse();
            Result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            CallbackId.Should().Be(6);
            Assert.Equal(Updatestudent.Id, CallbackId);

            //Verify that the GetByIDAsync method was called once
            _studentServiceMock.Verify(x => x.GetByIDAsync(Updatestudent.Id), Times.Once);

        }
    }
}

