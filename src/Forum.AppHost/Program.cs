var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Forum_ApiService>("apiservice");

builder.AddProject<Projects.Forum_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
