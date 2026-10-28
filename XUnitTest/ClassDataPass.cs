using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XUnitTest
{
    public class ClassDataPass : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { 1,CancellationToken.None },
            new object[] { 2,CancellationToken.None },
  
        };

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { 1 , CancellationToken.None };
            yield return new object[] { 2 , CancellationToken.None };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
