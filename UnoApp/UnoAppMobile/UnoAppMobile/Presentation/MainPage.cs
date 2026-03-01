
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Imaging;

namespace UnoAppMobile;

public sealed partial class MainPage : Page
{
    private DispatcherTimer _timer;
    public MainPage()
    {

        this
            .Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(new StackPanel()
                .VerticalAlignment(VerticalAlignment.Center)
                .HorizontalAlignment(HorizontalAlignment.Center)
                .Children(
                    new Image()
                    .Width(287)
                    .Height(287)
                    .Source(new BitmapImage(new Uri("ms-appx:///Assets/Icons/MainLogo.png"))),
                     new TextBlock()
                    .Text("SmartFridgeAI")
                    .FontFamily(new FontFamily("Assets/Fonts/VivaldiScript.ttf#Vivaldi script"))
                    .FontWeight(FontWeights.Normal)
                    .FontSize(45)

            ));

        StartNavigationTimer();

    }

    private void StartNavigationTimer()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(10);
        _timer.Tick += (s, e) =>
        {
            _timer.Stop();
            Frame.Navigate(typeof(CarouselPage));
        };
        _timer.Start();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
        _timer?.Stop();
    }
}
