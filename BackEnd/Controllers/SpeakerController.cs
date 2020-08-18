using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpeakerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Speaker
        [HttpGet]
        public async Task<ActionResult<List<ConferenceDTO.SpeakerResponse>>> GetSpeakers()
        {
            var speakers = await _context.Speakers.AsNoTracking()
                                            .Include(s => s.SessionSpeakers)
                                                .ThenInclude(ss => ss.Session)
                                            .Select(s => s.MapSpeakerResponse())
                                            .ToListAsync();
            return speakers;
        }

        // GET: api/Speaker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConferenceDTO.SpeakerResponse>> GetSpeaker(int id)
        {
            var speaker = await _context.Speakers.AsNoTracking()
                                            .Include(s => s.SessionSpeakers)
                                                .ThenInclude(ss => ss.Session)
                                            .SingleOrDefaultAsync(s => s.Id == id);
            if (speaker == null)
            {
                return NotFound();
            }
            return speaker.MapSpeakerResponse();
        }
    }
}
