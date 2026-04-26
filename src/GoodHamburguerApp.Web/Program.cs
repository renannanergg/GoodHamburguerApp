using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GoodHamburguerApp.Web;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

// 2. REGISTRE O HANDLER AQUI (No Web)
builder.Services.AddTransient<JwtHandler>();

// 3. CONFIGURE O HTTPCLIENT AQUI (No Web)
builder.Services.AddHttpClient("GoodHamburguerAPI", client =>
    client.BaseAddress = new Uri("https://localhost:7178/")) // Porta da sua API
    .AddHttpMessageHandler<JwtHandler>();

// 4. REGISTRE O CLIENTE PADRÃO (No Web)
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("GoodHamburguerAPI"));
await builder.Build().RunAsync();
