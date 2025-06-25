using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace Movies.Application.Repositories
{
    public class MovieRepositoriy : IMovieRepository
    {

        private readonly IDbConnnectionFactory _dbConnnectionFactory;

        public MovieRepositoriy(IDbConnnectionFactory dbConnnectionFactory)
        {
            _dbConnnectionFactory = dbConnnectionFactory;
        }

        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            using var connection = await _dbConnnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert into movies(id, slug, title, yearofrelease)
                values (@Id, @Slug, @Title, @YearOfRelease)
                """, movie));
            if (result > 0)
            {
                foreach (var genre in movie.Genres)
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        insert into genres(movieID, name)
                        values(@MovieID, @Name)
                        """, new { MovieID = movie.Id, Name = genre }));
                }
            }

            transaction.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            if (!await ExistByID(id, token)) return true;

            using var connection = await _dbConnnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var genreDeleted = await connection.ExecuteAsync(new CommandDefinition("""
                delete from genres where genres.movieid = @id
                """, new { id }, cancellationToken: token));

            var movieDeleted = await connection.ExecuteAsync(new CommandDefinition("""
                delete from movies where movies.id = @id
                """, new { id }, cancellationToken: token));

            transaction.Commit();
            return (movieDeleted > 0);
        }

        public async Task<bool> ExistByID(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var idExists = await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                Select Count(1)
                From Movies
                Where id = @id
                """, new { id }, cancellationToken: token));

            return idExists;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
        {
            using var connection = await _dbConnnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var movieDictionary = new Dictionary<Guid, Movie>();

            var movies = await connection.QueryAsync<Movie, string, Movie>(new CommandDefinition("""
                select movies.*, genres.name As Genre
                From movies
                inner join genres
                on genres.movieid = movies.id
                """, cancellationToken: token),
                (movie, genre) =>
                {
                    if (movieDictionary.TryGetValue(movie.Id, out var currentMovie))
                    {
                        if (!string.IsNullOrEmpty(genre))
                        {
                            currentMovie.Genres.Add(genre);
                        }
                        return currentMovie;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(genre))
                        {
                            movie.Genres.Add(genre);
                        }
                        movieDictionary.Add(movie.Id, movie);
                        return movie;
                    }
                },
                splitOn: "Genre");

            transaction.Commit();
            return movieDictionary.Values;
        }

        public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var movieDictionary = new Dictionary<Guid, Movie>();

            var movies = await connection.QueryAsync<Movie, string, Movie>(new CommandDefinition("""
                select m.id, m.slug, m.title, m.yearofrelease, g.name as Genre
                from movies m
                left join genres g on g.movieid = m.id
                where m.id = @id
                """, new { id }, cancellationToken: token),
                (movie, genre) =>
                {
                    if (!movieDictionary.TryGetValue(movie.Id, out var movieEntry))
                    {
                        movieEntry = movie;
                        movieDictionary.Add(movie.Id, movieEntry);
                    }
                    if (!string.IsNullOrEmpty(genre))
                    {
                        movieEntry.Genres.Add(genre);
                    }
                    return movieEntry;
                },
                splitOn: "Genre");

            transaction.Commit();
            return movieDictionary.Values.FirstOrDefault();
        }

        public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            using var connection = await _dbConnnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var movieDictionary = new Dictionary<Guid, Movie>();

            var movies = await connection.QueryAsync<Movie, string, Movie>(new CommandDefinition("""
                select m.id, m.slug, m.title, m.yearofrelease, g.name as Genre
                from movies m
                left join genres g on g.movieid = m.id
                where m.slug = @slug
                """, new { slug }, cancellationToken: token),
                (movie, genre) =>
                {
                    if (!movieDictionary.TryGetValue(movie.Id, out var movieEntry))
                    {
                        movieEntry = movie;
                        movieDictionary.Add(movie.Id, movieEntry);
                    }
                    if (!string.IsNullOrEmpty(genre))
                    {
                        movieEntry.Genres.Add(genre);
                    }
                    return movieEntry;
                },
                splitOn: "Genre"
            );

            transaction.Commit();
            return movieDictionary.Values.FirstOrDefault();
        }

        public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
        {
            using var connection = await _dbConnnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            if (!await ExistByID(movie.Id, token)) return false;

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                update movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease
                where id = @Id
                """, movie, cancellationToken: token));

            if (result > 0)
            {
                foreach (var genre in movie.Genres)
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        insert into genres(movieid, name)
                        select @MovieID, @Name
                        where not exists
                        (select 1 from genres where movieid = @MovieID and name = @Name)
                        """, new { MovieID = movie.Id, Name = genre }, cancellationToken: token));
                }
            }

            transaction.Commit();
            return result > 0;
        }
    }
}