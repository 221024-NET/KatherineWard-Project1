using Project1.App;
using Project1.Data;
using Project1.Logic;

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

app.MapPost("/login", (User user, SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    if(repo.CheckLogin(user.Username, user.Password))
    {
        var loginUser = repo.GetUser(user.Username);
        return loginUser;
    }
    else
    {
        return null;
    }
});

app.MapPost("/registeremployee", (string username, string password, string name, SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    if (repo.EmployeeRegister(username, password, name))
    {
        var user = repo.GetUser(username);
        return Results.Created($"/user/{user.EmployeeId}", user);
    }
    else
    {
        return null;
    }
});

app.MapPost("/registermanager", (string username, string password, string name, SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    if (repo.ManagerRegister(username, password, name))
    {
        var user = repo.GetUser(username);
        return Results.Created($"/user/{user.EmployeeId}", user);
    }
    else
    {
        return null;
    }
});

app.MapGet("/tickets", (SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    var tickets = repo.GetOpenTickets();
    return tickets;
});

app.MapGet("/tickets/{id}", (int id, SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    var tickets = repo.GetPreviousTickets(id);
    return tickets;
});

app.MapPost("/tickets/{employeeId}", (int employeeId, Ticket newTicket, SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    var ticket = repo.NewTicket(newTicket, employeeId);
    return ticket;
});

app.MapPut("/tickets/{id}", (int id, Ticket updatedTicket, SqlRepository repo) =>
{
    repo.connectionString = connectionString;
    var ticket = repo.ManageTicket(id, (updatedTicket.Status == "Approved" ? 1 : 2));
    return ticket;
});


app.Run();