/*
 * Program.cs
 * Arquivo principal de inicialização da aplicação
 */

// Namespaces necessários para o EF Core e MySQL
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
// Assumindo que você criou a pasta 'Data' como no Passo 3
using DESAFIOCRUD.Data; 

var builder = WebApplication.CreateBuilder(args);

// --- INÍCIO DA ADIÇÃO (Passo 5) ---

// 1. Pegar a string de conexão (do appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Adicionar o DbContext (o "serviço" do banco de dados)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions =>
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: System.TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)
    )
);

// --- FIM DA ADIÇÃO ---


// Add services to the container. (Isso já estava no seu arquivo)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Necessário para CSS, JS e Imagens
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

// GARANTA QUE ESTA É A ÚLTIMA LINHA. SEM CHAVES '}' A MAIS DEPOIS DAQUI.