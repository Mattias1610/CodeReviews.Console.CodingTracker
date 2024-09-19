using System;
using Microsoft.Data.Sqlite;
using System.Globalization;
using Spectre.Console;
using System.Data.Common;
using System.Data.SQLite;   
using System.Configuration;
using Coding_Tracker;


namespace CodingTracker{
    class Program{
        static void Main(string[] args){
            Console.Clear();
            ShowMenu menu = new ShowMenu();
            TableManager table = new TableManager();

            table.CreateTable();
            menu.Menu();
            Console.ReadLine();
            
        }
    }
}