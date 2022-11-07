using System.Diagnostics.CodeAnalysis;
using Hw9.Configuration;
using Hw9.Parser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddMathCalculator();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculator}/{action=Calculator}/{id?}");

app.Run();

namespace Hw9
{
    [ExcludeFromCodeCoverage]
    public partial class Program
    {
        public static void Main()
        {
            // var tokeniser = Tokenizer.ParseToTokens("-8 * (-2 + 2) - 3 * 4");
            // Console.WriteLine(tokeniser);
            // Console.WriteLine(MathTokenType.Minus == MathTokenType.OpenBracket);
            // Console.ReadLine();
        }
    }
}