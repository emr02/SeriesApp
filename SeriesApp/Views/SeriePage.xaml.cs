using Microsoft.UI.Xaml.Controls;

using SeriesApp.ViewModels;

namespace SeriesApp.Views;

public sealed partial class SeriePage : Page
{
    public SerieViewModel ViewModel
    {
        get;
    }

    public SeriePage()
    {
        ViewModel = App.GetService<SerieViewModel>();
        InitializeComponent();
    }
}
