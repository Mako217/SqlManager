using System;
using ClassLibrary;

namespace SqlManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;

            //Pass your connection string without Initial Catalog
            string ConnectionString = "Data Source = (Your data source); Integrated Security = True;";

            DatabaseDialog databaseDialog = new DatabaseDialog(ConnectionString);
            TableDialog tableDialog = new TableDialog(ConnectionString);
            TableOptions tableOptions = new TableOptions(ConnectionString);

            databaseDialog.Start(tableDialog, tableOptions);

        }
    }
}
