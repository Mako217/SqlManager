using System;
using ClassLibrary;

namespace SqlManager
{
    class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString = "Data Source = MICHAŁ; Integrated Security = True;";

            DatabaseDialog databaseDialog = new DatabaseDialog(ConnectionString);
            TableDialog tableDialog = new TableDialog(ConnectionString);
            TableOptions tableOptions = new TableOptions(ConnectionString);

            databaseDialog.Start(tableDialog, tableOptions);

        }
    }
}
