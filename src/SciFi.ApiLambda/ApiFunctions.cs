using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SciFi.ApiLambda;

public class ApiFunctions
{
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Get, "/")]
    public int RetrieveNumber()
    {
        return 5;
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/add/{x}/{y}")]
    public int Add(int x, int y)
    {
        return x + y;
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/lower")]
    public string Lower([FromBody] string str)
    {
        return str.ToLower();
    }
}
