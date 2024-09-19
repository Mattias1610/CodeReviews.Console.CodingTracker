using System;
using System.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Diagnostics;

namespace Coding_Tracker
{
    public class StopwatchManager
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        
        public TimeSpan GetStopwatchTime()
        {
            Stopwatch stopwatch = new Stopwatch();
            TimeSpan time = TimeSpan.Zero;
            bool isRunning = true;

            while (isRunning)
            {
                AnsiConsole.MarkupLine("[green]TYPE 1 TO START THE STOPWATCH[/]");
                AnsiConsole.MarkupLine("[green]TYPE ANYTHING TO STOP IT AFTER IT STARTS AND TYPE 0 TO CANCEL[/]");

                string command = Console.ReadLine();

                if (command == "1")
                {
                    stopwatch.Start();
                    AnsiConsole.MarkupLine("[yellow]Stopwatch started...[/]");
                    AnsiConsole.MarkupLine("[yellow]Type anything to stop it, or type 0 to cancel[/]");

                    string choice = Console.ReadLine();

                    if (choice == "0")
                    {
                        stopwatch.Reset();
                        time = stopwatch.Elapsed;
                        AnsiConsole.MarkupLine("[red]Stopwatch cancelled.[/]");
                        return time;
                    }
                    else
                    {
                        stopwatch.Stop();
                        time = stopwatch.Elapsed;
                        AnsiConsole.MarkupLine($"[green]Stopwatch stopped. Elapsed time: {time}[/]");
                        return time;
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Stopwatch was not started. Exiting...[/]");
                    isRunning = false;
                }
            }

            return time;
        }

        public DateOnly GetDate()
        {
            var Date = DateOnly.FromDateTime(DateTime.Now);
            return Date;
        }

        public void InsertTime(){
            try{
                var date = GetDate();
                var startTime = GetStopwatchTime().ToString(@"hh\:mm\:ss"); // Stopwatch time as start time
                var endTime = GetStopwatchTime().ToString(@"hh\:mm\:ss");   // Assuming you want to use stopwatch again for end time
                var duration = (TimeSpan.Parse(endTime) - TimeSpan.Parse(startTime)).ToString(@"hh\:mm\:ss");

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute(@"INSERT INTO coding_tracker (Date, Start, End, Duration) 
                                        VALUES (@Date, @Start, @End, @Duration)",
                                        new { Date = date.ToString("yyyy-MM-dd"), Start = startTime, End = endTime, Duration = duration });
                    AnsiConsole.MarkupLine("[green]Record inserted successfully![/]");
                }
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
            }
        }

    }
}