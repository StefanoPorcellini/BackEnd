using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.DAO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLogging();
builder.Services.AddScoped<IPrenotazioneDAO, PrenotazioneDAO>();
builder.Services.AddScoped<IClienteDAO, ClienteDAO>();
builder.Services.AddScoped<IDettaglioSoggiornoDAO, DettagliSoggiornoDAO>();
builder.Services.AddScoped<IServizioAggiuntivoDAO, ServizioAggiuntivoDAO>();
builder.Services.AddScoped<ITipologiaCameraDAO, TipologiaCameraDAO>();
builder.Services.AddScoped<ICameraDAO, CameraDAO>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
