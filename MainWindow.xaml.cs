// using System.Text;
// using System.Windows;
// using System.Windows.Controls;
// using System.Windows.Data;
// using System.Windows.Documents;
// using System.Windows.Input;
// using System.Windows.Media;
// using System.Windows.Media.Imaging;
// using System.Windows.Navigation;
// using System.Windows.Shapes;

// namespace ProductCatalogApp;

// /// <summary>
// /// Interaction logic for MainWindow.xaml
// /// </summary>
// public partial class MainWindow : Window
// {
//     public MainWindow()
//     {
//         InitializeComponent();
//     }
// }


using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ProductCatalogApp.Models;

namespace ProductCatalogApp;

/// <summary>
/// Главное окно приложения.
/// </summary>
public partial class MainWindow : Window
{
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
    private void RefreshProductsList()
    {
        ProductsListBox.ItemsSource = _products
            .OrderBy(product => product.Name)
            .ToList();
    }

    /// <summary>
    /// Обрабатывает выбор товара в списке.
    /// </summary>
    private void ProductsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (ProductsListBox.SelectedItem is not Product selectedProduct)
        {
            return;
        }

        NameTextBox.Text = selectedProduct.Name;
        ManufacturerTextBox.Text = selectedProduct.Manufacturer;
        CategoryComboBox.SelectedItem = selectedProduct.Category;
        QuantityTextBox.Text = selectedProduct.Quantity.ToString();
    }

    /// <summary>
    /// Добавляет новый товар.
    /// </summary>
    private void AddButton_Click(object sender, RoutedEventArgs e)
{
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
    RefreshProductsList();
    ProductsListBox.SelectedItem = product;
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
        RefreshProductsList();
        ClearForm();
    }

    /// <summary>
    /// Проверяет корректность введённых данных.
    /// </summary>
    private bool ValidateInput(out int quantity)
    {
        quantity = 0;

        string name = NameTextBox.Text.Trim();
        string manufacturer = ManufacturerTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Название товара не должно быть пустым.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (name.Length > 100)
        {
            MessageBox.Show("Название товара не должно превышать 100 символов.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (string.IsNullOrWhiteSpace(manufacturer))
        {
            MessageBox.Show("Производитель не должен быть пустым.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (manufacturer.Length > 100)
        {
            MessageBox.Show("Производитель не должен превышать 100 символов.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!int.TryParse(QuantityTextBox.Text.Trim(), out quantity) || quantity < 0)
        {
            MessageBox.Show("Количество товара должно быть неотрицательным целым числом.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Очищает форму редактирования.
    /// </summary>
    private void ClearForm()
    {
        NameTextBox.Clear();
        ManufacturerTextBox.Clear();
        QuantityTextBox.Clear();
        CategoryComboBox.SelectedIndex = 0;
    }
}