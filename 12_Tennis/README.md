# Tennis kata

## Source

<https://github.com/emilybache/Tennis-Refactoring-Kata>

## Problem Statement

You are taking over a tennis scoring system project from a colleague who:

- Has spent 8.5 hours of a 10-hour billable project
- Claims the work is complete with passing tests
- Is currently unavailable due to illness

Your manager has requested you to:

1. Spend the remaining billable time (~1 hour) improving the code
2. Prepare feedback on the current design
3. Be ready to discuss the value of refactoring beyond billable hours

## Your task

Refactor the code to improve its readability.

### Guidelines

#### Before you start

- Make sure the tests have a good coverage
  - Use a code coverage tool to make sure code coverage by tests is very high
  - For this exercise code coverage is high (100%)

##### Execute tests

```sh
dotnet test
```

##### Execute tests with coverage

```sh
dotnet test --collect:"XPlat Code Coverage;IncludeTestAssembly=true;Format=cobertura"
```

```txt
--------------------------|---------|----------|---------|---------|-------------------
File                      | % Stmts | % Branch | % Funcs | % Lines | Uncovered Line #s
--------------------------|---------|----------|---------|---------|-------------------
All files                 |   86.28 |    67.74 |   96.61 |   86.03 |
 10_Tennis                |     100 |      100 |     100 |     100 |
  kata.ts                 |     100 |      100 |     100 |     100 |
...
```

#### While refactoring

- Stay in the green while refactoring
  - Run the tests after each refactor
    - Check all tests still pass
    - Check code coverage has not dropped
- Commit after each refactor
- In case of persistent test fails, use `git reset` to go back to green

#### Improve readability

1. Tackle clutter by
   - Formatting the code, a simple and very effective technique
     - Format consistently and don’t force the reader to waste timed due to inconsistent formatting
   - Renaming bad names or abbreviations on variables, arguments, instance variables, methods, and classes
     - <https://www.digdeeproots.com/articles/naming-process/>
2. Tackle Comments and Dead Code by
   - Deleting useless comments
   - Deleting useful comments by extracting a method named after them
   - Deleting dead code
     - Don’t make the reader waste time figuring out code that is not used
3. Tackle implicit knowledge by
   - Extracting constants from magic numbers and strings
   - Extracting complex conditional expressions
4. Tackle scattering by
   - Refinining the scope for variables
   - Ensuring variables are declared close to where they are used
   - Grouping public methods at the top of the class to show first what matters the most

#### Reduce complexity

1. Tackle complexity by
   - Extracting smaller private methods from long methods
     - Also encapsulate any cryptic code (that cannot be made more explicit) in private methods
   - Extracting private methods from nested code blocks
   - Returning from methods as soon as possible
2. Tackle duplication by
   - Removing duplicated knowledge

## Useful tools

Resharper or Rider
