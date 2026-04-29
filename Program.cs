
using Frontend_AprendeYa.Services;


var builder = WebApplication.CreateBuilder(args);

// 1. Soporte para Vistas MVC
builder.Services.AddControllersWithViews();

// 2. Soporte para Sesiones (Para guardar el Token temporalmente)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // La sesión dura 2 horas
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. Configurar la conexión a tu API_AprendeYa
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    // Asegúrate de que esta URL sea la de tu Swagger
    client.BaseAddress = new Uri("https://localhost:7061/");
});
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/"); // Tu puerto del backend
});

//Soporte para acceder a la sesión desde los Services
builder.Services.AddHttpContextAccessor();

//Registrar el CursoService con la URL de tu API
builder.Services.AddHttpClient<CursoService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});
// NUESTRO NUEVO SERVICIO PARA EL CRUD DE USUARIOS
builder.Services.AddHttpClient<IUsuarioService, UsuarioService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/"); // ¡Debe ser el mismo puerto!
});
builder.Services.AddHttpClient<ICarritoService, CarritoService>(client =>
{
    // Asegúrate de que apunte al mismo puerto de tu API Backend
    client.BaseAddress = new Uri("https://localhost:7061/");
});
// Agrégalo junto a tus otros Scoped
builder.Services.AddHttpClient<IVentaService, VentaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/"); // <-- Verifica tu puerto del Backend
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Activar las sesiones ANTES de la autorización
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"); // <-- Hacemos que el Login sea la primera pantalla en abrir

app.Run();

