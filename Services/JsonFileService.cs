using System.IO;
using System.Text.Json;
using ProductCatalogApp.Models;

namespace ProductCatalogApp.Services;

/// <summary>
/// Выполняет сохранение и загрузку товаров в JSON-файл.
/// </summary>
public class JsonFileService
{
    /// <summary>
    /// Имя файла данных приложения.
    /// </summary>
    private const string FileName = "products.json";

    /// <summary>
    /// Загружает список товаров из файла.
    /// </summary>
    /// <returns>Список товаров или пустой список, если файл отсутствует.</returns>
    public List<Product> Load()
    {
        if (!File.Exists(FileName))
        {
            return new List<Product>();
        }

        string json = File.ReadAllText(FileName);

        List<Product>? products = JsonSerializer.Deserialize<List<Product>>(json);

        return products ?? new List<Product>();
    }

    /// <summary>
    /// Сохраняет список товаров в файл.
    /// </summary>
    /// <param name="products">Коллекция товаров для сохранения.</param>
    public void Save(IEnumerable<Product> products)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(products, options);
        File.WriteAllText(FileName, json);
    }
}