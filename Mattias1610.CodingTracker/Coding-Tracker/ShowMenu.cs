using Spectre.Console;

namespace Coding_Tracker
{
    public class ShowMenu
    {
        public void Menu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                AnsiConsole.Markup("[yellow]---------[/] WELCOME TO MY CODING TRACKER [yellow]---------[/] \n");
                AnsiConsole.Markup($"\t TYPE 1 TO ADD NEW RECORD \n");
                AnsiConsole.Markup($"\t TYPE 2 TO DELETE A RECORD \n");
                AnsiConsole.Markup($"\t TYPE 3 TO UPDATE A RECORD \n");
                AnsiConsole.Markup($"\t TYPE 4 TO SEE THE RECORDS \n");
                AnsiConsole.Markup($"\t TYPE 5 TO USE A STOPWATCH \n");
                AnsiConsole.Markup($"\t TYPE 6 TO ORDER THE RECORDS \n");
                AnsiConsole.Markup($"\t TYPE 7 TO SET A GOAL \n");
                AnsiConsole.Markup($"\t TYPE 8 TO GENERATE A REPORT \n");
                AnsiConsole.Markup($"\t TYPE 0 TO EXIT \n");

                string command = Console.ReadLine();

                switch (command)
                {
                     case "1":
                         AddRecords add = new AddRecords();
                         add.GetRecord();
                         break;
                    case "2":
                        DeleteRecord delete = new DeleteRecord();
                        delete.Delete();
                        break;
                    case "3":
                        UpdateRecord upd = new UpdateRecord();
                        upd.Update();
                        break;
                    case "4":
                        ShowRecords showRecs = new ShowRecords();
                        showRecs.Show();
                        break;
                    case "5":
                        StopwatchManager stopwatch = new StopwatchManager();
                        stopwatch.InsertTime();
                        break;
                    case "6":
                        SortTable sort = new SortTable();
                        sort.Sorted();
                        break;
                    case "7":
                        GetGoal goal = new GetGoal();
                        goal.Goal();
                        break;
                    case "8":
                        GetReport report = new GetReport();
                        report.GenerateReport();
                        break;
                     case "0":
                         isRunning = false;
                         Environment.Exit(0);
                         break;
                     default:
                        AnsiConsole.Markup("[bold red] ERROR [/]\n");
                        break;
                }
            }
        }
    }
}