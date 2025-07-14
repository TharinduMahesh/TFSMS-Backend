using Microsoft.AspNetCore.Mvc;
using paymentManager.DTOs;
using paymentManager.Services;
using paymentManager.DTOs;


namespace paymentManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksController : ControllerBase
    {
        private readonly FeedbackOperations _feedbackOps;

        public FeedbacksController(FeedbackOperations feedbackOps)
        {
            _feedbackOps = feedbackOps;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FeedbackDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _feedbackOps.AddFeedbackAsync(dto);
            return CreatedAtAction(nameof(Post), new { id = result.Id }, result);
        }
    }
}
