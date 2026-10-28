using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTest.Wrapper.Interfaces;

namespace XUnitTest.Wrapper.Implementations
{
    public class Paginationwrapper : IPaginationwrapper<Student>
    {

        public async Task<PaginatedResult<Student>> ToPaginatedListAsync(IQueryable<Student> source, int pageNumber, int pageSize)
        {
          return await source.ToPaginatedListAsync(pageNumber, pageSize);
        }
    }
}
