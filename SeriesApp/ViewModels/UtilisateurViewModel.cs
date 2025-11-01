// Exemple réduit et simple : utilise l'interface IService<Utilisateur> (qui contient GetByEmailAsync maintenant).
// Veille à inclure "using SeriesApp.Helpers;" si tu veux utiliser ContentDialogHelper directement.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeriesApp.Models;
using SeriesApp.Services;
using SeriesApp.Helpers;
using System;
using System.Threading.Tasks;

namespace SeriesApp.ViewModels;
public partial class UtilisateurViewModel : ObservableRecipient
{
    private readonly IService<Utilisateur> _wsService;
    private const string Controller = "utilisateurs";

    [ObservableProperty]
    private string _searchEmail = string.Empty;

    [ObservableProperty]
    private Utilisateur _current = new Utilisateur();

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

        // Appel via l'interface (qui contient GetByEmailAsync)
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

    private async Task SaveAsync()
    {
        if (Current == null) return;
        var ok = await _wsService.PutAsync(Controller, Current);
        await ContentDialogHelper.ShowAsync("Sauvegarde", ok ? "Modifications enregistrées." : "Erreur lors de la sauvegarde.");
    }

    private async Task AddAsync()
    {
        if (Current == null) return;
        var ok = await _wsService.PostAsync(Controller, Current);
        await ContentDialogHelper.ShowAsync("Ajout", ok ? "Utilisateur ajouté." : "Erreur lors de l'ajout.");
    }

    private async Task DeleteAsync()
    {
        if (Current == null || Current.Id == 0)
        {
            await ContentDialogHelper.ShowAsync("Suppression", "Aucun utilisateur sélectionné.");
            return;
        }
        var ok = await _wsService.DeleteAsync(Controller, Current);
        if (ok) Current = new Utilisateur();
        await ContentDialogHelper.ShowAsync("Suppression", ok ? "Utilisateur supprimé." : "Erreur lors de la suppression.");
    }
}