using System;

namespace SciFi.Infrastructure;

public record LambdaAssetNames(string AssemblyName, string HandlerName);

public static class Helpers
{
    public static LambdaAssetNames GetLambdaAssetNames(Type type, string handlerFunctionName)
    {
        var assemblyName = type.Assembly.GetName().Name!;
        return new LambdaAssetNames(assemblyName, $"{assemblyName}::{type.FullName}::{handlerFunctionName}");
    }

    public static LambdaAssetNames GetGeneratedLambdaAssetNames(Type type, string handlerFunctionName)
    {
        var assemblyName = type.Assembly.GetName().Name!;
        return new LambdaAssetNames(assemblyName, $"{assemblyName}::{type.FullName}_{handlerFunctionName}_Generated::{handlerFunctionName}");
    }
}
