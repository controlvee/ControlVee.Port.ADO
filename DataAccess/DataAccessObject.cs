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
            Movie rv = new Movie();
            // Easy but not efficient - does not check for null.
            rv.ID = (int)reader["ID"];
            rv.Title = (string)reader["Title"];
            rv.ReleaseDate = (DateTime)reader["ReleaseDate"];
            rv.Minutes = (int)reader["Minutes"];
            rv.RatingID = (int)reader["RatingID"];

            return rv;
        }

        public List<Movie> GetMovies(int skip, int take)
        {
            List<Movie> rv = new List<Movie>();
            return rv;
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
