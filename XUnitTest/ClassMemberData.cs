using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XUnitTest
{
    public class ClassMemberData : IEnumerable<object[]>
    {
        public static IEnumerable<object[]> GetParamData()
        {
            return new List<object[]> {
                new object[] { 1, CancellationToken.None },
                new object[] { 2, CancellationToken.None }
            };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return GetParamData().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
