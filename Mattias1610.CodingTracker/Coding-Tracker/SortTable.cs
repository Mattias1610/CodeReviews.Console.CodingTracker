using System;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Configuration;

namespace Coding_Tracker
{
    public class SortTable
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Sorted()
        {
            AnsiConsole.MarkupLine("[bold yellow] Choose period[/]");
            AnsiConsole.MarkupLine("\t 1. TODAY");
            AnsiConsole.MarkupLine("\t 2. THIS WEEK");
            AnsiConsole.MarkupLine("\t 3. THIS MONTH");
            AnsiConsole.MarkupLine("\t 4. THIS YEAR");
            AnsiConsole.MarkupLine("\t 5. ALL TIME");

            string periodChoice = Console.ReadLine();

            if (periodChoice != "1" && periodChoice != "2" && periodChoice != "3" && periodChoice != "4" && periodChoice != "5")
            {
                AnsiConsole.MarkupLine("[bold red] Invalid period choice! [/]");
                return;
            }

            AnsiConsole.MarkupLine("[bold yellow] Choose order[/]");
            AnsiConsole.MarkupLine("\t 1. ASCENDING");
            AnsiConsole.MarkupLine("\t 2. DESCENDING");

            string orderChoice = Console.ReadLine();

            if (orderChoice != "1" && orderChoice != "2")
            {
                AnsiConsole.MarkupLine("[bold red] Invalid order choice! [/]");
                return;
            }

            string query = BuildFilterAndOrderQuery(periodChoice, orderChoice);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        AnsiConsole.MarkupLine("[bold yellow] Results:[/]");
                        while (reader.Read())
                        {
                            int Id = reader.GetInt32(0);
                            string date = reader.GetString(1);
                            string duration = reader.GetString(2);

                            AnsiConsole.MarkupLine($"[bold yellow]Id:[/] {Id}, [bold yellow]Date:[/] {date}, [bold yellow]Duration:[/] {duration}");
                        }
                    }
                }
            }
        }

        public string BuildFilterAndOrderQuery(string periodChoice, string orderChoice){

            string dateCondition = "";
            string orderBy = orderChoice == "1" ? "ASC" : "DESC";

            switch (periodChoice)
            {
                case "1":
                    dateCondition = "WHERE Date = date('now')";
                    break;
                case "2":
                    dateCondition = "WHERE Date >= date('now', '-7 days')";
                    break;
                case "3":
                    dateCondition = "WHERE Date >= date('now', 'start of month')";
                    break;
                case "4":
                    dateCondition = "WHERE Date >= date('now', 'start of year')";
                    break;
                case "5":
                    dateCondition = "";
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red] ERROR [/]");
                    break;
            }

            
            return $"SELECT * FROM coding_tracker {dateCondition} ORDER BY date(Date) {orderBy}, Id {orderBy}";
        }

    }
}
