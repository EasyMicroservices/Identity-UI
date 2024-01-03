using EasyMicroservices.UI.Cores;

namespace EasyMicroservices.UI.Identity.Maui.Helpers;
public class Language
{
    public static readonly BindableProperty KeyProperty = BindableProperty.CreateAttached("Key",
        typeof(string), typeof(Language),
        null,
        BindingMode.TwoWay,
        propertyChanged: PropertyChanged);

    public static string GetKey(BindableObject view)
    {
        return (string)view.GetValue(KeyProperty);
    }

    public static void SetKey(BindableObject view, string value)
    {
        view.SetValue(KeyProperty, value);
    }

    static ApiBaseViewModel ViewModel { get; set; } = new ApiBaseViewModel();
    static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var value = ViewModel.GetLanguage((string)newValue);
        if (bindable is Button button)
            button.Text = value + button.Text;
        else if (bindable is Label label)
            label.Text = value + label.Text;
    }
}
