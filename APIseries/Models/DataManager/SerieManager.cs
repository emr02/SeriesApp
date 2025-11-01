using APIseries.Models.EntityFramework;
using APIseries.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace APIseries.Models.DataManager
{
    public class SerieManager : IDataRepository<Serie>
    {
        private readonly SeriesDbContext dbContext;
        private readonly IMapper? _mapper;

        // constructeur de base
        public SerieManager(SeriesDbContext context)
        {
            dbContext = context;
        }

        // surcharge pour AutoMapper
        public SerieManager(SeriesDbContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Serie>> GetAllAsync()
        {
            return await dbContext.Series
                .Include(s => s.NotesSerie)
                .ToListAsync();
        }

        public async Task<ActionResult<Serie>> GetByIdAsync(int id)
        {
            var serie = await dbContext.Series
                .Include(s => s.NotesSerie)
                .FirstOrDefaultAsync(s => s.SerieId == id);

            if (serie == null)
            {
                return new ActionResult<Serie>(new NotFoundResult());
            }
            return new ActionResult<Serie>(serie);
        }

        public async Task<ActionResult<Serie>> GetByStringAsync(string titre)
        {
            var serie = await dbContext.Series
                .Include(s => s.NotesSerie)
                .FirstOrDefaultAsync(s => s.Titre.ToUpper() == titre.ToUpper());

            if (serie == null)
            {
                return new ActionResult<Serie>(new NotFoundResult());
            }
            return new ActionResult<Serie>(serie);
        }

        public async Task AddAsync(Serie entity)
        {
            await dbContext.Series.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Serie entity)
        {
            dbContext.Series.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Serie entity)
        {
            dbContext.Series.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        // --- Méthodes DTO ---

        // Retourne SerieDTO avec NoteMoyenne calculée
        public async Task<IEnumerable<SerieDTO>> GetAllSerieDTOAsync()
        {
            var series = await dbContext.Series
                .Include(s => s.NotesSerie)
                .ToListAsync();

            if (_mapper != null)
            {
                return _mapper.Map<List<SerieDTO>>(series);
            }

            return series.Select(s => new SerieDTO
            {
                Id = s.SerieId,
                Titre = s.Titre,
                NbSaisons = s.NbSaisons,
                NbEpisodes = s.NbEpisodes,
                NoteMoyenne = (s.NotesSerie != null && s.NotesSerie.Any()) ? s.NotesSerie.Average(n => n.Note) : 0.0
            }).ToList();
        }

        // Retourne SerieDetailDTO pour une série spécifique
        public async Task<SerieDetailDTO?> GetSerieDetailDTOAsync(int id)
        {
            var serie = await dbContext.Series
                .Include(s => s.NotesSerie)
                .FirstOrDefaultAsync(s => s.SerieId == id);

            if (serie == null)
            {
                return null;
            }

            if (_mapper != null)
            {
                return _mapper.Map<SerieDetailDTO>(serie);
            }

            return new SerieDetailDTO
            {
                Id = serie.SerieId,
                Titre = serie.Titre,
                Resume = serie.Resume,
                NbSaisons = serie.NbSaisons,
                NbEpisodes = serie.NbEpisodes,
                AnneeCreation = serie.AnneeCreation,
                Network = serie.Network,
                NoteMoyenne = (serie.NotesSerie != null && serie.NotesSerie.Any()) ? serie.NotesSerie.Average(n => n.Note) : 0.0
            };
        }
    }
}