using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

namespace SciFi.SimpleLambda.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new SimpleFunction();
        var context = new TestLambdaContext();
        var upperCase = function.FunctionHandler("hello world");

        Assert.Equal("HELLO WORLD", upperCase);
    }
}
