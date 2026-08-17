using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SmellyShoppingCartKata.Tests;

// Infrastructure only -- not part of the kata's deliberate smells.
//
// xUnit does not guarantee test execution order within a class, and
// Meziantou.Xunit.ParallelTestFramework's [DisableParallelization] only
// stops tests from running *concurrently*; it does not pin them to
// declaration order either. The TS source relies on Jest's default
// sequential, declaration-order execution within a describe block to make
// the deliberate "Test Interdependence" / "Shared Mutable State" smell
// reproduce the same way every run. This orderer restores that same
// declaration-order guarantee for the ported smelly test classes. See
// PORTING_NOTES_CS.md for details.
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class TestPriorityAttribute(int priority) : Attribute
{
    public int Priority { get; } = priority;
}

public sealed class PriorityOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        return testCases
            .OrderBy(testCase => GetPriority(testCase))
            .ThenBy(testCase => testCase.TestMethod.Method.Name, StringComparer.Ordinal);
    }

    private static int GetPriority(ITestCase testCase)
    {
        var attribute = testCase.TestMethod.Method
            .ToRuntimeMethod()
            .GetCustomAttribute<TestPriorityAttribute>();

        return attribute?.Priority ?? int.MaxValue;
    }
}
