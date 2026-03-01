using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Uno.UI.Extensions;

namespace UnoAppMobile.Presentation;

public sealed partial class RegistrationPage : Page
{
    private TextBox firstNameBox;
    private TextBox lastNameBox;
    private TextBox emailBox;
    private TextBox phoneBox;
    private DatePicker birthDatePicker;

    public RegistrationPage()
    {
        var stack = new StackPanel
        {
            Margin = new Thickness(20),
            Spacing = 15
        };

        // Заголовок
        stack.Children.Add(new TextBlock
        {
            Text = "Регистрация",
            FontSize = 28,
            FontWeight = FontWeights.Normal,
            FontFamily = new FontFamily("Assets/Fonts/SFProDisplayMedium.otf#SF Pro Display"),
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 20)
        });

        // Имя
        firstNameBox = new TextBox { Header = "Имя", PlaceholderText = "Введите имя" };
        stack.Children.Add(firstNameBox);

        // Фамилия
        lastNameBox = new TextBox { Header = "Фамилия", PlaceholderText = "Введите фамилию" };
        stack.Children.Add(lastNameBox);

        // Email
        emailBox = new TextBox
        {
            Header = "Email",
            PlaceholderText = "example@mail.ru",
            InputScope = new InputScope
            {
                Names = { new InputScopeName { NameValue = InputScopeNameValue.EmailNameOrAddress } }
            }
        };
        stack.Children.Add(emailBox);

        // Телефон
        phoneBox = new TextBox
        {
            Header = "Номер телефона",
            PlaceholderText = "+7 (999) 123-45-67",
            InputScope = new InputScope
            {
                Names = { new InputScopeName { NameValue = InputScopeNameValue.TelephoneNumber } }
            }
        };
        stack.Children.Add(phoneBox);

        // Дата рождения
        birthDatePicker = new DatePicker
        {
            Header = "Дата рождения",
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };
        stack.Children.Add(birthDatePicker);

        // Кнопка регистрации
        var registerButton = new Button
        {
            Content = "Зарегистрироваться",
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 20, 0, 0)
        };
        registerButton.Click += OnRegisterClick;
        stack.Children.Add(registerButton);

        // Оборачиваем в ScrollViewer для маленьких экранов
        var scrollViewer = new ScrollViewer { Content = stack };

        this.Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(scrollViewer);
    }

    private async void OnRegisterClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var firstName = firstNameBox.Text;
            var lastName = lastNameBox.Text;
            var email = emailBox.Text;
            var phone = phoneBox.Text;
            var birthDate = birthDatePicker.Date;

            var dialog = new ContentDialog
            {
                Title = "Регистрация",
                Content = $"Имя: {firstName}\nФамилия: {lastName}\nEmail: {email}\nДата рождения: {birthDate:d}\nТелефон: {phone}",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();

            Frame.Navigate(typeof(VerificationCodePage));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка регистрации: {ex}");
            var errorDialog = new ContentDialog
            {
                Title = "Ошибка",
                Content = "Произошла ошибка при регистрации.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await errorDialog.ShowAsync();
        }
    }
}
