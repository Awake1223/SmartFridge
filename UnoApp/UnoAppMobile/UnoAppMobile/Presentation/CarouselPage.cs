using System;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uno.UI.Extensions;
using UnoAppMobile.Presentation;

namespace UnoAppMobile;

public sealed partial class CarouselPage : Page
{
    public CarouselPage()
    {
        var items = new[]
        {
            new { Image = "ms-appx:///Assets/Icons/CarouselImage1.png", Caption = "Сканируй «Честный знак» и штрихкоды. Продукт добавится сам вместе со сроком годности и КБЖУ" },
            new { Image = "ms-appx:///Assets/Icons/CarouselImage2.png", Caption = "Умные уведомления напомнят, что нужно съесть сегодня. Экономь деньги и не выбрасывай еду." },
            new { Image = "ms-appx:///Assets/Icons/CarouselImage3.png", Caption = "Подберем рецепты из твоих продуктов. Если чего-то не хватает — добавим в список покупок." },
            new { Image = "ms-appx:///Assets/Icons/CarouselImage4.png", Caption = "Список недостающего сформируется сам. Заказ доставки или маршрут до магазина рядом." }
        };

        var flipView = new FlipView();

        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var panel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Spacing = 20
            };

            // Изображение
            panel.Children.Add(new Image
            {
                Width = 300,
                Height = 300,
                Source = new BitmapImage(new Uri(item.Image))
            });

            // Текст подписи
            panel.Children.Add(new TextBlock
            {
                Text = item.Caption,
                FontSize = 20,
                FontWeight = FontWeights.Normal,
                FontFamily = new FontFamily("Assets/Fonts/SFProDisplayMedium.otf#SF Pro Display"),
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 350
            });

            // Добавляем кнопку только на последний слайд
            if (i == items.Length - 1)
            {
                var button = new Button
                {
                    Content = "Зарегистрироваться",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                button.Click += (s, e) => Frame.Navigate(typeof(RegistrationPage));
                panel.Children.Add(button);
            }


            flipView.Items.Add(panel);
        }

        this.Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(flipView);
    }
}
