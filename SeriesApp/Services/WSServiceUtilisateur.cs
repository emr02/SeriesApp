using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SeriesApp.Models;

namespace SeriesApp.Services;
public class WSServiceUtilisateur : IService<Utilisateur>
{
    private readonly HttpClient httpClient;
    private const string Controller = "Utilisateurs"; // <-- nom du controller de l'API

    public WSServiceUtilisateur()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
        };

        httpClient = new HttpClient(handler);
        httpClient.BaseAddress = new Uri("https://localhost:7271/api/");
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<Utilisateur>?> GetAllAsync(string? nomControleur)
    {
        try { return await httpClient.GetFromJsonAsync<List<Utilisateur>>(nomControleur ?? Controller); }
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

    public async Task<Utilisateur?> GetByEmailAsync(string? nomControleur, string? email)
    {
        if (string.IsNullOrEmpty(email)) return null;
        try
        {
            var path = string.Concat(Controller, "/GetUtilisateurByEmail/", Uri.EscapeDataString(email));
            return await httpClient.GetFromJsonAsync<Utilisateur>(path);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string? nomControleur, Utilisateur? utilisateur)
    {
        if (utilisateur == null) return false;
        var id = utilisateur.UtilisateurId != 0 ? utilisateur.UtilisateurId : utilisateur.Id;
        if (id == 0) return false;

        try
        {
            var response = await httpClient.DeleteAsync($"{Controller}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // --- Debug / Diagnostic versions that return the response body ---
    // Useful to show server validation errors (ModelState) to the client.

    // POST: api/Utilisateurs/PostUtilisateur
    public async Task<(bool Success, string ResponseContent)> PostAsyncWithResponse(string? nomControleur, Utilisateur? entity)
    {
        if (entity == null) return (false, "Entity is null");
        try
        {
            var path = string.Concat(Controller, "/PostUtilisateur");
            var response = await httpClient.PostAsJsonAsync(path, entity);
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"POST {path} -> {response.StatusCode} : {content}");
            return (response.IsSuccessStatusCode, content);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"POST exception: {ex}");
            return (false, ex.Message);
        }
    }

    // PUT: api/Utilisateurs/PutUtilisateur/{id}
    public async Task<(bool Success, string ResponseContent)> PutAsyncWithResponse(string? nomControleur, Utilisateur? entity)
    {
        if (entity == null) return (false, "Entity is null");
        var id = entity.UtilisateurId != 0 ? entity.UtilisateurId : entity.Id;
        if (id == 0) return (false, "Id is 0 or missing");

        try
        {
            var path = string.Concat(Controller, "/PutUtilisateur/", id);
            var response = await httpClient.PutAsJsonAsync(path, entity);
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"PUT {path} -> {response.StatusCode} : {content}");
            return (response.IsSuccessStatusCode, content);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"PUT exception: {ex}");
            return (false, ex.Message);
        }
    }

    // Implement interface methods as simple wrappers (keeps compatibility)
    public async Task<bool> PostAsync(string? nomControleur, Utilisateur? entity)
    {
        var (ok, _) = await PostAsyncWithResponse(nomControleur, entity);
        return ok;
    }

    public async Task<bool> PutAsync(string? nomControleur, Utilisateur? entity)
    {
        var (ok, _) = await PutAsyncWithResponse(nomControleur, entity);
        return ok;
    }
}