using AppThatWillNeedToBeScaledHorizontally;
using Orleans.Clustering.CosmosDB;
using Orleans.Hosting;
using Orleans.Persistence.CosmosDB;

var builder = WebApplication.CreateBuilder(args);

// add services to the container
builder.Services.AddRazorPages();

// set the app up to be distributed by default
builder.Services.AddDefaultDistributedProviders((options, siloBuilder) =>
{
    // opt in/out of services
    options.DistributedCache = true;
    options.FeatureManagement = false;
    options.Identity = true;
    options.HubLifetimeManager = true;
    options.OutputCaching = false;

    // configure the silo to use Cosmos DB storage
    siloBuilder
        .AddCosmosDBGrainStorageAsDefault((CosmosDBStorageOptions cosmosOptions) =>
        {
            cosmosOptions.AccountEndpoint = builder.Configuration.GetValue<string>("AccountEndpoint");
            cosmosOptions.AccountKey = builder.Configuration.GetValue<string>("AccountKey");
            cosmosOptions.DB = builder.Configuration.GetValue<string>("DB");
            cosmosOptions.CanCreateResources = true;
        });
});

// build the service container
var app = builder.Build();

// configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
