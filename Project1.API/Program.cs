using Project1.App;
using Project1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// initialize the connection string value
string connectionString = File.ReadAllText(@"/Revature/221024/Project1/ConnectionStrings/Project1ConnectionString.txt");
//IRepository repo = new SqlRepository(connectionString);
// add the repository to the build as a service
builder.Services.AddTransient<SqlRepository>(); // SqlRepository or IRepository?


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/user", (string username, SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    var user = repo.GetUser(username);
    return user;
});

app.MapGet("/open-tickets", (SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    var tickets = repo.GetOpenTickets();
    return tickets;
});

app.MapGet("/ticket-history", (string username, SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    var tickets = repo.GetPreviousTickets(username);
    return tickets;
});



app.Run();