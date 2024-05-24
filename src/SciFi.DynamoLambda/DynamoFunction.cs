using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SciFi.DynamoLambda;

public class DynamoFunction
{
    private readonly IAmazonDynamoDB _dynamoClient;

    public DynamoFunction(IAmazonDynamoDB dynamoClient)
    {
        _dynamoClient = dynamoClient;
    }

    [LambdaFunction]
    public async Task<string> FunctionHandler(string item)
    {
        await _dynamoClient.PutItemAsync(new PutItemRequest
        {
            TableName = Environment.GetEnvironmentVariable("TABLE_NAME"),
            Item = new Dictionary<string, AttributeValue>
        {
            { "id", new AttributeValue { S = Guid.NewGuid().ToString() } },
            { "value", new AttributeValue { S = item } }
        }
        });

        return item.ToUpper();
    }
}
