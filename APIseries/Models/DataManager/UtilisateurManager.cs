using APIseries.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using APIseries.Models.DTO;

namespace APIseries.Models.DataManager
{
    public class UtilisateurManager : IDataRepository<Utilisateur>
    {
        private readonly SeriesDbContext filmsDbContext;
        private readonly IMapper? _mapper;

        // constructeur existant (pour tes tests actuels)
        public UtilisateurManager(SeriesDbContext context)
        {
            filmsDbContext = context;
        }

        // surcharge pour l'injection d'AutoMapper (via DI si tu l'enregistres)
        public UtilisateurManager(SeriesDbContext context, IMapper mapper)
        {
            filmsDbContext = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Utilisateur>> GetAllAsync()
        {
            // inclure les notes pour éviter des surprises si on veut la moyenne côté mapping
            return await filmsDbContext.Utilisateurs
                .Include(u => u.NotesUtilisateur)
                .ToListAsync();
        }

        public async Task<ActionResult<Utilisateur>> GetByIdAsync(int id)
        {
            var user = await filmsDbContext.Utilisateurs
                .Include(u => u.NotesUtilisateur) // inclure notes
                .FirstOrDefaultAsync(u => u.UtilisateurId == id);

            if (user == null)
            {
                // retourne un ActionResult avec Result = NotFound (wrapper non-null)
                return new ActionResult<Utilisateur>(new NotFoundResult());
            }
            return new ActionResult<Utilisateur>(user);
        }

        public async Task<ActionResult<Utilisateur>> GetByStringAsync(string mail)
        {
            var user = await filmsDbContext.Utilisateurs
                .Include(u => u.NotesUtilisateur)
                .FirstOrDefaultAsync(u => u.Mail.ToUpper() == mail.ToUpper());

            if (user == null)
            {
                return new ActionResult<Utilisateur>(new NotFoundResult());
            }
            return new ActionResult<Utilisateur>(user);
        }

        public async Task AddAsync(Utilisateur entity)
        {
            await filmsDbContext.Utilisateurs.AddAsync(entity);
            await filmsDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Utilisateur entity)
        {
            // Détacher toute instance déjà trackée avec la même clé pour éviter conflit
            var tracked = filmsDbContext.ChangeTracker.Entries<Utilisateur>()
                .FirstOrDefault(e => e.Entity.UtilisateurId == entity.UtilisateurId);
            if (tracked != null)
                tracked.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            filmsDbContext.Utilisateurs.Update(entity);
            await filmsDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Utilisateur entity)
        {
            filmsDbContext.Utilisateurs.Remove(entity);
            await filmsDbContext.SaveChangesAsync();
        }

        // --- Méthodes DTO demandées par le TP ---

        // Retourne UtilisateurDTO avec NoteMoyenne calculée
        public async Task<IEnumerable<UtilisateurDTO>> GetAllUtilisateurDTOAsync()
        {
            var users = await filmsDbContext.Utilisateurs
                .Include(u => u.NotesUtilisateur)
                .ToListAsync();

            if (_mapper != null)
            {
                return _mapper.Map<List<UtilisateurDTO>>(users);
            }

            return users.Select(u => new UtilisateurDTO
            {
                Id = u.UtilisateurId,
                Nom = u.Nom,
                Prenom = u.Prenom,
                NoteMoyenne = (u.NotesUtilisateur != null && u.NotesUtilisateur.Any()) ? u.NotesUtilisateur.Average(n => n.Note) : 0.0
            }).ToList();
        }

        // Retourne NotationDTO triées par TitreSerie puis NomUtilisateur
        public async Task<IEnumerable<NotationDTO>> GetAllNotationsDTOAsync()
        {
            var notes = await filmsDbContext.Notations
                .Include(n => n.UtilisateurNotant)
                .Include(n => n.SerieNotee)
                .ToListAsync();

            if (_mapper != null)
            {
                var mapped = _mapper.Map<List<NotationDTO>>(notes);
                return mapped.OrderBy(n => n.TitreSerie).ThenBy(n => n.NomUtilisateur).ToList();
            }

            return notes.Select(n => new NotationDTO
            {
                NomUtilisateur = n.UtilisateurNotant?.Nom,
                TitreSerie = n.SerieNotee?.Titre,
                Note = n.Note
            }).OrderBy(n => n.TitreSerie).ThenBy(n => n.NomUtilisateur).ToList();
        }
    }
}