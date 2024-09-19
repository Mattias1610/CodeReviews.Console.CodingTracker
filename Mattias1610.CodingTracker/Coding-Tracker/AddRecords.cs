using System;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Coding_Tracker
{
    public class AddRecords
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public string GetDate()
        {
            while (true)
            {
                AnsiConsole.MarkupLine("Enter the date in this format [bold red]DD-MM-YY[/]:");
                string initialDate = Console.ReadLine();

                if (DateOnly.TryParseExact(initialDate, "dd-MM-yy", out DateOnly date))
                {
                    return date.ToString("yyyy-MM-dd");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid date format. Please try again.[/]");
                }
            }
        }

        public string GetStartTime()
{
    while (true)
    {
        AnsiConsole.MarkupLine("Enter the time you started coding in this format [bold red]HH:mm[/] or [bold red]h:mm tt[/]:");
        string initialTime = Console.ReadLine();

        if (TimeOnly.TryParseExact(initialTime, "HH:mm", out TimeOnly time) || 
            TimeOnly.TryParseExact(initialTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
        {
            return time.ToString("HH:mm"); // Return the time in 24-hour format
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Invalid time format. Please try again.[/]");
        }
    }
}

public string GetEndTime(string startTime)
{
    while (true)
    {
        AnsiConsole.MarkupLine("Enter the time you finished coding in this format [bold red]HH:mm[/] or [bold red]h:mm tt[/]:");
        string endTime = Console.ReadLine();

        if (TimeOnly.TryParseExact(endTime, "HH:mm", out TimeOnly parsedEndTime) || 
            TimeOnly.TryParseExact(endTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndTime))
        {
            var startTimeSpan = TimeSpan.Parse(startTime);
            var endTimeSpan = TimeSpan.Parse(parsedEndTime.ToString("HH:mm")); // Normalize to 24-hour format

            if (endTimeSpan > startTimeSpan)
            {
                return parsedEndTime.ToString("HH:mm"); // Return time in 24-hour format
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]End time cannot be earlier than start time.[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Invalid time format. Please try again.[/]");
        }
    }
}

        public string GetDuration(string startTime, string endTime)
        {
            var startTimeSpan = TimeSpan.Parse(startTime);
            var endTimeSpan = TimeSpan.Parse(endTime);

            TimeSpan timeDifference = endTimeSpan - startTimeSpan;

            return timeDifference.ToString();
        }
        public void GetRecord(){
            try{

                string date = GetDate();
                string start = GetStartTime();
                string end = GetEndTime(start);
                string duration = GetDuration(start, end);

                using(var connection = new SqliteConnection(connectionString)){
                    connection.Open();
                    connection.Execute(@"INSERT INTO coding_tracker (Date, Start, End, Duration) VALUES (@Date, @Start, @End, @Duration)", new { Date = date, Start = start, End = end, Duration = duration });
                    AnsiConsole.MarkupLine("[green] VALUES INSERTED [/]");
                }
            }

            catch(Exception e){
                AnsiConsole.Markup($"[yellow] {e.Message} [/]");
            }
        }
    }
}