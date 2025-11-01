using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeriesApp.Models;
using SeriesApp.Services;
using SeriesApp.Helpers;
using System.Windows.Input;

namespace SeriesApp.ViewModels
{
    public partial class UtilisateurViewModel : ObservableRecipient
    {
        private readonly IService<Utilisateur> _wsService;
        private const string Controller = "Utilisateurs"; // correspond au controller de l'API

        [ObservableProperty]
        private string _searchEmail = string.Empty;

        [ObservableProperty]
        private Utilisateur _current = new Utilisateur();

        // Commandes réelles
        public IAsyncRelayCommand SearchCommand
        {
            get;
        }
        public IAsyncRelayCommand SaveCommand
        {
            get;
        }
        public IAsyncRelayCommand AddCommand
        {
            get;
        }
        public IAsyncRelayCommand DeleteCommand
        {
            get;
        }
        public IRelayCommand ClearCommand
        {
            get;
        }

        // Aliases / propriétés attendues par le XAML (noms du TP)
        public ICommand BtnModifyUtilisateurCommand => SaveCommand;
        public ICommand BtnClearUtilisateurCommand => ClearCommand;
        public ICommand BtnAddUtilisateurCommand => AddCommand;
        public ICommand BtnDeleteUtilisateurCommand => DeleteCommand;

        public UtilisateurViewModel(IService<Utilisateur> wsService)
        {
            _wsService = wsService ?? throw new ArgumentNullException(nameof(wsService));

            SearchCommand = new AsyncRelayCommand(SearchAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync);
            AddCommand = new AsyncRelayCommand(AddAsync);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync);
            ClearCommand = new RelayCommand(() => Current = new Utilisateur());
        }

        private async Task SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchEmail))
            {
                await ContentDialogHelper.ShowAsync("Recherche", "Saisissez une adresse e‑mail.");
                return;
            }

            var user = await _wsService.GetByEmailAsync(Controller, SearchEmail);
            if (user != null)
            {
                Current = user;
            }
            else
            {
                Current = new Utilisateur();
                await ContentDialogHelper.ShowAsync("Recherche", "Aucun utilisateur trouvé.");
            }
        }

        // Simple validation client avant envoi pour éviter 400 basiques côté API
        private bool ValidateForSend(out string message)
        {
            if (Current == null) { message = "Utilisateur vide"; return false; }
            if (string.IsNullOrWhiteSpace(Current.Nom)) { message = "Nom requis"; return false; }
            if (string.IsNullOrWhiteSpace(Current.Prenom)) { message = "Prénom requis"; return false; }
            if (string.IsNullOrWhiteSpace(Current.Mail)) { message = "Mail requis"; return false; }
            if (string.IsNullOrWhiteSpace(Current.Pwd)) { message = "Mot de passe requis"; return false; }
            message = string.Empty; return true;
        }

        private async Task SaveAsync()
        {
            if (Current == null)
            {
                await ContentDialogHelper.ShowAsync("Sauvegarde", "Utilisateur vide");
                return;
            }

            if (Current.UtilisateurId == 0)
            {
                await ContentDialogHelper.ShowAsync("Sauvegarde", "Impossible : aucun identifiant pour mise à jour.");
                return;
            }

            if (!ValidateForSend(out var vmsg))
            {
                await ContentDialogHelper.ShowAsync("Sauvegarde", $"Validation : {vmsg}");
                return;
            }

            var wsConcrete = _wsService as WSServiceUtilisateur;
            if (wsConcrete == null)
            {
                var ok = await _wsService.PutAsync(Controller, Current);
                await ContentDialogHelper.ShowAsync("Sauvegarde", ok ? "Modifications enregistrées." : "La sauvegarde a échouée.");
                return;
            }

            var (success, response) = await wsConcrete.PutAsyncWithResponse(Controller, Current);
            if (success)
            {
                // Attendre que la DB committe avant de recharger
                await Task.Delay(1000); // 1 seconde pour être sûr

                // Recharger pour synchroniser avec la DB
                var refreshed = await _wsService.GetByIdAsync(Controller, Current.UtilisateurId);
                if (refreshed != null)
                {
                    Current = refreshed;
                }

                await ContentDialogHelper.ShowAsync("Sauvegarde", "Modifications enregistrées.");
            }
            else
            {
                await ContentDialogHelper.ShowAsync("Sauvegarde échouée", response);
            }
        }

        private async Task AddAsync()
        {
            if (Current == null)
            {
                await ContentDialogHelper.ShowAsync("Ajout", "Utilisateur vide");
                return;
            }

            if (!ValidateForSend(out var vmsg))
            {
                await ContentDialogHelper.ShowAsync("Ajout", $"Validation : {vmsg}");
                return;
            }

            var wsConcrete = _wsService as WSServiceUtilisateur;
            if (wsConcrete == null)
            {
                var ok = await _wsService.PostAsync(Controller, Current);
                await ContentDialogHelper.ShowAsync("Ajout", ok ? "Utilisateur ajouté avec succès." : "L'ajout a échoué.");
                return;
            }

            var (success, response) = await wsConcrete.PostAsyncWithResponse(Controller, Current);
            if (success)
            {
                await ContentDialogHelper.ShowAsync("Ajout", "Utilisateur ajouté avec succès.");
            }
            else
            {
                // Affiche le message renvoyé par le serveur (ModelState JSON ou message)
                await ContentDialogHelper.ShowAsync("Ajout échoué", response);
            }
        }

        private async Task DeleteAsync()
        {
            if (Current == null || (Current.UtilisateurId == 0 && Current.Id == 0))
            {
                await ContentDialogHelper.ShowAsync("Suppression", "Aucun utilisateur sélectionné.");
                return;
            }

            var id = (Current.UtilisateurId != 0) ? Current.UtilisateurId : Current.Id;
            var tmp = new Utilisateur { UtilisateurId = id, Id = id };
            var ok = await _wsService.DeleteAsync(Controller, tmp);
            if (ok) Current = new Utilisateur();
            await ContentDialogHelper.ShowAsync("Suppression", ok ? "Utilisateur supprimé." : "Erreur lors de la suppression.");
        }
    }
}