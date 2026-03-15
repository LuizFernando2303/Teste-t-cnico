using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5000");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=gastos.db"));

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowReact", policy =>
  {
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
  });
});

var app = builder.Build();

app.UseCors("AllowReact");

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();

  var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp");

  if (Directory.Exists(frontendPath))
  {
    var startFront = new ProcessStartInfo
    {
      FileName = "bash",
      Arguments = "-c \"npm run dev\"",
      WorkingDirectory = frontendPath,
      UseShellExecute = true
    };

    Process.Start(startFront);
  }

  await Task.Run(async () =>
  {
    await Task.Delay(3000);

    Process.Start(new ProcessStartInfo
    {
      FileName = "xdg-open",
      Arguments = "http://localhost:5001",
      UseShellExecute = false
    });

    Process.Start(new ProcessStartInfo
    {
      FileName = "xdg-open",
      Arguments = "http://localhost:5000/swagger",
      UseShellExecute = false
    });
  });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
