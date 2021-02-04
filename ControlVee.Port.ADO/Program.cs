using System;
using DataAccess;
using DataObject;
using System.Data;

namespace ControlVee.Port.ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new System.Data.SqlClient.SqlConnection();

            //  Not yet implemented past this line.
            connection.ConnectionString = @"Data Source=(db)\mssqllocaldb;Initial Catalog=[table];Integrated Security=True";

            DataAccessObject context = new DataAccessObject(connection);

            Movie aMovie = context.GetMoviByID(10);

            // No implemntation of params skip/take.
            var movies = context.GetMovies(0, 100);
        }
    }
}
