using System;
using System.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Coding_Tracker
{
    public class DeleteRecord
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Delete(){
            try{
                using(var connection = new SqliteConnection(connectionString)){
                    connection.Open();

                    ShowRecords showRecs = new ShowRecords();
                    showRecs.Show();

                    AnsiConsole.MarkupLine("[red] WHAT LINE DO YOU WANT TO DELETE? TYPE 0 TO EXIT[/]");
                
                    int id = Convert.ToInt32(Console.ReadLine());

                    if(id == 0){
                        ShowMenu menu = new ShowMenu();
                        menu.Menu();
                    }
                    else{
                        connection.Execute("DELETE FROM coding_tracker WHERE Id = @Id", new { Id= id});
                        Console.Clear();
                        showRecs.Show();
                    }

                    
                }
            }

            catch(Exception e){
                AnsiConsole.MarkupLine($"[yellow] {e.Message} [/]");
            }
        }
    }
}