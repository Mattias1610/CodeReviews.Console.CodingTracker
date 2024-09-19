using System;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Configuration;

namespace Coding_Tracker
{
    public class GetReport
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public string BuildReportQuery(string periodChoice)
        {
            

            string dateCondition = "";

            switch (periodChoice)
            {
                case "1":
                    dateCondition = "WHERE Date = date('now')";
                    break;
                case "2":
                    dateCondition = "WHERE Date >= date('now', '-7 days')";
                    break;
                case "3":
                    dateCondition = "WHERE Date >= date('now', 'start of the month')";
                    break;
                case "4":
                    dateCondition = "WHERE Date >= date('now', 'start of the year')";
                    break;
                case "5":
                    dateCondition = "";
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red] ERROR: Invalid Choice [/]");
                    break;
            }

            return $@"
                SELECT 
                    SUM(strftime('%s', Duration)) AS TotalDurationSeconds, 
                    AVG(strftime('%s', Duration)) AS AverageDurationSeconds
                FROM coding_tracker 
                {dateCondition};";
        }

        public void GenerateReport()
        {
            AnsiConsole.MarkupLine("[bold yellow] Choose period[/]");
            AnsiConsole.MarkupLine("\t 1. TODAY");
            AnsiConsole.MarkupLine("\t 2. THIS WEEK");
            AnsiConsole.MarkupLine("\t 3. THIS MONTH");
            AnsiConsole.MarkupLine("\t 4. THIS YEAR");
            AnsiConsole.MarkupLine("\t 5. ALL TIME");

            string periodChoice = Console.ReadLine();
            string query = BuildReportQuery(periodChoice);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long totalDurationSeconds = reader.GetInt64(0);
                            double averageDurationSeconds = reader.GetDouble(1);

                            TimeSpan totalDuration = TimeSpan.FromSeconds(totalDurationSeconds);
                            TimeSpan averageDuration = TimeSpan.FromSeconds(averageDurationSeconds);

                            AnsiConsole.MarkupLine($"[bold yellow]Total Duration:[/] {totalDuration:hh\\:mm\\:ss}");
                            AnsiConsole.MarkupLine($"[bold yellow]Average Duration:[/] {averageDuration:hh\\:mm\\:ss}");
                        }
                    }
                }
            }
        }
    }
}