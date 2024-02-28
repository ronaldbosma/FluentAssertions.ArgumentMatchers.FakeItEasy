using System;
using System.Collections.Generic;
using System.Diagnostics;
using FakeItEasy;
using FluentAssertions.Equivalency;

namespace FluentAssertions.ArgumentMatchers.FakeItEasy
{
    /// <summary>
    /// Contains extension methods for <seealso cref="IArgumentConstraintManager{T}"/>.
    /// </summary>
    public static class ArgumentConstraintManagerExtensions
    {
        /// <summary>
        /// Matches any value that is equivalent to <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="T">Type of the argument to check.</typeparam>
        /// <param name="mgr">The <seealso cref="IArgumentConstraintManager{T}"/> with the actual object to match.</param>
        /// <param name="expected">The expected object to match.</param>
        /// <returns>A dummy argument value.</returns>
        public static T IsEquivalentTo<T>(this IArgumentConstraintManager<T> mgr, T expected)
        {
            return mgr.IsEquivalentTo(expected, config => config);
        }

        /// <summary>
        /// Matches any value that is equivalent to <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="T">Type of the argument to check.</typeparam>
        /// <param name="mgr">The <seealso cref="IArgumentConstraintManager{T}"/> with the actual object to match.</param>
        /// <param name="expected">The expected object to match.</param>
        /// <param name="config">
        /// A reference to the <seealso cref="EquivalencyAssertionOptions{T}"/> configuration object
        /// that can be used to influence the way the object graphs are compared.
        /// You can also provide an alternative instance of the <seealso cref="EquivalencyAssertionOptions{T}"/> class.
        /// The global defaults are determined by the <seealso cref="AssertionOptions"/> class.
        /// </param>
        /// <returns>A dummy argument value.</returns>
        public static T IsEquivalentTo<T>(this IArgumentConstraintManager<T> mgr,
            T expected,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config)
        {
            if (mgr == null)
            {
                throw new ArgumentNullException(nameof(mgr));
            }

            return mgr.Matches(actual => AreEquivalent(actual, expected, config));
        }

        private static bool AreEquivalent<TValue>(TValue actual, TValue expected,
            Func<EquivalencyAssertionOptions<TValue>, EquivalencyAssertionOptions<TValue>> config)
        {
            try
            {
                actual.Should().BeEquivalentTo(expected, config);
                return true;
            }
            catch (Exception ex)
            {
                // Although catching an Exception to return false is a bit ugly
                // the great advantage is that we can log the error message of FluentAssertions.
                // This makes it easier to troubleshoot why a Fake was not called with the expected parameters.

                Trace.WriteLine($"Actual and expected of type {typeof(TValue)} are not equal. Details:");
                Trace.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Matches any enumerable value that is equivalent to <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="T">Type of the items of the enumerable to check.</typeparam>
        /// <param name="mgr">The <seealso cref="IArgumentConstraintManager{T}"/> with the actual object to match.</param>
        /// <param name="expected">The expected object to match.</param>
        /// <param name="config">
        /// A reference to the <seealso cref="EquivalencyAssertionOptions{T}"/> configuration object
        /// that can be used to influence the way the object graphs are compared.
        /// You can also provide an alternative instance of the <seealso cref="EquivalencyAssertionOptions{T}"/> class.
        /// The global defaults are determined by the <seealso cref="AssertionOptions"/> class.
        /// </param>
        /// <returns>A dummy argument value.</returns>
        public static IEnumerable<T> IsEnumerableEquivalentTo<T>(this IArgumentConstraintManager<IEnumerable<T>> mgr,
            IEnumerable<T> expected,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config)
        {
            if (mgr == null)
            {
                throw new ArgumentNullException(nameof(mgr));
            }

            return mgr.Matches(actual => AreEnumerablesEquivalent(actual, expected, config));
        }

        private static bool AreEnumerablesEquivalent<TValue>(object actual, IEnumerable<TValue> expected,
            Func<EquivalencyAssertionOptions<TValue>, EquivalencyAssertionOptions<TValue>> config)
        {
            try
            {
                actual.Should().BeAssignableTo<IEnumerable<TValue>>().Which.Should().BeEquivalentTo(expected, config);
                return true;
            }
            catch (Exception ex)
            {
                // Although catching an Exception to return false is a bit ugly
                // the great advantage is that we can log the error message of FluentAssertions.
                // This makes it easier to troubleshoot why a Fake was not called with the expected parameters.

                Trace.WriteLine($"Actual and expected of type {typeof(IEnumerable<TValue>)} are not equal. Details:");
                Trace.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
