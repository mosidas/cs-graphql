using GraphQL;
using StarWars;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var s = builder.Services;

s.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
s.AddEndpointsApiExplorer();
s.AddSwaggerGen();
s.AddGraphQL(x => x
    .AddSystemTextJson()
    .AddSchema<StarWarsSchema>()
    .AddGraphTypes(typeof(StarWarsSchema).Assembly)
    .AddErrorInfoProvider(opt => opt.ExposeExceptionDetails = true)
);
//s.AddHostedService<TestMessageService>();
s.AddSingleton<StarWarsData>();
s.AddSingleton<IChat, Chat>();
s.AddLogging(builder => builder.AddConsole());
s.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseWebSockets();

app.UseGraphQL<StarWarsSchema>();
app.UseGraphQLPlayground(); // http://{hostnaname}/ui/playground
app.UseGraphQLGraphiQL(); // http://{hostnaname}/ui/graphiql

app.Run();