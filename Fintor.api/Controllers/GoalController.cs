using Application.DTOs.Goals;
using Application.Interfaces.UseCases.Goals;
using Application.UseCases.Goals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintor.api.Controllers
{
    [ApiController]
    [Route("api/goals")]
    public class GoalController : Controller
    {
        private readonly IGetAllGoals _getAllGoals;
        private readonly ICreateGoal _createGoal;

        public GoalController(IGetAllGoals getAllGoals, ICreateGoal createGoal)
        {
            _getAllGoals = getAllGoals;
            _createGoal = createGoal;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateGoal([FromBody] CreateGoalDTO createGoalDTO)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            GoalDTO goalDTO = await _createGoal.ExecuteAsync(createGoalDTO, userId);
            return Ok(goalDTO);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllGoals()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            List<GoalDTO> goalsDTO = await _getAllGoals.ExecuteAsync(userId);
            return Ok(goalsDTO);
        }
    }
}
