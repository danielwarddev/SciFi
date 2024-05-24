using Amazon.CDK;
using Constructs;
using SciFi.SimpleLambda;
using Dynamo = Amazon.CDK.AWS.DynamoDB;
using System.Collections.Generic;
using SciFi.DynamoLambda;
using ApiGw = Amazon.CDK.AWS.APIGateway;
using SciFi.ApiLambda;

namespace SciFi.Infrastructure;

public class SciFiStackProps : StackProps { public Dynamo.Table MyTable { get; init; } }

public class SciFiStack : Stack
{
    internal SciFiStack(Construct scope, string id, SciFiStackProps props) : base(scope, id, props)
    {
        var simpleNames = Helpers.GetLambdaAssetNames(typeof(SimpleFunction), nameof(SimpleFunction.FunctionHandler));
        var simpleFunction = new DotNet8Function(this, "simpleFunction",
            new DotNet8FunctionProps($"./src/{simpleNames.AssemblyName}", simpleNames.HandlerName));

        var dynamoNames = Helpers.GetGeneratedLambdaAssetNames(typeof(DynamoFunction), nameof(DynamoFunction.FunctionHandler));
        var dynamoFunction = new DotNet8Function(this, "dynamoFunction",
            new DotNet8FunctionProps($"./src/{dynamoNames.AssemblyName}", dynamoNames.HandlerName)
            {
                Environment = new Dictionary<string, string>
                {
                    { "TABLE_NAME", props.MyTable.TableName }
                },
                Timeout = Duration.Seconds(10)
            });
        props.MyTable.GrantWriteData(dynamoFunction);

        var rootAssetNames = Helpers.GetGeneratedLambdaAssetNames(typeof(ApiFunctions), nameof(ApiFunctions.RetrieveNumber));
        var rootFunction = new DotNet8Function(this, "rootFunction",
            new DotNet8FunctionProps($"./src/{rootAssetNames.AssemblyName}", rootAssetNames.HandlerName));

        var addAssetNames = Helpers.GetGeneratedLambdaAssetNames(typeof(ApiFunctions), nameof(ApiFunctions.Add));
        var addFunction = new DotNet8Function(this, "addFunction",
            new DotNet8FunctionProps($"./src/{addAssetNames.AssemblyName}", addAssetNames.HandlerName));

        var lowerAssetNames = Helpers.GetGeneratedLambdaAssetNames(typeof(ApiFunctions), nameof(ApiFunctions.Lower));
        var lowerFunction = new DotNet8Function(this, "lowerFunction",
            new DotNet8FunctionProps($"./src/{lowerAssetNames.AssemblyName}", lowerAssetNames.HandlerName));

        var api = new ApiGw.LambdaRestApi(this, "lambdaApi", new ApiGw.LambdaRestApiProps
        {
            Handler = rootFunction
        });
        api.Root.AddResource("add").AddResource("{x}").AddResource("{y}").AddMethod("POST", new ApiGw.LambdaIntegration(addFunction));
        api.Root.AddResource("lower").AddMethod("POST", new ApiGw.LambdaIntegration(lowerFunction));
    }
}
