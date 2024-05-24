using Amazon.CDK;

namespace SciFi.Infrastructure.Infrastructure;

sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        var statefulStack = new StatefulStack(app, "StatefulStack");
        
        new SciFiStack(app, "SciFiStack", new SciFiStackProps { MyTable = statefulStack.MyTable });

        app.Synth();
    }
}
