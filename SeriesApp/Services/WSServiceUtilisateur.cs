// Remplace ou mets à jour ce fichier. J'ajoute GetByEmailAsync public.
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SeriesApp.Models;

namespace SeriesApp.Services;
public class WSServiceUtilisateur : IService<Utilisateur>
{
    private readonly HttpClient httpClient;
    private readonly string _controller = "utilisateurs";

    public WSServiceUtilisateur()
    {
        httpClient = new HttpClient();
        // utilise ton API swagger (https) : modifie si nécessaire ou lis depuis Resources.resw
        httpClient.BaseAddress = new Uri("https://localhost:7271/api/");
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<Utilisateur>?> GetAllAsync(string? nomControleur)
    {
        try { return await httpClient.GetFromJsonAsync<List<Utilisateur>>(nomControleur ?? _controller); }
        catch { return null; }
    }

    public async Task<Utilisateur?> GetByIdAsync(string? nomControleur, int? id)
    {
        if (string.IsNullOrEmpty(nomControleur) || id == null) return null;
        try { return await httpClient.GetFromJsonAsync<Utilisateur>($"{nomControleur}/{id}"); }
        catch { return null; }
    }

    public async Task<Utilisateur?> GetByStringAsync(string? nomControleur, string? str)
    {
        if (string.IsNullOrEmpty(nomControleur) || string.IsNullOrEmpty(str)) return null;
        try { return await httpClient.GetFromJsonAsync<Utilisateur>($"{nomControleur}/{Uri.EscapeDataString(str)}"); }
        catch { return null; }
    }

    // --- méthode ajoutée demandée : GET /utilisateurs/getbyemail/{email}
    public async Task<Utilisateur?> GetByEmailAsync(string? nomControleur, string? email)
    {
        if (string.IsNullOrEmpty(nomControleur) || string.IsNullOrEmpty(email)) return null;
        try
        {
            var path = string.Concat(nomControleur.TrimEnd('/'), "/getbyemail/", Uri.EscapeDataString(email));
            return await httpClient.GetFromJsonAsync<Utilisateur>(path);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> PostAsync(string? nomControleur, Utilisateur? entity)
    {
        if (string.IsNullOrEmpty(nomControleur) || entity == null) return false;
        try
        {
            var response = await httpClient.PostAsJsonAsync(nomControleur, entity);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> PutAsync(string? nomControleur, Utilisateur? entity)
    {
        if (string.IsNullOrEmpty(nomControleur) || entity == null) return false;
        try
        {
            var response = await httpClient.PutAsJsonAsync(nomControleur, entity);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteAsync(string? nomControleur, Utilisateur? utilisateur)
    {
        if (string.IsNullOrEmpty(nomControleur) || utilisateur == null || utilisateur.Id == 0) return false;
        try
        {
            var response = await httpClient.DeleteAsync($"{nomControleur}/{utilisateur.Id}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}