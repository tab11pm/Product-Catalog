using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProductCatalogApp.Models;

/// <summary>
/// Представляет товар в информационной системе.
/// </summary>
public class Product : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _manufacturer = string.Empty;
    private ProductCategory _category;
    private int _quantity;

    /// <summary>
    /// Возникает при изменении свойства.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Получает или задаёт название товара.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value)
            {
                return;
            }

            _name = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DisplayName));
        }
    }

    /// <summary>
    /// Получает или задаёт производителя товара.
    /// </summary>
    public string Manufacturer
    {
        get => _manufacturer;
        set
        {
            if (_manufacturer == value)
            {
                return;
            }

            _manufacturer = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Получает или задаёт категорию товара.
    /// </summary>
    public ProductCategory Category
    {
        get => _category;
        set
        {
            if (_category == value)
            {
                return;
            }

            _category = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Получает или задаёт количество товара на складе.
    /// </summary>
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity == value)
            {
                return;
            }

            _quantity = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Получает название товара для отображения в списке.
    /// </summary>
    public string DisplayName => Name;

    /// <summary>
    /// Вызывает событие изменения свойства.
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}