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

            
            //Pass your connection string without Initial Catalog/Database
            string ConnectionString = "Data Source = Michał; Integrated Security = True;";


            string serverType = "";
            if(ConnectionString.Contains("Data Source") && ConnectionString.Contains("Integrated Security"))
            {
                serverType = "MSSQL Server";
            }
            else if(ConnectionString.Contains("Server") && ConnectionString.Contains("Port") && ConnectionString.Contains("Uid") && ConnectionString.Contains("Pwd"))
            {
                serverType = "PostgreSQL";
            }


            DatabaseDialog databaseDialog = new DatabaseDialog(ConnectionString, serverType);
            TableDialog tableDialog = new TableDialog(ConnectionString, serverType);
            TableOptions tableOptions = new TableOptions(ConnectionString, serverType);
            databaseDialog.Start(tableDialog, tableOptions);

        }
    }
}
