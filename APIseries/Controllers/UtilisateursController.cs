using APIseries.Models.DataManager;
using APIseries.Models.EntityFramework;
using APIseries.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace APIseries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly IDataRepository<Utilisateur> dataRepository;
        private readonly IMapper _mapper;

        // UN SEUL constructeur avec IMapper (injecté par DI)
        public UtilisateursController(IDataRepository<Utilisateur> dataRepo, IMapper mapper)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Utilisateur>))]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            var utilisateurs = await dataRepository.GetAllAsync();
            return Ok(utilisateurs);
        }

        // --- DTO endpoints ---

        // GET: api/Utilisateurs/dto
        [HttpGet("dto")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UtilisateurDTO>))]
        public async Task<ActionResult<IEnumerable<UtilisateurDTO>>> GetUtilisateursDto()
        {
            // si le repository est le manager concret, appeler la méthode DTO
            if (dataRepository is APIseries.Models.DataManager.UtilisateurManager mgr)
            {
                var dtos = await mgr.GetAllUtilisateurDTOAsync();
                return Ok(dtos);
            }

            // fallback : mapper avec IMapper
            var users = await dataRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<UtilisateurDTO>>(users));
        }

        // GET: api/Utilisateurs/notations
        [HttpGet("notations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NotationDTO>))]
        public async Task<ActionResult<IEnumerable<NotationDTO>>> GetNotationsDto()
        {
            if (dataRepository is APIseries.Models.DataManager.UtilisateurManager mgr)
            {
                var dtos = await mgr.GetAllNotationsDTOAsync();
                return Ok(dtos);
            }

            return StatusCode(500, "Repository ne supporte pas GetAllNotationsDTOAsync");
        }

        // --- keep existing endpoints (Get by id/email, Put, Post, Delete, Patch) ---

        // GET: api/Utilisateurs/GetUtilisateurById/{id}
        [HttpGet("GetUtilisateurById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Utilisateur))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurById(int id)
        {
            var utilisateur = await dataRepository.GetByIdAsync(id);

            if (utilisateur == null || utilisateur.Value == null)
            {
                return NotFound();
            }

            return utilisateur;
        }

        // GET: api/Utilisateurs/GetUtilisateurByEmail/{email}
        [HttpGet("GetUtilisateurByEmail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Utilisateur))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurByEmail(string email)
        {
            var utilisateur = await dataRepository.GetByStringAsync(email);

            if (utilisateur == null || utilisateur.Value == null)
            {
                return NotFound();
            }

            return utilisateur;
        }

        // PUT: api/Utilisateurs/PutUtilisateur/{id}
        [HttpPut("PutUtilisateur/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.UtilisateurId)
            {
                return BadRequest();
            }

            var userToUpdate = await dataRepository.GetByIdAsync(id);

            if (userToUpdate == null || userToUpdate.Value == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(utilisateur);
                return NoContent();
            }
        }

        // POST: api/Utilisateurs/PostUtilisateur
        [HttpPost("PostUtilisateur")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Utilisateur))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await dataRepository.AddAsync(utilisateur);

            return CreatedAtAction(nameof(GetUtilisateurById), new { id = utilisateur.UtilisateurId }, utilisateur);
        }

        // DELETE: api/Utilisateurs/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = await dataRepository.GetByIdAsync(id);
            if (utilisateur == null || utilisateur.Value == null)
            {
                return NotFound();
            }
            await dataRepository.DeleteAsync(utilisateur.Value);
            return NoContent();
        }

        // PATCH: api/Utilisateurs/{id}
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Utilisateur>> PatchUtilisateur(int id, [FromBody] JsonPatchDocument<Utilisateur> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var utilisateur = await dataRepository.GetByIdAsync(id);

            if (utilisateur == null || utilisateur.Value == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(utilisateur.Value, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.UpdateAsync(utilisateur.Value);
            return utilisateur;
        }
    }
}