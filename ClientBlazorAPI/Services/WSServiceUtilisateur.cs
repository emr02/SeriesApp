using ClientBlazorAPI.Models;
using ClientBlazorAPI.Services;
using System.Net.Http;
using System.Net.Http.Json;

public class WSServiceUtilisateur : IService<Utilisateur>
{
    private readonly HttpClient httpClient;

    public WSServiceUtilisateur(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<bool> CreateAsync(string nomControleur, Utilisateur entity)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(nomControleur, entity);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }


    public async Task<List<Utilisateur>?> GetAllAsync(string nomControleur)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<List<Utilisateur>>(nomControleur);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Utilisateur?> GetByIdAsync(string nomControleur, int id)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<Utilisateur>($"{nomControleur}/{id}");
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, Utilisateur utilisateur)
    {
        // base address est https://localhost:7271/api/ :
        var response = await httpClient.PutAsJsonAsync($"Utilisateurs/PutUtilisateur/{id}", utilisateur);

        return response.IsSuccessStatusCode;
    }

    public async Task<Utilisateur> GetByEmailAsync(string email)
    {
        var encodedEmail = System.Net.WebUtility.UrlEncode(email);
        return await httpClient.GetFromJsonAsync<Utilisateur>($"Utilisateurs/GetUtilisateurByEmail/{encodedEmail}");
    }

    // Autres méthodes non implémentées
    public Task<Utilisateur?> GetByStringAsync(string nomControleur, string str) => throw new NotImplementedException();
    public Task<bool> PostAsync(string nomControleur, Utilisateur entity) => throw new NotImplementedException();
    public Task<bool> PutAsync(string nomControleur, Utilisateur entity) => throw new NotImplementedException();
    public Task<bool> DeleteAsync(string nomControleur, Utilisateur entity) => throw new NotImplementedException();
}