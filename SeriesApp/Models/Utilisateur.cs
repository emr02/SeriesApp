using System;

namespace SeriesApp.Models;

public class Utilisateur
{
    public int Id
    {
        get; set;
    }
    public string? Nom
    {
        get; set;
    }
    public string? Prenom
    {
        get; set;
    }
    public string? Mobile
    {
        get; set;
    }
    public string? Mail
    {
        get; set;
    }
    public string? Password
    {
        get; set;
    }
    public string? Adresse
    {
        get; set;
    }
}