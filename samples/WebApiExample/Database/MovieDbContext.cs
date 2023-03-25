using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApiExample.Models;

namespace WebApiExample.Database;

public sealed class MovieDbContext : DbContext
{
    private readonly IMediator _mediator;
    
    public MovieDbContext(DbContextOptions<MovieDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
        Movies = Set<Movie>();
    }

    public DbSet<Movie> Movies { get; set; }
}
