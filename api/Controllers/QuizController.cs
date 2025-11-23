using api.DAL;
using api.DTOs;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuizController : ControllerBase
{
    private readonly IQuizRepository _repository;

    public QuizController(IQuizRepository repository)
    {
        _repository = repository;
    }

    // GET: api/Quiz
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizDto>>> GetQuizzes()
    {
        var quizzes = await _repository.GetAllQuizzes();
        
        // Map Entity -> DTO
        var quizDtos = quizzes.Select(q => new QuizDto
        {
            QuizId = q.QuizId,
            Title = q.Title,
            Description = q.Description,
            Questions = q.Questions.Select(qn => new QuestionDto
            {
                QuestionId = qn.QuestionId,
                Text = qn.Text,
                Type = qn.Type,
                // We don't necessarily need options for the list view, but it's fine for now
            }).ToList()
        });

        return Ok(quizDtos);
    }

    // GET: api/Quiz/5
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizDto>> GetQuiz(int id)
    {
        var quiz = await _repository.GetQuizById(id);

        if (quiz == null)
        {
            return NotFound();
        }

        // Map Entity -> DTO (Deep mapping)
        var quizDto = new QuizDto
        {
            QuizId = quiz.QuizId,
            Title = quiz.Title,
            Description = quiz.Description,
            Questions = quiz.Questions.Select(q => new QuestionDto
            {
                QuestionId = q.QuestionId,
                Text = q.Text,
                Type = q.Type,
                Options = q.Options.Select(o => new OptionDto
                {
                    OptionId = o.OptionId,
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()
            }).ToList()
        };

        return Ok(quizDto);
    }

    // POST: api/Quiz
    // [Authorize] means you must be logged in to create a quiz
    [HttpPost]
    [Authorize] 
    public async Task<ActionResult<Quiz>> CreateQuiz(QuizDto quizDto)
    {
        // Map DTO -> Entity
        var quiz = new Quiz
        {
            Title = quizDto.Title,
            Description = quizDto.Description,
            Questions = quizDto.Questions.Select(q => new Question
            {
                Text = q.Text,
                Type = q.Type,
                Options = q.Options.Select(o => new Option
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()
            }).ToList()
        };

        await _repository.CreateQuiz(quiz);

        return CreatedAtAction(nameof(GetQuiz), new { id = quiz.QuizId }, quiz);
    }

    // PUT: api/Quiz/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateQuiz(int id, QuizDto quizDto)
    {
        if (id != quizDto.QuizId)
        {
            return BadRequest();
        }

        // For simplicity in this demo, we will rely on EF Core's update behavior 
        // or you would typically fetch, map changes, and save.
        // To keep it simple for the "Project", we will recreate the entity structure.
        
        var quiz = await _repository.GetQuizById(id);
        if (quiz == null) return NotFound();

        // Update properties
        quiz.Title = quizDto.Title;
        quiz.Description = quizDto.Description;
        
        // Note: updating nested collections (Questions/Options) is complex in EF Core.
        // For this level of project, we often skip deep nested updates 
        // or implement a specific "UpdateQuestions" logic.
        // We will simply save the main changes here.
        
        await _repository.UpdateQuiz(quiz);

        return NoContent();
    }

    // DELETE: api/Quiz/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var quiz = await _repository.GetQuizById(id);
        if (quiz == null)
        {
            return NotFound();
        }

        await _repository.DeleteQuiz(id);

        return NoContent();
    }
}