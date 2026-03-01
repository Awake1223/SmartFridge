using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uno.Toolkit.UI;
using Uno.UI.Extensions;
using Windows.UI;

namespace UnoAppMobile.Presentation;

// Модель продукта
public class ProductItem
{
    public string Name { get; set; }
    public string Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string ImageUrl { get; set; }
    public int FreshnessLevel { get; set; } // 1-7, где 1 - свежий, 7 - испорчен
}

public sealed partial class MyFridgePage : Page
{
    private ObservableCollection<ProductItem> _products = new ObservableCollection<ProductItem>();

    public MyFridgePage()
    {
        BuildUI();
        LoadSampleData();
    }

    private void BuildUI()
    {
        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // заголовок
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // поиск
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // список продуктов
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // таббар

        // --- Заголовок ---
        var headerBorder = new Border
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 143, 170, 232)),
            CornerRadius = new CornerRadius(0, 0, 20, 20),
            Padding = new Thickness(20),
            Margin = new Thickness(0, 0, 0, 20),
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var headerText = new TextBlock
        {
            Text = "Мой холодильник",
            FontSize = 24,
            FontWeight = FontWeights.Normal,
            FontFamily = new FontFamily("Assets/Fonts/SFProDisplayMedium.otf#SF Pro Display"),
            Foreground = new SolidColorBrush(Colors.White),
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        headerBorder.Child = headerText;
        Grid.SetRow(headerBorder, 0);
        grid.Children.Add(headerBorder);

        // --- Поиск и фильтры ---
        var searchPanel = new StackPanel
        {
            Margin = new Thickness(16, 0, 16, 16),
            Spacing = 10,
        };

        var searchBox = new TextBox
        {
            PlaceholderText = "Поиск продуктов...",
            FontSize = 20,
            Height = 48,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
        };
        searchBox.TextChanged += OnSearchTextChanged;

        var filterPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        filterPanel.Children.Add(CreateFilterChip("Всё"));
        filterPanel.Children.Add(CreateFilterChip("Скоропортящиеся"));
        filterPanel.Children.Add(CreateFilterChip("Замороженные"));

        searchPanel.Children.Add(searchBox);
        searchPanel.Children.Add(filterPanel);

        Grid.SetRow(searchPanel, 1);
        grid.Children.Add(searchPanel);

        // --- Список продуктов с использованием нашего контрола ---
        var productsListView = new ListView
        {
            ItemsSource = _products,
            Margin = new Thickness(16, 0, 16, 0),
            ItemTemplate = CreateProductTemplate()
        };

        Grid.SetRow(productsListView, 2);
        grid.Children.Add(productsListView);

        // --- Нижняя панель ---
        var tabBar = CreateTabBar();
        Grid.SetRow(tabBar, 3);
        grid.Children.Add(tabBar);


        // --- Фон страницы ---
        this.Background(new SolidColorBrush(Color.FromArgb(255, 248, 248, 248)))
            .Content(grid);
    }

    // Простой DataTemplate, который использует наш UserControl
    private DataTemplate CreateProductTemplate()
    {
        // Создаём DataTemplate, который будет создавать ProductItemControl
        Func<ProductItem, UIElement> factory = (product) => new ProductItemControl { Product = product };

        // Преобразуем Func в DataTemplate
        return new DataTemplate(() => new ProductItemControl());
    }

    // Создание чип-фильтра
    private FrameworkElement CreateFilterChip(string text)
    {
        var border = new Border
        {
            Background = new SolidColorBrush(Colors.LightGray),
            CornerRadius = new CornerRadius(20),
            Padding = new Thickness(12, 6, 12, 6)
        };

        var textBlock = new TextBlock
        {
            Text = text,
            FontSize = 14,
            Foreground = new SolidColorBrush(Colors.Black),
        };

        border.Child = textBlock;

        border.Tapped += (s, e) =>
        {
            // Сбрасываем все фильтры
            var parent = (s as Border)?.Parent as StackPanel;
            if (parent != null)
            {
                foreach (var child in parent.Children)
                {
                    if (child is Border b)
                    {
                        b.Background = new SolidColorBrush(Colors.LightGray);
                    }
                }
            }
            border.Background = new SolidColorBrush(Colors.Gray);

            // Здесь можно добавить логику фильтрации
        };

        return border;
    }

    // Создание нижней панели навигации
    private TabBar CreateTabBar()
    {
        var tabBar = new TabBar
        {
            Height = 50,
            Background = new SolidColorBrush(Colors.White),
            SelectedIndex = 1, // Холодильник выбран
            Margin = new Thickness(0, 0, 0, 5)
        };

        var tabItems = new object[]
        {
            new TabBarItem { Content = "Главная", Icon = new FontIcon { Glyph = "\uE80F" } },
            new TabBarItem { Content = "Холодильник", Icon = new FontIcon { Glyph = "\uE764" } },
            CreateScanButton(),
            new TabBarItem { Content = "Покупки", Icon = new FontIcon { Glyph = "\uE74C" } },
            new TabBarItem { Content = "Профиль", Icon = new FontIcon { Glyph = "\uE77B" } }
        };

        tabBar.ItemsSource = tabItems;
        return tabBar;
    }

    // Создание круглой кнопки сканирования
    private FrameworkElement CreateScanButton()
    {
        var scanButton = new Border
        {
            Width = 56,
            Height = 56,
            CornerRadius = new CornerRadius(28),
            Background = new SolidColorBrush(Color.FromArgb(255, 90, 238, 139)),
            Margin = new Thickness(0, -15, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top
        };

        var icon = new FontIcon
        {
            Glyph = "\uE756",
            FontSize = 28,
            Foreground = new SolidColorBrush(Colors.White)
        };

        scanButton.Child = icon;

        scanButton.Tapped += async (s, e) =>
        {
            var dialog = new ContentDialog
            {
                Title = "Сканирование",
                Content = "Открыть камеру для сканирования продукта",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        };

        return scanButton;
    }

    // Обработчик изменения текста поиска
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = (sender as TextBox)?.Text?.ToLower();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            _products.Clear();
            LoadSampleData();
        }
        else
        {
            var allProducts = GetAllProducts();
            var filtered = allProducts.Where(p => p.Name.ToLower().Contains(searchText)).ToList();

            _products.Clear();
            foreach (var product in filtered)
            {
                _products.Add(product);
            }
        }
    }

    // Загрузка тестовых данных
    private void LoadSampleData()
    {
        _products.Clear();

        _products.Add(new ProductItem
        {
            Name = "Молоко",
            Quantity = "1 л",
            ExpiryDate = DateTime.Now.AddDays(3),
            ImageUrl = "ms-appx:///Assets/Icons/milk.png",
            FreshnessLevel = 3
        });

        _products.Add(new ProductItem
        {
            Name = "Куриное филе",
            Quantity = "500 г",
            ExpiryDate = DateTime.Now.AddDays(1),
            ImageUrl = "ms-appx:///Assets/Icons/chicken.png",
            FreshnessLevel = 6
        });

        _products.Add(new ProductItem
        {
            Name = "Йогурт",
            Quantity = "3 шт",
            ExpiryDate = DateTime.Now.AddDays(5),
            ImageUrl = "ms-appx:///Assets/Icons/yogurt.png",
            FreshnessLevel = 2
        });

        _products.Add(new ProductItem
        {
            Name = "Яблоки",
            Quantity = "1 кг",
            ExpiryDate = DateTime.Now.AddDays(10),
            ImageUrl = "ms-appx:///Assets/Icons/apples.png",
            FreshnessLevel = 1
        });

        _products.Add(new ProductItem
        {
            Name = "Сыр",
            Quantity = "200 г",
            ExpiryDate = DateTime.Now.AddDays(7),
            ImageUrl = "ms-appx:///Assets/Icons/cheese.png",
            FreshnessLevel = 2
        });
    }

    // Вспомогательный метод для получения всех продуктов
    private ObservableCollection<ProductItem> GetAllProducts()
    {
        var all = new ObservableCollection<ProductItem>();

        all.Add(new ProductItem
        {
            Name = "Молоко",
            Quantity = "1 л",
            ExpiryDate = DateTime.Now.AddDays(3),
            ImageUrl = "ms-appx:///Assets/Icons/milk.png",
            FreshnessLevel = 3
        });

        all.Add(new ProductItem
        {
            Name = "Куриное филе",
            Quantity = "500 г",
            ExpiryDate = DateTime.Now.AddDays(1),
            ImageUrl = "ms-appx:///Assets/Icons/chicken.png",
            FreshnessLevel = 6
        });

        all.Add(new ProductItem
        {
            Name = "Йогурт",
            Quantity = "3 шт",
            ExpiryDate = DateTime.Now.AddDays(5),
            ImageUrl = "ms-appx:///Assets/Icons/yogurt.png",
            FreshnessLevel = 2
        });

        all.Add(new ProductItem
        {
            Name = "Яблоки",
            Quantity = "1 кг",
            ExpiryDate = DateTime.Now.AddDays(10),
            ImageUrl = "ms-appx:///Assets/Icons/apples.png",
            FreshnessLevel = 1
        });

        all.Add(new ProductItem
        {
            Name = "Сыр",
            Quantity = "200 г",
            ExpiryDate = DateTime.Now.AddDays(7),
            ImageUrl = "ms-appx:///Assets/Icons/cheese.png",
            FreshnessLevel = 2
        });

        return all;
    }
}
