namespace ProductCatalogApp.Models;

/// <summary>
/// Представляет товар в информационной системе.
/// </summary>
public class Product
{
    /// <summary>
    /// Получает или задаёт название товара.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задаёт производителя товара.
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задаёт категорию товара.
    /// </summary>
    public ProductCategory Category { get; set; }

    /// <summary>
    /// Получает или задаёт количество товара на складе.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Получает название товара для отображения в списке.
    /// </summary>
    public string DisplayName => Name;
}