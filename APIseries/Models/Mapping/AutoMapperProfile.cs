using APIseries.Models.DTO;
using APIseries.Models.EntityFramework;
using AutoMapper;
using System.Linq;

namespace APIseries.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // ==================== UTILISATEUR ====================

            // Utilisateur -> UtilisateurDTO
            CreateMap<Utilisateur, UtilisateurDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UtilisateurId))
                .ForMember(dest => dest.NoteMoyenne, opt => opt.MapFrom(src =>
                    (src.NotesUtilisateur != null && src.NotesUtilisateur.Any())
                        ? src.NotesUtilisateur.Average(n => n.Note)
                        : 0.0));

            // Utilisateur -> UtilisateurDetailDTO
            CreateMap<Utilisateur, UtilisateurDetailDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UtilisateurId));

            // ==================== NOTATION ====================

            // Notation -> NotationDTO
            CreateMap<Notation, NotationDTO>()
                .ForMember(dest => dest.NomUtilisateur, opt => opt.MapFrom(src =>
                    src.UtilisateurNotant != null ? src.UtilisateurNotant.Nom : null))
                .ForMember(dest => dest.TitreSerie, opt => opt.MapFrom(src =>
                    src.SerieNotee != null ? src.SerieNotee.Titre : null))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));

            // ==================== SERIE ====================

            // Serie -> SerieDTO
            CreateMap<Serie, SerieDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SerieId))
                .ForMember(dest => dest.NoteMoyenne, opt => opt.MapFrom(src =>
                    (src.NotesSerie != null && src.NotesSerie.Any())
                        ? src.NotesSerie.Average(n => n.Note)
                        : 0.0));

            // Serie -> SerieDetailDTO
            CreateMap<Serie, SerieDetailDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SerieId))
                .ForMember(dest => dest.NoteMoyenne, opt => opt.MapFrom(src =>
                    (src.NotesSerie != null && src.NotesSerie.Any())
                        ? src.NotesSerie.Average(n => n.Note)
                        : 0.0));
        }
    }
}