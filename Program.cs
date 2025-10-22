/*
 * Program.cs
 * Arquivo principal de inicialização da aplicação
 */

// Namespaces necessários para o EF Core e MySQL
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
// Assumindo que você criou a pasta 'Data' como no Passo 3
using DESAFIOCRUD.Data; 

[cite_start]var builder = WebApplication.CreateBuilder(args); [cite: 1]

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
[cite_start]builder.Services.AddControllersWithViews(); [cite: 1]

[cite_start]var app = builder.Build(); [cite: 1]

// Configure the HTTP request pipeline.
[cite_start]if (!app.Environment.IsDevelopment()) [cite: 1]
{
    [cite_start]app.UseExceptionHandler("/Home/Error"); [cite: 1]
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    [cite_start]app.UseHsts(); [cite: 1]
}

[cite_start]app.UseHttpsRedirection(); [cite: 1]
[cite_start]app.UseRouting(); [cite: 1]

[cite_start]app.UseAuthorization(); [cite: 1]

[cite_start]app.MapStaticAssets(); [cite: 1]

[cite_start]app.MapControllerRoute( [cite: 1]
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    [cite_start].WithStaticAssets(); [cite: 1]


[cite_start]app.Run(); [cite: 1]

// (Note que eu removi a chave "}" extra que tinha no final do seu arquivo original)