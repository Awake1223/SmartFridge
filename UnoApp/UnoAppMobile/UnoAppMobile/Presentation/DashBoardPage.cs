using System;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uno.Toolkit.UI;
using Uno.UI.Extensions;
using Windows.Foundation;
using Windows.Networking.NetworkOperators;
using Windows.UI;

namespace UnoAppMobile.Presentation;

public sealed partial class DashBoardPage : Page
{
    public DashBoardPage()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        var grid = new Grid();

        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // приветствие
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // квадраты
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // таббар

        // --- Приветствие ---
        var headerBorder = new Border
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 143, 170, 232)),
            CornerRadius = new CornerRadius(20),
            Padding = new Thickness(16),
            Margin = new Thickness(20, 107, 20, 20),
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        var headerText = new TextBlock
        {
            Text = "Здравствуйте, Имя",
            FontSize = 24,
            FontWeight = FontWeights.Normal,
            FontFamily = new FontFamily("Assets/Fonts/SFProDisplayMedium.otf#SF Pro Display"),
            Foreground = new SolidColorBrush(Colors.White),
            HorizontalAlignment = HorizontalAlignment.Center
        };

        headerBorder.Child = headerText;
        Grid.SetRow(headerBorder, 0);
        grid.Children.Add(headerBorder);

        // --- Сетка для квадратов 2x2 ---
        var squaresGrid = new Grid
        {
            Margin = new Thickness(16, 0, 16, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        squaresGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        squaresGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        squaresGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        squaresGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        // Создаём квадраты с нужными цветами и разными картинками
        squaresGrid.Children.Add(CreateColoredSquare(
            title: "Срок годности",
            colorHex: "#FD8E7D",
            imagePath: "ms-appx:///Assets/Icons/ExpirationDate.png",
            column: 0, row: 0));

        squaresGrid.Children.Add(CreateColoredSquare(
            title: "Докупить",
            colorHex: "#73DE5D",
            imagePath: "ms-appx:///Assets/Icons/Basket.png",
            column: 1, row: 0));

        squaresGrid.Children.Add(CreateColoredSquare(
            title: "Идея ужина",
            colorHex: "#7DF8FD",
            imagePath: "ms-appx:///Assets/Icons/DinnerIdea.png",
            column: 0, row: 1));

        squaresGrid.Children.Add(CreateColoredSquare(
            title: "Статистика",
            colorHex: "#977DFD",
            imagePath: "ms-appx:///Assets/Icons/SircularRoad.png",
            column: 1, row: 1));

        Grid.SetRow(squaresGrid, 1);
        grid.Children.Add(squaresGrid);

        // --- Нижняя панель ---
        var tabBar = new TabBar
        {
            Height = 50,
            Background = new SolidColorBrush(Colors.White),
            SelectedIndex = 0,
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
        Grid.SetRow(tabBar, 2);
        grid.Children.Add(tabBar);

        tabBar.SelectionChanged += (s, e) =>
        {
            var currentType = Frame.CurrentSourcePageType;

            if (tabBar.SelectedIndex == 0 && currentType != typeof(DashBoardPage))
            {
                Frame.Navigate(typeof(DashBoardPage));
            }
            else if (tabBar.SelectedIndex == 1 && currentType != typeof(MyFridgePage))
            {
                Frame.Navigate(typeof(MyFridgePage));
            }
            // Индекс 2 (сканирование) обрабатывается в самой кнопке, здесь не нужно
            else if (tabBar.SelectedIndex == 3 && currentType != typeof(ShoppingPage))
            {
                // Frame.Navigate(typeof(ShoppingPage));
            }
            else if (tabBar.SelectedIndex == 4 && currentType != typeof(ProfilePage))
            {
                // Frame.Navigate(typeof(ProfilePage));
            }
        };

        this.Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(grid);
    }

    // Метод для создания цветного квадрата с иконкой и подписью под ним
    private Border CreateColoredSquare(string title, string colorHex, string imagePath, int column, int row)
    {
        // Преобразуем HEX в Color
        var color = Color.FromArgb(
            255,
            byte.Parse(colorHex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber),
            byte.Parse(colorHex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
            byte.Parse(colorHex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber)
        );

        // Внешний контейнер (сам квадрат)
        var border = new Border
        {
            Background = new SolidColorBrush(color),
            CornerRadius = new CornerRadius(16),
            Margin = new Thickness(8),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        // Внутренняя сетка для расположения иконки и подписи
        var innerGrid = new Grid();
        innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // иконка
        innerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // подпись

        // Иконка (изображение)
        var image = new Image
        {
            Width = 120,
            Height = 120,
            Source = new BitmapImage(new Uri(imagePath))
        };
        Grid.SetRow(image, 0);
        innerGrid.Children.Add(image);

        // Подпись под блоком (белым цветом)
        var caption = new TextBlock
        {
            Text = title,
            FontSize = 14,
            FontWeight = FontWeights.SemiBold,
            Foreground = new SolidColorBrush(Colors.White),
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 12),
            TextWrapping = TextWrapping.WrapWholeWords
        };
        Grid.SetRow(caption, 1);
        innerGrid.Children.Add(caption);

        border.Child = innerGrid;

        // Обработчик нажатия
        border.Tapped += async (s, e) =>
        {
            var dialog = new ContentDialog
            {
                Title = "Навигация",
                Content = $"Переход на страницу \"{title}\" (заглушка)",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        };

        Grid.SetColumn(border, column);
        Grid.SetRow(border, row);
        return border;
    }

    // Круглая кнопка сканирования
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
            Glyph = "\uE756", // камера
            FontSize = 28,
            Foreground = new SolidColorBrush(Colors.White)
        };

        scanButton.Child = icon;

        scanButton.Tapped += async (s, e) =>
        {
            var dialog = new ContentDialog
            {
                Title = "Сканирование",
                Content = "Открыть камеру для сканирования",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        };

        return scanButton;
    }
}
