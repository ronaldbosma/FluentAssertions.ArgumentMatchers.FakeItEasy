using System;
using System.Collections.Generic;
using System.Text;

namespace FluentAssertions.ArgumentMatchers.FakeItEasy.Tests.TestTools
{
    public interface IInterface
    {
        void DoSomething(ComplexType complexType);

        void DoSomethingWithCollection(IEnumerable<ComplexType> complexType);
    }
}
