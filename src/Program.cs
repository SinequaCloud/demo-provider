using System.Threading;
using System.Threading.Tasks;
using Pulumi.Experimental.Provider;


public static class Program
{
    public static Task Main(string[] args)
    {
        return Provider.Serve(args, host => new SecApiProvider(host), CancellationToken.None);
    }
}
