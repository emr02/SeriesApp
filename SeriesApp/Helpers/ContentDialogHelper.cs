// Ajoute ce fichier (namespace SeriesApp.Helpers) et utilise-le depuis le VM.
// Il utilise App.MainRoot (voir fichier App.xaml.cs plus bas).
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace SeriesApp.Helpers;
public static class ContentDialogHelper
{
    public static async Task ShowAsync(string title, string content)
    {
        var dlg = new ContentDialog
        {
            Title = title,
            Content = content,
            CloseButtonText = "OK",
            XamlRoot = App.MainRoot?.XamlRoot
        };

        await dlg.ShowAsync();
    }
}