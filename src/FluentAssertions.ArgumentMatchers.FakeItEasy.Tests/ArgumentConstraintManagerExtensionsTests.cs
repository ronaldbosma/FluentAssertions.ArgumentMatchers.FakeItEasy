using System;
using System.Diagnostics;
using AutoFixture;
using FakeItEasy;
using FluentAssertions.ArgumentMatchers.FakeItEasy.Tests.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.ArgumentMatchers.FakeItEasy.Tests
{
    /// <summary>
    /// This <see cref="ArgumentConstraintManagerExtensionsTests"/>, <seealso cref="ComplexType"/> and <seealso cref="IInterface"/>
    /// are reused by the test solution in the tests folder.
    /// </summary>
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

            // Print the FakeItEasy and FluentAssertions assembly version so we can check what version is used.
            Trace.WriteLine($"{typeof(Fake).AssemblyQualifiedName}");
            Trace.WriteLine($"{typeof(FluentAssertions.TypeExtensions).AssemblyQualifiedName}");
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
    }
}
