using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ProductCatalogApp.Models;
using System.Windows.Controls;
using System.Windows.Media;
using ProductCatalogApp.Services;
using System.ComponentModel;
using System.Windows.Data;
namespace ProductCatalogApp;

/// <summary>
/// Главное окно приложения.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Переводит форму в режим создания нового товара.
    /// </summary>
    private void NewButton_Click(object sender, RoutedEventArgs e)
    {
        ProductsListBox.SelectedItem = null;
        ClearForm();
    }
    private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
{
    if (_isUpdatingForm || ProductsListBox.SelectedItem is not Product selectedProduct)
    {
        return;
    }

    selectedProduct.Name = NameTextBox.Text;
}

private void ManufacturerTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
{
    if (_isUpdatingForm || ProductsListBox.SelectedItem is not Product selectedProduct)
    {
        return;
    }

    selectedProduct.Manufacturer = ManufacturerTextBox.Text;
}

private void CategoryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
{
    if (_isUpdatingForm || ProductsListBox.SelectedItem is not Product selectedProduct || CategoryComboBox.SelectedItem is null)
    {
        return;
    }

    selectedProduct.Category = (ProductCategory)CategoryComboBox.SelectedItem;
}

private void QuantityTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
{
    if (_isUpdatingForm || ProductsListBox.SelectedItem is not Product selectedProduct)
    {
        return;
    }

    if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity >= 0)
    {
        selectedProduct.Quantity = quantity;
    }
}
    /// <summary>
/// Флаг внутреннего обновления формы.
/// </summary>
private bool _isUpdatingForm;

    /// <summary>
/// Обрабатывает изменение свойств товара.
/// </summary>
private void Product_PropertyChanged(object? sender, PropertyChangedEventArgs e)
{
    _productsView?.Refresh();
}

    /// <summary>
/// Отписывает обработчик от изменения свойств товара.
/// </summary>
/// <param name="product">Товар.</param>
private void UnsubscribeFromProduct(Product product)
{
    product.PropertyChanged -= Product_PropertyChanged;
}

    /// <summary>
/// Подписывает обработчик на изменение свойств товара.
/// </summary>
/// <param name="product">Товар.</param>
private void SubscribeToProduct(Product product)
{
    product.PropertyChanged += Product_PropertyChanged;
}

    /// <summary>
/// Представление коллекции товаров для сортировки и отображения.
/// </summary>
private ICollectionView? _productsView;

    /// <summary>
/// Сервис для работы с файлом данных.
/// </summary>
private readonly JsonFileService _jsonFileService = new();

    /// <summary>
/// Сбрасывает оформление поля ввода.
/// </summary>
private void ResetValidation(Control control)
{
    control.ClearValue(BackgroundProperty);
    control.ClearValue(BorderBrushProperty);
    control.ToolTip = null;
}

/// <summary>
/// Отмечает поле как ошибочное.
/// </summary>
private void MarkInvalid(Control control, string message)
{
    control.Background = Brushes.MistyRose;
    control.BorderBrush = Brushes.Red;
    control.ToolTip = message;
}

    /// <summary>
    /// Коллекция товаров.
    /// </summary>
    private ObservableCollection<Product> _products = new();

    /// <summary>
    /// Инициализирует экземпляр окна.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        InitializeCategories();
        LoadProducts();
        RefreshProductsList();
    }

    /// <summary>
    /// Заполняет список категорий.
    /// </summary>
    private void InitializeCategories()
    {
        CategoryComboBox.ItemsSource = Enum.GetValues(typeof(ProductCategory));
        CategoryComboBox.SelectedIndex = 0;
    }

    /// <summary>
    /// Обновляет список товаров с сортировкой по алфавиту.
    /// </summary>
    /// <summary>
/// Обновляет представление списка товаров с сортировкой по алфавиту.
/// </summary>
    private void RefreshProductsList()
    {
        _productsView = CollectionViewSource.GetDefaultView(_products);

        _productsView.SortDescriptions.Clear();
        _productsView.SortDescriptions.Add(new SortDescription(nameof(Product.Name), ListSortDirection.Ascending));

        ProductsListBox.ItemsSource = _productsView;
    }

    /// <summary>
    /// Обрабатывает выбор товара в списке.
    /// </summary>
    private void ProductsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (ProductsListBox.SelectedItem is not Product selectedProduct)
        {
            ClearForm();
            return;
        }

        _isUpdatingForm = true;

        NameTextBox.Text = selectedProduct.Name;
        ManufacturerTextBox.Text = selectedProduct.Manufacturer;
        CategoryComboBox.SelectedItem = selectedProduct.Category;
        QuantityTextBox.Text = selectedProduct.Quantity.ToString();

        _isUpdatingForm = false;
    }

    /// <summary>
    /// Загружает товары из файла.
    /// </summary>
    private void LoadProducts()
    {
        List<Product> loadedProducts = _jsonFileService.Load();
        _products = new ObservableCollection<Product>(loadedProducts);

        foreach (Product product in _products)
        {
            SubscribeToProduct(product);
        }
    }

    /// <summary>
    /// Сохраняет товары в файл перед закрытием окна.
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
        _jsonFileService.Save(_products);
        base.OnClosed(e);
    }

    /// <summary>
    /// Добавляет новый товар.
    /// </summary>
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductsListBox.SelectedItem is not null)
        {
            MessageBox.Show(
                "Чтобы добавить новый товар, сначала нажмите кнопку \"Новый\".",
                "Информация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            return;
        }

        if (!ValidateInput(out int quantity))
        {
            return;
        }

        Product product = new()
        {
            Name = NameTextBox.Text.Trim(),
            Manufacturer = ManufacturerTextBox.Text.Trim(),
            Category = (ProductCategory)CategoryComboBox.SelectedItem!,
            Quantity = quantity
        };

        _products.Add(product);
        SubscribeToProduct(product);
        RefreshProductsList();
        ClearForm();
    }

    /// <summary>
    /// Сохраняет изменения выбранного товара.
    /// </summary>
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductsListBox.SelectedItem is not Product selectedProduct)
        {
            MessageBox.Show("Сначала выберите товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ValidateInput(out int quantity))
        {
            return;
        }

        selectedProduct.Name = NameTextBox.Text.Trim();
        selectedProduct.Manufacturer = ManufacturerTextBox.Text.Trim();
        selectedProduct.Category = (ProductCategory)CategoryComboBox.SelectedItem!;
        selectedProduct.Quantity = quantity;

        RefreshProductsList();
        ProductsListBox.SelectedItem = _products.FirstOrDefault(product =>
            product.Name == selectedProduct.Name &&
            product.Manufacturer == selectedProduct.Manufacturer &&
            product.Quantity == selectedProduct.Quantity &&
            product.Category == selectedProduct.Category);
    }

    /// <summary>
    /// Удаляет выбранный товар.
    /// </summary>
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductsListBox.SelectedItem is not Product selectedProduct)
        {
            MessageBox.Show("Сначала выберите товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _products.Remove(selectedProduct);
        UnsubscribeFromProduct(selectedProduct);
        RefreshProductsList();
        ClearForm();
    }

    /// <summary>
    /// Проверяет корректность введённых данных.
    /// </summary>
    private bool ValidateInput(out int quantity)
    {
        quantity = 0;

        ResetValidation(NameTextBox);
        ResetValidation(ManufacturerTextBox);
        ResetValidation(CategoryComboBox);
        ResetValidation(QuantityTextBox);

        bool isValid = true;

        string name = NameTextBox.Text.Trim();
        string manufacturer = ManufacturerTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            MarkInvalid(NameTextBox, "Название товара не должно быть пустым.");
            isValid = false;
        }
        else if (name.Length > 100)
        {
            MarkInvalid(NameTextBox, "Название товара не должно превышать 100 символов.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(manufacturer))
        {
            MarkInvalid(ManufacturerTextBox, "Производитель не должен быть пустым.");
            isValid = false;
        }
        else if (manufacturer.Length > 100)
        {
            MarkInvalid(ManufacturerTextBox, "Производитель не должен превышать 100 символов.");
            isValid = false;
        }

        if (CategoryComboBox.SelectedItem is null)
        {
            MarkInvalid(CategoryComboBox, "Выберите категорию товара.");
            isValid = false;
        }

        if (!int.TryParse(QuantityTextBox.Text.Trim(), out quantity))
        {
            MarkInvalid(QuantityTextBox, "Количество должно быть целым числом.");
            isValid = false;
        }
        else if (quantity < 0)
        {
            MarkInvalid(QuantityTextBox, "Количество товара не может быть отрицательным.");
            isValid = false;
        }

        return isValid;
    }

    /// <summary>
    /// Очищает форму редактирования.
    /// </summary>
    private void ClearForm()
    {
        _isUpdatingForm = true;

        NameTextBox.Clear();
        ManufacturerTextBox.Clear();
        QuantityTextBox.Clear();
        CategoryComboBox.SelectedIndex = 0;

        ResetValidation(NameTextBox);
        ResetValidation(ManufacturerTextBox);
        ResetValidation(CategoryComboBox);
        ResetValidation(QuantityTextBox);

        _isUpdatingForm = false;
    }
}