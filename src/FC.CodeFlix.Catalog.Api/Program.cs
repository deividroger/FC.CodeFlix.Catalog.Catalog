using FC.CodeFlix.Catalog.Api.CastMembers;
using FC.CodeFlix.Catalog.Api.Categories;
using FC.CodeFlix.Catalog.Api.Filters;
using FC.CodeFlix.Catalog.Api.Genres;
using FC.CodeFlix.Catalog.Api.Videos;
using FC.CodeFlix.Catalog.Application;
using FC.CodeFlix.Catalog.Infra.ES;
using FC.CodeFlix.Catalog.Infra.HttpClients;
using FC.CodeFlix.Catalog.Infra.Messaging;
using Keycloak.AuthServices.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddKeycloakAuthentication(builder.Configuration);

builder.Services
                .AddAuthorization() 
                .AddUseCases()
                .AddMemoryCache()
                .AddHttpClients(builder.Configuration)
                .AddConsumers(builder.Configuration)
                .AddElasticSearch(builder.Configuration)
                .AddRepositories();

builder.Services.AddGraphQLServer()
                .AddAuthorization() //HOT CHOCOLATE
                .AddQueryType()
                .AddMutationType()
                .AddTypeExtension<CategoryQueries>()
                .AddTypeExtension<GenreQueries>()
                .AddTypeExtension<CastMemberQueries>()
                .AddTypeExtension<VideoQueries>()
                .AddTypeExtension<CategoriesMutations>()
                .AddErrorFilter<GraphQLErrorFilter>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
   //.RequireAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }