using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    // options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings.Remove(".data");
provider.Mappings[".data"] = "application/octet-stream";
provider.Mappings.Remove(".wasm");
provider.Mappings[".wasm"] = "application/wasm";
provider.Mappings.Remove(".symbols.json");
provider.Mappings[".symbols.json"] = "application/octet-stream";
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// app.UseAuthorization();

app.MapRazorPages();

app.Run();