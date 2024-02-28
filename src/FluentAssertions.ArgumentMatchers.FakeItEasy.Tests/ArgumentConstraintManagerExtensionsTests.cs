using System;
using System.Collections.Generic;
using AutoFixture;
using FakeItEasy;
using FluentAssertions.ArgumentMatchers.FakeItEasy.Tests.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.ArgumentMatchers.FakeItEasy.Tests
{
    [TestClass]
    public class ArgumentConstraintManagerExtensionsTests
    {
        private Fixture _fixture;
        private IInterface _fake;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _fake = A.Fake<IInterface>();
        }

        [TestMethod]
        public void IsEquivalentTo_Matches_Same_Complex_Types()
        {
            var complexType = _fixture.Create<ComplexType>();

            _fake.DoSomething(complexType);

            A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo(complexType))).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void IsEquivalentTo_Matches_Two_Different_Complex_Types_With_Same_Data()
        {
            var complexType = _fixture.Create<ComplexType>();
            var expectedComplexType = complexType.Copy();

            _fake.DoSomething(complexType);

            A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo(expectedComplexType))).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void IsEquivalentTo_Does_Not_Match_Two_Complex_Types_If_Child_Property_Has_Different_Value()
        {
            var complexType = _fixture.Create<ComplexType>();

            var expectedComplexType = complexType.Copy();
            expectedComplexType.ComplexTypeProperty.IntProperty++;

            _fake.DoSomething(complexType);

            Action verify = () => A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo(expectedComplexType))).MustHaveHappenedOnceExactly();
            verify.Should().Throw<ExpectationException>();
        }

        [TestMethod]
        public void IsEquivalentTo_Matches_Two_Complex_Types_If_Child_Property_Has_Different_Value_But_Its_Ignored()
        {
            var complexType = _fixture.Create<ComplexType>();

            var expectedComplexType = complexType.Copy();
            expectedComplexType.ComplexTypeProperty.IntProperty++;

            _fake.DoSomething(complexType);

            A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo(
                expectedComplexType,
                options => options.Excluding(c => c.ComplexTypeProperty.IntProperty)
            ))).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void IsEquivalentTo_Matches_If_Actual_And_Expected_Are_Null()
        {
            _fake.DoSomething(null);

            A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo<ComplexType>(null))).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void IsEquivalentTo_Does_Not_Match_If_Actual_Object_Has_Value_And_Expected_Object_Is_Null()
        {
            var complexType = _fixture.Create<ComplexType>();

            _fake.DoSomething(complexType);

            Action verify = () => A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo<ComplexType>(null))).MustHaveHappenedOnceExactly();
            verify.Should().Throw<ExpectationException>();
        }

        [TestMethod]
        public void IsEquivalentTo_Does_Not_Match_If_Actual_Object_Is_Null_And_Expected_Object_Has_Value()
        {
            var expectedComplexType = _fixture.Create<ComplexType>();

            _fake.DoSomething(null);

            Action verify = () => A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo(expectedComplexType))).MustHaveHappenedOnceExactly();
            verify.Should().Throw<ExpectationException>();
        }

        [TestMethod]
        public void IsEquivalentTo_Matches_Two_Different_Types_With_Same_Data()
        {
            var list = new List<ComplexType> { _fixture.Create<ComplexType>() };

            _fake.DoSomethingWithCollection(list.ToArray());

            A.CallTo(() => _fake.DoSomethingWithCollection(A<IEnumerable<ComplexType>>.That.IsEquivalentTo(list)))
                .MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void IsEquivalentTo_Does_Not_Match_Two_Collections_When_Child_Property_Has_Different_Value()
        {
            var complexType = _fixture.Create<ComplexType>();
            var list = new List<ComplexType> { complexType };

            var expectedComplexType = complexType.Copy();
            // Change a property of the expected object to make it different from the actual object
            expectedComplexType.ComplexTypeProperty.IntProperty++;
            var expectedList = new List<ComplexType> { expectedComplexType };

            _fake.DoSomethingWithCollection(list);

            Action verify = () => A.CallTo(() => _fake.DoSomethingWithCollection(A<List<ComplexType>>.That.IsEquivalentTo(expectedList)))
                .MustHaveHappenedOnceExactly();
            verify.Should().Throw<ExpectationException>();
        }

        [TestMethod]
        public void IsEnumerableEquivalentTo_Matches_Two_Collections_When_Child_Property_Has_Different_Value_But_Its_Ignored()
        {
            var complexType = _fixture.Create<ComplexType>();
            var list = new List<ComplexType> { complexType };

            var expectedComplexType = complexType.Copy();
            // Change a property of the expected object to make it different from the actual object
            expectedComplexType.ComplexTypeProperty.IntProperty++;
            var expectedList = new List<ComplexType> { expectedComplexType };

            _fake.DoSomethingWithCollection(list);

            A.CallTo(() => _fake.DoSomethingWithCollection(A<IEnumerable<ComplexType>>.That.IsEnumerableEquivalentTo(
                expectedList,
                options => options.Excluding(c => c.ComplexTypeProperty.IntProperty)
            ))).MustHaveHappenedOnceExactly();
        }
    }
}
