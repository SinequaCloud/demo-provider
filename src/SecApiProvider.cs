using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.Experimental.Provider;
using Newtonsoft.Json.Linq;

class SecurityProxyClient
{
    private static HttpClient httpClient = null;
    private static string token = null;

    private static string GetToken()
    {
        if (SecurityProxyClient.token != null)
            return SecurityProxyClient.token;

        DefaultAzureCredential credential = new DefaultAzureCredential(includeInteractiveCredentials: true);
        //var authContext = new Azure.Core.TokenRequestContext(new[] { $"https://management.azure.com/.default" });
        Azure.Core.TokenRequestContext authContext = new Azure.Core.TokenRequestContext(new[] { "1b51071d-9874-44d1-b22b-7ed45fc85cbf" });
        Azure.Core.AccessToken token = credential.GetToken(authContext);

        return (SecurityProxyClient.token = token.Token);
    }

    public static HttpClient GetHttpClient()
    {
        if (SecurityProxyClient.httpClient == null)
        {
            SecurityProxyClient.httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetToken());
        }

        return SecurityProxyClient.httpClient;
    }

    public static async Task<string> APIGet(string url)
    {
        var resp = await SecurityProxyClient.GetHttpClient().GetAsync(url);

        if (resp.IsSuccessStatusCode)
        {
            return await resp.Content.ReadAsStringAsync();
        }

        string body = await resp.Content.ReadAsStringAsync();
        string msg = $"SecurityProxy call failled: {resp.StatusCode}: {body}";
        throw new Exception(msg);
    }

    public static async Task<string> APIPost(string url, string data)
    {
        var post = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
        var resp = await SecurityProxyClient.GetHttpClient().PostAsync(url, post);

        if (resp.IsSuccessStatusCode)
        {
            return await resp.Content.ReadAsStringAsync();
        }

        string body = await resp.Content.ReadAsStringAsync();
        string msg = $"SecurityProxy call failled: {resp.StatusCode}: {body}";
        throw new Exception(msg);
    }
    public static async Task<string> APIPost(string url, Object json)
    {
        String data = JsonContent.Create(json).ReadAsStringAsync().Result;
        return await APIPost(url, data);
    }
}


public class SecApiProvider : Provider
{
    readonly IHost host;
    int id = 0;

    public SecApiProvider(IHost host)
    {
        this.host = host;
    }

    public override Task<GetSchemaResponse> GetSchema(GetSchemaRequest request, CancellationToken ct)
    {
        return base.GetSchema(request, ct);
    }

    public override Task<CheckResponse> CheckConfig(CheckRequest request, CancellationToken ct)
    {
        return base.CheckConfig(request, ct);
    }

    public override Task<DiffResponse> DiffConfig(DiffRequest request, CancellationToken ct)
    {
        return Task.FromResult(new DiffResponse());
    }

    public override Task<ConfigureResponse> Configure(ConfigureRequest request, CancellationToken ct)
    {
        return Task.FromResult(new ConfigureResponse());
    }

    public override Task<CheckResponse> Check(CheckRequest request, CancellationToken ct)
    {
        if (request.Type == "secapiprovider:index:SecAPI") 
        {
            return Task.FromResult(new CheckResponse() { Inputs = request.News });
        }

        throw new Exception($"Unknown resource type '{request.Type}'");
    }

    public override Task<DiffResponse> Diff(DiffRequest request, CancellationToken ct)
    {
        if (request.Type == "secapiprovider:index:SecAPI")
        {
            var changes = !request.Olds["group"].Equals(request.News["groups"]);
            return Task.FromResult(new DiffResponse()
            {
                Changes = changes,
                Replaces = new string[] { "groups" },
            });
        }
        throw new Exception($"Unknown resource type '{request.Type}'");
    }

    public override Task<CreateResponse> Create(CreateRequest request, CancellationToken ct)
    {
        if (request.Type == "secapiprovider:index:groups")
        {
            //var org = request.Properties["org"]; 
            //var env = request.Properties["env"];
            var groupsProp = request.Properties["groups"];

            //var orgValue = org.TryGetString(orgValue);
            //var envValue = env.TryGetString(envValue);

            var groups = SecurityProxyClient.APIGet("https://operator.snqa.lan/api/group/org/create/snqd/pooq").GetAwaiter().GetResult();

            var outputs = new Dictionary<string, PropertyValue>();
            outputs.Add(groups, groupsProp);
            return Task.FromResult(new CreateResponse()
            {
                ID = Guid.NewGuid().ToString(),
                Properties = outputs,
            });
        }
        throw new Exception($"Unknown resource type '{request.Type}'");
    }

    public override Task Delete(DeleteRequest request, CancellationToken ct)
    {
        return Task.CompletedTask;
    }

    public override Task<ReadResponse> Read(ReadRequest request, CancellationToken ct)
    {
        var response = new ReadResponse()
        {
            ID = request.ID,
            Properties = request.Properties,
        };
        return Task.FromResult(response);
    }
}

