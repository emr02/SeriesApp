using Microsoft.UI.Xaml.Controls;

using SeriesApp.ViewModels;

namespace SeriesApp.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
    }
}
