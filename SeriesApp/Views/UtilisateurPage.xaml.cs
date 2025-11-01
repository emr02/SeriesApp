using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SeriesApp.ViewModels;

namespace SeriesApp.Views;

public sealed partial class UtilisateurPage : Page
{
    public UtilisateurViewModel ViewModel
    {
        get;
    }

    public UtilisateurPage()
    {
        ViewModel = App.GetService<UtilisateurViewModel>();
        this.DataContext = ViewModel;
        InitializeComponent();
    }

    // Gestion du PasswordBox : met à jour la propriété ViewModel.Current.Password
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox pb) ViewModel.Current.Password = pb.Password;
    }
}