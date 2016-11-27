## Testing Standards

## Table of Contents
* abc
* def

### Server Testing 
1. Integration tests exist for the Launchpad.Data
   - The primary goal of these tests is to excercise the EntityFramework configuration and repositories
2. There should be a mirrored structure between project files and test files
   - This allows the developer to locate the test file quickly based on the location of the project file
3. The tests use behavior and state verification
   - Use MOQ to setup expectations and verify the behavior
   - There is a BaseUnitTest class which provides a MockRepository and AutFixture.Fixture member to the 
     derived class
4. Use interfaces for dependencies in order to loosely couple components and make code mock-able
5. When possible avoid statics to promote testable components

### Testing Stack
The current testing stack for the server is found below. 

#### Server Tests
1. [Xunit](https://xunit.github.io/)
2. [AutoFixture](https://github.com/AutoFixture/AutoFixture)
3. [FluentAssertions](http://www.fluentassertions.com/)
4. [MOQ](https://github.com/moq/moq4)
