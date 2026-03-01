using System;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.UI;

namespace UnoAppMobile.Presentation;

public sealed partial class ProductItemControl : UserControl
{
    public static readonly DependencyProperty ProductProperty =
        DependencyProperty.Register(
            nameof(Product),
            typeof(ProductItem),
            typeof(ProductItemControl),
            new PropertyMetadata(null, OnProductChanged));

    public ProductItem Product
    {
        get => (ProductItem)GetValue(ProductProperty);
        set => SetValue(ProductProperty, value);
    }

    private static void OnProductChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (ProductItemControl)d;
        control.UpdateUI();
    }

    private Image _productImage;
    private TextBlock _nameText;
    private TextBlock _quantityText;
    private TextBlock _expiryText;
    private StackPanel _freshnessPanel;

    public ProductItemControl()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // фото
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // информация
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // индикатор
        grid.Margin = new Thickness(0, 8, 0, 8);

        // Фото продукта (с закруглением через Border)
        var imageBorder = new Border
        {
            Width = 70,
            Height = 70,
            CornerRadius = new CornerRadius(12),
            Background = new SolidColorBrush(Colors.LightGray),
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush(Colors.LightGray)
        };

        _productImage = new Image
        {
            Stretch = Stretch.UniformToFill
        };
        imageBorder.Child = _productImage;
        Grid.SetColumn(imageBorder, 0);
        grid.Children.Add(imageBorder);

        // Информация о продукте
        var infoStack = new StackPanel
        {
            Margin = new Thickness(12, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(infoStack, 1);
        grid.Children.Add(infoStack);

        _nameText = new TextBlock
        {
            FontSize = 18,
            FontWeight = FontWeights.SemiBold,
            FontFamily = new FontFamily("Assets/Fonts/SFProDisplayMedium.otf#SF Pro Display")
        };
        infoStack.Children.Add(_nameText);

        _quantityText = new TextBlock
        {
            FontSize = 14,
            Foreground = new SolidColorBrush(Colors.Gray),
            Margin = new Thickness(0, 2, 0, 2)
        };
        infoStack.Children.Add(_quantityText);

        _expiryText = new TextBlock
        {
            FontSize = 14
        };
        infoStack.Children.Add(_expiryText);

        // Индикатор свежести
        _freshnessPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 4
        };
        Grid.SetColumn(_freshnessPanel, 2);
        grid.Children.Add(_freshnessPanel);

        // Создаем 7 квадратиков заранее
        for (int i = 1; i <= 7; i++)
        {
            _freshnessPanel.Children.Add(new Border
            {
                Width = 16,
                Height = 16,
                CornerRadius = new CornerRadius(4),
                Name = $"Square{i}"
            });
        }

        this.Content = grid;
    }

    private void UpdateUI()
    {
        if (Product == null) return;

        // Загружаем изображение
        try
        {
            _productImage.Source = new BitmapImage(new Uri(Product.ImageUrl));
        }
        catch
        {
            // Если изображение не загрузилось, оставляем серый фон
        }

        // Текстовая информация
        _nameText.Text = Product.Name;
        _quantityText.Text = $"Количество: {Product.Quantity}";

        var daysLeft = (Product.ExpiryDate - DateTime.Now).Days;
        _expiryText.Text = $"Годен до: {Product.ExpiryDate:dd.MM.yyyy}";
        _expiryText.Foreground = new SolidColorBrush(daysLeft <= 2 ? Colors.Red : Colors.Gray);

        // Обновляем индикатор свежести
        for (int i = 0; i < _freshnessPanel.Children.Count; i++)
        {
            var square = (Border)_freshnessPanel.Children[i];
            int squareNumber = i + 1;

            if (squareNumber <= Product.FreshnessLevel)
            {
                // От зеленого к красному
                byte green = (byte)Math.Max(0, 255 - (Product.FreshnessLevel * 30));
                byte red = (byte)Math.Min(255, Product.FreshnessLevel * 30);
                square.Background = new SolidColorBrush(Color.FromArgb(255, red, green, 0));
            }
            else
            {
                square.Background = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
            }
        }
    }
}
