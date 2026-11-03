using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// 1. Get the raw connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Parse and fix the string immediately
if (connectionString != null && connectionString.StartsWith("postgres://"))
{
    var databaseUri = new Uri(connectionString);
    var userInfo = databaseUri.UserInfo.Split(':');
    var username = userInfo[0];
    var password = userInfo.Length > 1 ? userInfo[1] : "";

    connectionString = $"Host={databaseUri.Host};" +
                       $"Port={databaseUri.Port};" +
                       $"Database={databaseUri.AbsolutePath.TrimStart('/')};" +
                       $"Username={username};" +
                       $"Password={password};" +
                       $"SSL Mode=Require;" +
                       $"Trust Server Certificate=true;";
                       
    // 3. OVERWRITE IT in the configuration provider so Npgsql cannot sneakily read the old one!
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

// 4. Pass the string to DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

// --- API ENDPOINTS ---

// READ all todos
app.MapGet("/api/todos", async (AppDbContext db) => 
    await db.Todos.ToListAsync());

// WRITE (Create) a new todo
app.MapPost("/api/todos", async (Todo todo, AppDbContext db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/api/todos/{todo.Id}", todo);
});

// Run migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
