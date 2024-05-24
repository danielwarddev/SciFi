using Amazon.CDK;
using Constructs;
using Lambda = Amazon.CDK.AWS.Lambda;
using S3 = Amazon.CDK.AWS.S3.Assets;

namespace SciFi.Infrastructure;

public class DotNet8Function : Lambda.Function
{
    public DotNet8Function(Construct scope, string id, DotNet8FunctionProps props) : base(scope, id, props) { }
}

public class DotNet8FunctionProps : Lambda.FunctionProps
{
    public DotNet8FunctionProps(string codeAssetPath, string handlerName)
    {
        Runtime = Lambda.Runtime.DOTNET_8;
        Handler = handlerName;
        Code = Lambda.Code.FromAsset(codeAssetPath, new S3.AssetOptions
        {
            Bundling = new BundlingOptions
            {
                Image = Lambda.Runtime.DOTNET_8.BundlingImage,
                Command = new string[]
                {
                    "/bin/sh",
                    "-c",
                    " dotnet tool install -g Amazon.Lambda.Tools" +
                    " && dotnet build" +
                    " && dotnet lambda package --output-package /asset-output/function.zip"
                }
            }
        });
    }
}