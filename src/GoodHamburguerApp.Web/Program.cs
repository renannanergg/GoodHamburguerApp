using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GoodHamburguerApp.Web;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTransient<JwtHandler>();

builder.Services.AddHttpClient("GoodHamburguerAPI", client =>
    client.BaseAddress = new Uri("https://localhost:7178/")) 
    .AddHttpMessageHandler<JwtHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("GoodHamburguerAPI"));
await builder.Build().RunAsync();
