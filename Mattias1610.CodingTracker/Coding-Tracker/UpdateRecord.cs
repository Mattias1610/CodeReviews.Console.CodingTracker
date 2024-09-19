using System;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;


namespace Coding_Tracker
{
    public class UpdateRecord
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
        public void Update()
        {
            AnsiConsole.MarkupLine("[bold yellow] Enter the id of the record to update:[/]");
            int id = Convert.ToInt32(Console.ReadLine());

            string date = GetDate();
            string startTime = GetStartTime();
            string endTime = GetEndTime(startTime);
            string duration = GetDuration(startTime, endTime);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                connection.Execute("UPDATE coding_tracker SET Date = @Date, Start = @Start, End = @End, Duration = @Duration WHERE Id = @Id", new { Id = id, Date = date, Start = startTime, End = endTime, Duration = duration });
                AnsiConsole.MarkupLine("[bold yellow]Record updated successfully![/]");
            }
        }
    }
}