using Microsoft.AspNetCore.Mvc;
using WebApiExample.Models;
using WebApiExample.Services;

namespace WebApiExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMoviesService _moviesService;

    public MoviesController(IMoviesService moviesService)
    {
        _moviesService = moviesService;
    }

    [HttpGet]
    public async Task<IEnumerable<Movie>> GetAsync(CancellationToken cancellationToken)
    {
        return await _moviesService.GetAsync(cancellationToken);
    }

    [HttpPost]
    public async Task<Movie> CreateAsync([FromBody] Movie movie, CancellationToken cancellationToken)
    {
        return await _moviesService.AddAsync(movie, cancellationToken);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _moviesService.DeleteAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            success => NoContent(), 
            notFound => NotFound());
      
    }
}
