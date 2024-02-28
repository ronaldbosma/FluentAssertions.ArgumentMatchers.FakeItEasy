![Nuget](https://img.shields.io/nuget/dt/FluentAssertions.ArgumentMatchers.FakeItEasy)

FluentAssertions.ArgumentMatchers.FakeItEasy
===

The [FluentAssertions.ArgumentMatchers.FakeItEasy NuGet package](https://www.nuget.org/packages/FluentAssertions.ArgumentMatchers.FakeItEasy/) provides a simple way to use FakeItEasy in combination with FluentAssertions to compare complex objects.

### IsEquivalentTo

The package has an extension method called `IsEquivalentTo` for `IArgumentConstraintManager<T>`. It can be used in the setup and verify stages of a Fake similar to other argument matchers like `A<string>.Ignored`. The `actual.Should().BeEquivalentTo(expected)` method is used inside to compare objects. An overload is available so you can pass in configuration to FluentAssertions.

#### Examples
```csharp
A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo(expectedComplexType))).Returns(result);

A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo(expectedComplexType))).MustHaveHappenedOnceExactly();

A.CallTo(() => _fake.DoSomething(A<ComplexType>.That.IsEquivalentTo(
    expectedComplexType, 
    options => options.Excluding(c => c.SomeProperty)
))).MustHaveHappenedOnceExactly();
```

### IsEnumerableEquivalentTo

When comparing collections and certain properties need to be ignored, the `IsEnumerableEquivalentTo` method can be used. The code `actual.Should().BeAssignableTo<IEnumerable<TValue>>().Which.Should().BeEquivalentTo(expected, config)` is used inside to compare the collections. It's important to note that you need to explicitly use `IEnumerable` in `A<IEnumerable<T>>.That.IsEnumerableEquivalentTo` for it to work.

#### Examples

```csharp
// Ignore the 'ComplexTypeProperty.IntProperty' property of the ComplexType objects in the collection
A.CallTo(() => _fake.DoSomethingWithCollection(A<IEnumerable<ComplexType>>.That.IsEnumerableEquivalentTo(
    expectedList,
    options => options.Excluding(c => c.ComplexTypeProperty.IntProperty)
))).MustHaveHappenedOnceExactly();
```