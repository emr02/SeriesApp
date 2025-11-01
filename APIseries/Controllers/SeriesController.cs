using APIseries.Models.DataManager;
using APIseries.Models.EntityFramework;
using APIseries.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace APIseries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly IDataRepository<Serie> dataRepository;
        private readonly IMapper _mapper;

        public SeriesController(IDataRepository<Serie> dataRepo, IMapper mapper)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }

        // GET: api/Series
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Serie>))]
        public async Task<ActionResult<IEnumerable<Serie>>> GetSeries()
        {
            var series = await dataRepository.GetAllAsync();
            return Ok(series);
        }

        // GET: api/Series/dto
        [HttpGet("dto")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SerieDTO>))]
        public async Task<ActionResult<IEnumerable<SerieDTO>>> GetSeriesDto()
        {
            if (dataRepository is SerieManager mgr)
            {
                var dtos = await mgr.GetAllSerieDTOAsync();
                return Ok(dtos);
            }

            // fallback avec mapper
            var series = await dataRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SerieDTO>>(series));
        }

        // GET: api/Series/5/detail
        [HttpGet("{id}/detail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SerieDetailDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SerieDetailDTO>> GetSerieDetail(int id)
        {
            if (dataRepository is SerieManager mgr)
            {
                var dto = await mgr.GetSerieDetailDTOAsync(id);
                if (dto == null)
                {
                    return NotFound();
                }
                return Ok(dto);
            }

            // fallback
            var serie = await dataRepository.GetByIdAsync(id);
            if (serie == null || serie.Value == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<SerieDetailDTO>(serie.Value));
        }

        // GET: api/Series/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Serie))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Serie>> GetSerieById(int id)
        {
            var serie = await dataRepository.GetByIdAsync(id);

            if (serie == null || serie.Value == null)
            {
                return NotFound();
            }

            return serie;
        }

        // POST: api/Series
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Serie))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Serie>> PostSerie(Serie serie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await dataRepository.AddAsync(serie);

            return CreatedAtAction(nameof(GetSerieById), new { id = serie.SerieId }, serie);
        }

        // PUT: api/Series/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutSerie(int id, Serie serie)
        {
            if (id != serie.SerieId)
            {
                return BadRequest();
            }

            var serieToUpdate = await dataRepository.GetByIdAsync(id);

            if (serieToUpdate == null || serieToUpdate.Value == null)
            {
                return NotFound();
            }

            await dataRepository.UpdateAsync(serie);
            return NoContent();
        }

        // DELETE: api/Series/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSerie(int id)
        {
            var serie = await dataRepository.GetByIdAsync(id);
            if (serie == null || serie.Value == null)
            {
                return NotFound();
            }
            await dataRepository.DeleteAsync(serie.Value);
            return NoContent();
        }
    }
}