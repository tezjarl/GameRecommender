var builder = DistributedApplication.CreateBuilder(args);

var postgresUser = builder.AddParameter("postgresUser");
var postgresPassword = builder.AddParameter("postgresPassword", secret: true);
builder.AddPostgres("postgres", postgresUser, postgresPassword);

var rabbitUser = builder.AddParameter("rabbitUser");
var rabbitPassword = builder.AddParameter("rabbitPassword", secret: true);
builder.AddRabbitMQ("rabbitmq", rabbitUser, rabbitPassword);

var apiService = builder.AddProject<Projects.GameRecommender_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.GameRecommender_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
