using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Text;

namespace UnoAppMobile.Presentation;
public sealed partial class VerificationCodePage : Page
{
    private const int CodeLength = 4;

    private TextBox[] codeBoxes;

    private Button verifyButton;

    public VerificationCodePage() 
    {
        BuildUI();
    }

    public void BuildUI()
    {
        var mainStack = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 20,
            Margin = new Thickness(20)
        };

        mainStack.Children.Add(new TextBlock
        {
            Text = "Подтверждение номера",
            FontSize = 28,
            FontFamily = new FontFamily("Assets/Fonts/SFProDisplayMedium.otf#SF Pro Display"),
            FontWeight = FontWeights.Normal,
            HorizontalAlignment = HorizontalAlignment.Center

        });

        mainStack.Children.Add(new TextBlock
        {
            Text = "Код отправлен на +7 999 123-45-67",
            FontSize = 16,
            FontFamily = new FontFamily("Assets/Fonts/SFProDisplayMedium.otf#SF Pro Display"),
            HorizontalAlignment = HorizontalAlignment.Center
        });

        var codePanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 10,
        };

        codeBoxes = new TextBox[CodeLength];

        for (int i = 0; i < CodeLength; i++)
        {
            var box = new TextBox
            {
                Width = 60,
                Height = 60,
                FontSize = 32,
                FontWeight = FontWeights.Bold,
                TextAlignment = Microsoft.UI.Xaml.TextAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                MaxLength = 1,
                PlaceholderText = "0",
                InputScope = new InputScope
                {
                    Names = { new InputScopeName { NameValue = InputScopeNameValue.TelephoneNumber } }
                }
            };

            box.TextChanged += (sender, e) => OnCodeBoxChanged(sender as TextBox, i);
            box.KeyDown += OnCodeBoxKeyDown;
            box.GotFocus += (sender, e) => ((TextBox)sender).SelectAll();

            codeBoxes[i] = box;
            codePanel.Children.Add(box);
        }

        mainStack.Children.Add(codePanel);

        verifyButton = new Button
        {
            Content = "Подтверидть",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness(0, 20, 0, 0),
            IsEnabled = false,
        };


        verifyButton.Click += OnVerifyClick;

        mainStack.Children.Add(verifyButton);

        var resendButton = new Button
        {
            Content = "Отправить код повторно",
            HorizontalAlignment = HorizontalAlignment.Center,
            Style = Application.Current.Resources["TextButtonStyle"] as Style
        };

        resendButton.Click += OnResendClick;

        mainStack.Children.Add(resendButton);

        this.Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(mainStack);
    }
    

    private void OnCodeBoxChanged(TextBox currentBox, int index)
    { 
        if(currentBox.Text.Length == 1 && index < CodeLength - 1)
        {
            codeBoxes[index + 1].Focus(FocusState.Programmatic);
        }

        verifyButton.IsEnabled = codeBoxes.All(box => box.Text.Length == 1);
    }

    private void OnCodeBoxKeyDown( object sender, KeyRoutedEventArgs e)
    {
        var box = sender as TextBox;
        int index = Array.IndexOf(codeBoxes, box);
        if (e.Key == Windows.System.VirtualKey.Back && string.IsNullOrEmpty(box.Text) && index > 0)
        {
            // Если текущий пуст и нажат Backspace, переходим к предыдущему
            codeBoxes[index - 1].Focus(FocusState.Programmatic);
        }
    }

    private async void OnVerifyClick(object sender, RoutedEventArgs e)
    {
        // Собираем код из всех боксов
        string code = string.Concat(codeBoxes.Select(b => b.Text));
        // Здесь логика проверки кода (API)
        var dialog = new ContentDialog
        {
            Title = "Код подтверждения",
            Content = $"Введён код: {code}",
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        await dialog.ShowAsync();

        // При успехе переходим на следующую страницу
         Frame.Navigate(typeof(DashBoardPage));
    }

    private async void OnResendClick(object sender, RoutedEventArgs e)
    {
        // Логика повторной отправки кода
        var dialog = new ContentDialog
        {
            Title = "Повторная отправка",
            Content = "Код отправлен повторно",
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        await dialog.ShowAsync();
    }

}
