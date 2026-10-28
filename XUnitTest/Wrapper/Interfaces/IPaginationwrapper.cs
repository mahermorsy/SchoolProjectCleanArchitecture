using SchoolProject.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTest.Wrapper.Interfaces
{
    public interface IPaginationwrapper<T> where T : class
    {
        public Task<PaginatedResult<T>> ToPaginatedListAsync(IQueryable<T> source, int pageNumber, int pageSize);
    }
}
