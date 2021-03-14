using BlazorCRUD.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BlCRUD.Data.Dapper.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private string ConnectionString;
        public FilmRepository(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        protected SqlConnection dbConecction()
        {
            return new SqlConnection(ConnectionString);
        }
        public async Task<bool> DeleteFilm(int id)
        {
            var db = dbConecction();
            var sql = @"DELETE FROM Films WHERE Id = @Id";
            var result = await db.ExecuteAsync(sql.ToString(), new {Id = id});
            return result > 0;
        }

        public async Task<IEnumerable<Film>> GetAllFilms()
        {
            var db = dbConecction();
            var sql = @" SELECT Id, Title, Director, RelaseDate
                         FROM [dbo].[Films]";
            return await db.QueryAsync<Film>(sql.ToString(), new { });
        }

        public async Task<Film> GetFilmDetails(int id)
        {
            var db = dbConecction();
            var sql = @" SELECT Id, Title, Director, RelaseDate
                         FROM [dbo].[Films]
                         WHERE Id = @Id";
            return await db.QueryFirstOrDefaultAsync<Film>(sql.ToString(), new { Id = id });
        }

        public async Task<bool> InsertFilm(Film film)
        {
            var db = dbConecction();
            var sql = @"
                        INSERT INTO Films (Title, Director, RelaseDate)
                        VALUES (@Title, @Director, @RelaseDate)
                        ";
            var result = await db.ExecuteAsync(sql.ToString(), new { film.Title, film.Director, film.RelaseDate });
            
            return result > 0;
        }

        public async Task<bool> UpdateFilm(Film film)
        {
            var db = dbConecction();
            var sql = @" UPDATE Films
                         SET Title = @Title, Director = @Director, RelaseDate = @RelaseDate
                         WHERE Id = @Id";
            var result = await db.ExecuteAsync(sql.ToString(), new { film.Title, film.Director, film.RelaseDate, film.Id });
            return result > 0;
        }
    }
}
