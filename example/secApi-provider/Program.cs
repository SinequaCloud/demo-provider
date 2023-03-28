using Pulumi.Secapiprovider;
using Pulumi;

return await Pulumi.Deployment.RunAsync(() =>
{
    // Create an Azure Resource Group
    var groups = new SecAPI("SecAPIProvider", new SecAPIArgs { Env = "dev", Org = "pooq" }); 

    // Export the primary key of the Storage Account
    return new Dictionary<string, object?>
    {
    };
});