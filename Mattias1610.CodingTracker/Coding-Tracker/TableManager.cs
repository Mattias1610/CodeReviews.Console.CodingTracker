using System;
using System.Configuration;
using Microsoft.Data.Sqlite;

namespace Coding_Tracker
{
    public class TableManager
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void CreateTable()
        {

            try{
                using (var connection = new SqliteConnection(connectionString))
                {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @$"
                    CREATE TABLE IF NOT EXISTS coding_tracker(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Start TEXT,
                    End TEXT,
                    Duration TEXT)";

                tableCmd.ExecuteNonQuery();
                connection.Close();
                }
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
