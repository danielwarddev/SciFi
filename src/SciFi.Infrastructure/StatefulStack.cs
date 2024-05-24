using Amazon.CDK;
using Kms = Amazon.CDK.AWS.KMS;
using Constructs;
using Dynamo = Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.KMS;

namespace SciFi.Infrastructure;

public class StatefulStack : Stack
{
    public readonly Dynamo.Table MyTable;

    public StatefulStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        TerminationProtection = true;

        var key = new Kms.Key(this, "tableKey", new KeyProps
        {
            EnableKeyRotation = true
        });

        var table = new Dynamo.Table(this, "myTable", new Dynamo.TableProps
        {
            PartitionKey = new Dynamo.Attribute
            {
                Name = "id",
                Type = Dynamo.AttributeType.STRING
            },
            RemovalPolicy = RemovalPolicy.RETAIN,
            BillingMode = Dynamo.BillingMode.PAY_PER_REQUEST,
            Encryption = Dynamo.TableEncryption.CUSTOMER_MANAGED,
            EncryptionKey = key
        });

        Amazon.CDK.Tags.Of(table).Add("my-key", "my-value");

        MyTable = table;
    }
}
