using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApiExample.Database;
using WebApiExample.Exceptions;
using WebApiExample.Models;
using WebApiExample.Notifications;

namespace WebApiExample.Services;

public interface IMoviesService
{
    Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken);
    Task<IEnumerable<Movie>> GetAsync(CancellationToken cancellationToken);
    Task<OneOf<Unit, EntityNotFoundException>> DeleteAsync(int id, CancellationToken cancellationToken);
}

public class MoviesService : IMoviesService
{
    private readonly MovieDbContext _movieDbContext;
    private readonly IMediator _mediator;
    
    public MoviesService(MovieDbContext movieDbContext, IMediator mediator)
    {
        _movieDbContext = movieDbContext;
        _mediator = mediator;
    }
    
    public async Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken)
    {
        await _movieDbContext.Movies.AddAsync(movie, cancellationToken);
        
        await _movieDbContext.SaveChangesAsync(cancellationToken);
        
        await _mediator.Publish(new EntityAddedNotification(movie), cancellationToken);

        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAsync(CancellationToken cancellationToken)
    {
        return await _movieDbContext.Movies.ToListAsync(cancellationToken);
    }

    public async Task<OneOf<Unit, EntityNotFoundException>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        Movie? foundMovie = await _movieDbContext.Movies.FindAsync(id, cancellationToken);
        
        if (foundMovie is null)
        {
            return EntityNotFoundException.Instance;
        }

        _movieDbContext.Movies.Remove(foundMovie);
        
        await _movieDbContext.SaveChangesAsync(cancellationToken);
        
        await _mediator.Publish(new EntityDeletedNotification(foundMovie), cancellationToken);

        return Unit.Value;
    }
}
