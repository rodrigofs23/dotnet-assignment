using Work.Database;
using Work.Implementation;
using Work.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Register services for DI
builder.Services.AddScoped<IRepository<User, Guid>>(provider =>
{
    var mockDatabase = new MockDatabase(seed: 10);
    return new UserRepository(mockDatabase);
});

//builder.Services.AddSingleton<IRepository<User, Guid>, UserRepository>();

//builder.Services.AddScoped<UserRepository, IRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
