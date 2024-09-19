using Spectre.Console;
using System;
using System.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Coding_Tracker
{
    public class ShowRecords
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Show(){

            try{
                using(var connection = new SqliteConnection(connectionString)){
                    connection.Open();
                    var table = connection.Query("SELECT * FROM coding_tracker");

                    foreach (var dw in table){
                        AnsiConsole.MarkupLine($"ID: {dw.Id} \t Date: {dw.Date} \t Start:{dw.Start} \t End:{dw.End} \t Duration: {dw.Duration}");
                    }
                }
            }

            catch(Exception e){
                AnsiConsole.MarkupLine(e.Message);
            }
        }
    }
}