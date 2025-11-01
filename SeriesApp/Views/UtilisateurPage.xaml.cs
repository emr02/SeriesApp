using Microsoft.UI.Xaml.Controls;
using SeriesApp.ViewModels;
using Microsoft.UI.Xaml;
using System.ComponentModel;

namespace SeriesApp.Views
{
    public sealed partial class UtilisateurPage : Page
    {
        public UtilisateurViewModel ViewModel
        {
            get;
        }

        public UtilisateurPage()
        {
            ViewModel = App.GetService<UtilisateurViewModel>();
            this.InitializeComponent();
            this.DataContext = ViewModel;

            // S'abonner aux changements de la VM (pour détecter Current)
            if (ViewModel is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += ViewModel_PropertyChanged;
            }

            // Initialiser PasswordBox si Current déjà renseigné
            PasswordBox.Password = ViewModel?.Current?.Pwd ?? string.Empty;
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Quand Current change, mettre à jour PasswordBox.Password
            if (e.PropertyName == nameof(ViewModel.Current))
            {
                var pwd = ViewModel?.Current?.Pwd ?? string.Empty;
                // Mettre à jour sur l'UI thread si nécessaire
                _ = DispatcherQueue.TryEnqueue(() =>
                {
                    PasswordBox.Password = pwd;
                });
            }
        }

        // Quand l'utilisateur saisit dans la PasswordBox on met à jour la VM
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb && ViewModel != null && ViewModel.Current != null)
            {
                ViewModel.Current.Pwd = pb.Password;
            }
        }
    }
}