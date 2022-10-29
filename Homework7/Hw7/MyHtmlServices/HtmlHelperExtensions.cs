using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var properties = helper.ViewData.ModelMetadata.ModelType.GetProperties();
        return model is null ? 
            EditorWithoutModel(properties) : 
            EditorWithModel(properties, model);
    }
    
    private static IHtmlContent EditorWithModel(IEnumerable<PropertyInfo> properties, object model)
    {
        var builder = new HtmlContentBuilder();

        foreach (var property in properties)
        {
            var validAttributes = property.GetCustomAttributes<ValidationAttribute>();
            var valueFromModel = property.GetValue(model);

            builder.AppendHtmlLine($"<div>{FormProperty(property)}");

            foreach (var validAttribute in validAttributes)
            {
                if (!validAttribute.IsValid(valueFromModel))
                {
                    builder.AppendHtmlLine(
                        $"{GetLabel($"{property.Name}", string.Empty)}<span>{validAttribute.ErrorMessage}</span>");
                }
            }

            builder.AppendHtmlLine("</div>");
        }

        return builder;
    }
    
    private static IHtmlContent EditorWithoutModel(IEnumerable<PropertyInfo> properties)
    {
        var builder = new HtmlContentBuilder();
        
        foreach (var property in properties)
        {
            builder.AppendHtmlLine($"<div>{FormProperty(property)}</div>");
        }
        
        return builder;
    }
    
    private static string FormProperty(PropertyInfo property)
    {
        var name = property.Name;
        
        var labelContent = GetDisplayedName(property.Name, 
            property.GetCustomAttribute<DisplayAttribute>());

        if (property.PropertyType.IsEnum)
            return $"{GetLabel($"{name}", labelContent)}<br>{GetSelect(property.PropertyType.GetEnumNames())}<br>";
        
        var type = property.PropertyType == typeof(string) ? "text" : "number";

        return $"{GetLabel($"{name}", labelContent)}<br> <input id=\"{name}\" name=\"{name}\" type=\"{type}\">";
    }
    
    private static string GetSelect(IEnumerable<string> data) =>
        $"<select>{string.Join("", GetOptions(data))}</select>";
    
    private static string GetLabel(string name, string content) =>
        $"<label for=\"{name}\">{content}</label>";

    private static IEnumerable<string> GetOptions(IEnumerable<string> data) =>
        data.Select(optionValue => 
            $"<option value=\"{optionValue}\">{optionValue}</option>");
    
    private static string GetDisplayedName(string property, DisplayAttribute? attribute) =>
        attribute is null ? SplitCamelCase(property) : attribute.Name!;
    
    private static string SplitCamelCase(string input) => 
        Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
} 