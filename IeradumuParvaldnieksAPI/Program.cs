using IeradumuParvaldnieksApplication;
using IeradumuParvaldnieksAPI;

var builder = WebApplication.CreateBuilder(args);

// Pievieno kontrolierus.
builder.Services.AddControllers();

// Pievieno OpenAPI.
builder.Services.AddOpenApi();

// Viena kopīga atmiņas glabātuve visā API darbības laikā.
builder.Services.AddSingleton<
    IIeradumuRepository,
    AtminasIeradumuRepository
>();

// Aplikācijas slāņa serviss.
builder.Services.AddScoped<IeradumuService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();