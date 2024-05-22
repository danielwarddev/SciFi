using Amazon.CDK;

namespace SciFi
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new SciFiStack(app, "SciFiStack");

            app.Synth();
        }
    }
}
