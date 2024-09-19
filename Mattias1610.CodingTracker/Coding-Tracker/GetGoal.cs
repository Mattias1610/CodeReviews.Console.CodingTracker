using System;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Configuration;

namespace Coding_Tracker
{
    
    public class GetGoal
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Goal()
        {
            AnsiConsole.MarkupLine("[bold yellow] PLEASE ENTER YOUR CODING GOAL IN HOURS:[/]");
            string goal = Console.ReadLine();

            if (!int.TryParse(goal, out int goalInHours))
            {
                AnsiConsole.MarkupLine("[bold red] INVALID [/]");
                return;
            }

            int goalInMinutes = goalInHours * 60;

            int totalCoding = 0;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand("SELECT Duration FROM coding_tracker", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string durationStr = reader.GetString(0);

                            if (TimeSpan.TryParse(durationStr, out TimeSpan duration))
                            {
                                totalCoding += (int)duration.TotalMinutes;
                            }
                        }
                    }
                }
            }

            int remaining = goalInMinutes - totalCoding;
            TimeSpan remainingTime = TimeSpan.FromMinutes(remaining);

            AnsiConsole.MarkupLine($"[bold yellow]Total Coding Time:[/] {TimeSpan.FromMinutes(totalCoding):hh\\:mm}");

            if (remaining > 0)
            {
                AnsiConsole.MarkupLine($"[bold yellow]You need to code for another:[/] {remainingTime:hh\\:mm} hours to reach your goal.");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold green]Congratulations! You've reached or exceeded your goal![/]");
            }
        }
    }
}