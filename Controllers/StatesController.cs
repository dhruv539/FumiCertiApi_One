using FumicertiApi.Data;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FumicertiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/states
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            return await _context.States.ToListAsync();
        }

        // GET: api/states/5
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(int id)
        {
            var state = await _context.States.FindAsync(id);

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        // POST: api/states
        [HttpPost]
        public async Task<ActionResult<State>> PostState(State state)
        {
            state.StateCreated = DateTime.UtcNow;
            _context.States.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetState), new { id = state.StateId }, state);
        }

        // PUT: api/states/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(int id, State updatedState)
        {
            if (id != updatedState.StateId)
            {
                return BadRequest("State ID mismatch.");
            }

            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            state.StateCompanyId = updatedState.StateCompanyId;
            state.StateAddedByUserId = updatedState.StateAddedByUserId;
            state.StateUpdatedByUserId = updatedState.StateUpdatedByUserId;
            state.StateName = updatedState.StateName;
            state.StateCode = updatedState.StateCode;
            state.StateStatus = updatedState.StateStatus;
            state.StateUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(state);
        }

        // DELETE: api/states/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _context.States.Remove(state);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
