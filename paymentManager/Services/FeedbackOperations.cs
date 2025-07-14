using paymentManager.Data;
using paymentManager.DTOs;
using paymentManager.Models;
using System;


namespace paymentManager.Services
{
    public class FeedbackOperations
    {
        private readonly ApplicationDbContext _context;

        public FeedbackOperations(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Feedback> AddFeedbackAsync(FeedbackDto dto)
        {
            var feedback = new Feedback
            {
                Rating = dto.Rating,
                Tags = dto.Tags,
                Comment = dto.Comment
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }
    }
}
