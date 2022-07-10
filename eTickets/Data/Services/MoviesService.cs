using eTickets.Data.Base;
using eTickets.Models;
using eTickets.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services;

public class MoviesService: EntityBaseRepository<Movie>, IMoviesService
{
    private readonly AppDbContext _context;
    
    public MoviesService(AppDbContext context): base(context)
    {
        _context = context;
    }

    public async Task<Movie> GetMovieByIdAsync(int id)
    {
        var movieDetail = await _context.Movies
            .Include(m => m.Cinema)
            .Include(m => m.Producer)
            .Include(m => m.Actors_Movies).ThenInclude(a => a.Actor)
            .FirstOrDefaultAsync(m => m.Id == id);

        return movieDetail;
    }

    public async Task<NewMovieDropdownVM> GetNewMovieDropDownValue()
    {
        var response = new NewMovieDropdownVM()
        {
            Actors = await _context.Actors.OrderBy(a => a.FullName).ToListAsync(),
            Cinemas = await _context.Cinemas.OrderBy(a => a.Name).ToListAsync(),
            Producers = await _context.Producers.OrderBy(a => a.FullName).ToListAsync(),
        };

        return response;

    }

    public async Task AddNewMovieAsync(NewMovieVM data)
    {
        var movie = new Movie()
        {
            Name = data.Name,
            Description = data.Description,
            Price = data.Price,
            ImageURL = data.ImageURL,
            CinemaId = data.CinemaId,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            MovieCategory = data.MovieCategory,
            ProducerId = data.ProducerId
        };

        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        
        // Add Movie Actors
        foreach (var actorId in data.ActorIds)
        {
            var newActorMovie = new Actor_Movie()
            {
                MovieId = movie.Id,
                ActorId = actorId
            };
            await _context.Actors_Movies.AddAsync(newActorMovie);
        }
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMovieAsync(NewMovieVM data)
    {
        var dbMovie = await _context.Movies.FirstOrDefaultAsync(n => n.Id == data.Id);
        
        if (dbMovie != null)
        {
            dbMovie.Name = data.Name;
            dbMovie.Description = data.Description;
            dbMovie.Price = data.Price;
            dbMovie.ImageURL = data.ImageURL;
            dbMovie.CinemaId = data.CinemaId;
            dbMovie.StartDate = data.StartDate;
            dbMovie.EndDate = data.EndDate;
            dbMovie.MovieCategory = data.MovieCategory;
            dbMovie.ProducerId = data.ProducerId;
            
            await _context.SaveChangesAsync();
        }
        // Remove all existing Actors
        var existingActorsdb = 
            _context.Actors_Movies.Where(am => am.MovieId == data.Id).ToList();

        _context.Actors_Movies.RemoveRange(existingActorsdb);
        await _context.SaveChangesAsync();

        // Add new Movie Actors
        foreach (var actorId in data.ActorIds)
        {
            var newActorMovie = new Actor_Movie()
            {
                MovieId = data.Id,
                ActorId = actorId
            };
            await _context.Actors_Movies.AddAsync(newActorMovie);
        }
        await _context.SaveChangesAsync();
    }
}