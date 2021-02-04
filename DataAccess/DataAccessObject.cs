using System;
using System.Collections.Generic;
using DataObject;

namespace DataAccess
{
    /// <summary>
    /// Open/close check connection "design pattern".
    /// Easy - not efficient pattern used in MapMovie().
    /// </summary>
    public class DataAccessObject
    {
        private System.Data.IDbConnection connection;
        
        public DataAccessObject()
        {

        }

        public DataAccessObject(System.Data.IDbConnection connection)
        {
            this.connection = connection;
        }

        private bool AssuredConnected()
        {
            switch(connection.State)
            {
                case (System.Data.ConnectionState.Closed):
                    connection.Open();
                    return false;

                case (System.Data.ConnectionState.Broken):
                    connection.Close();
                    connection.Open();
                    return false;

                default: return true;
            }
        }

        public Movie MapMovie(System.Data.IDataReader reader)
        {
            Movie movie = new Movie();
            // Easy but not efficient - does not check for null.
            movie.ID = (int)reader["ID"];
            movie.Title = (string)reader["Title"];
            movie.ReleaseDate = (DateTime)reader["Released"];
            movie.Minutes = (int)reader["Minutes"];

            // Proper implentation at next line.
            if (reader.IsDBNull(5))
            {
                movie.RatingID = null; 
            }
            else
            {
                movie.RatingID = (int)reader["RatingID"];
            }
            
            return movie;
        }

        // Study implementation of skip/take.
        public List<Movie> GetMovies(int skip, int take)
        {
            List<Movie> movie = new List<Movie>();

            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                // Demonstrate a text command.
                // Be leary - but valid.
                // Safe with int only.
                // string text = $"select * from movies";
                // command.CommandText = text;
                // command.CommandType = System.Data.CommandType.Text;

                command.CommandText = "[stored proc]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                System.Data.IDataParameter P_skip = command.CreateParameter();
                System.Data.IDataParameter P_take = command.CreateParameter();

                P_skip.Direction = System.Data.ParameterDirection.Input;
                P_skip.ParameterName = "@skip";
                P_skip.DbType = System.Data.DbType.Int32;
                P_skip.Value = skip;

                P_take.Direction = System.Data.ParameterDirection.Input;
                P_take.ParameterName = "@take";
                P_take.DbType = System.Data.DbType.Int32;
                P_take.Value = take;

                command.Parameters.Add(P_skip);
                command.Parameters.Add(P_take);

                // Study the implementation.
                using (System.Data.IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movie.Add(MapMovie(reader));
                    }
                }
            }
            
            return movie;
        }

        public Movie GetMoviByID(int id)
        {
            AssuredConnected();
            using(System.Data.IDbCommand command = connection.CreateCommand())
            {
                // Demonstrate a text command.
                // Be leary - but valid.
                // Safe with int only.
                string text = $"select * from movies where ID = {id}";
                command.CommandText = text;
                command.CommandType = System.Data.CommandType.Text;

                // Study the implementation.
                System.Data.IDataReader reader = command.ExecuteReader();
                if(!reader.Read())
                {
                    return null;
                }

                Movie m = MapMovie(reader);
                if(reader.Read())
                {
                    throw new Exception($"Found more than one matching record with ID {id}.");
                }

                return m;
            }
        }
    }
}
