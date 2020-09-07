namespace ClassLibrary
{
    public class TableOptions : Dialog
    {
        public TableOptions(string connectionString, string serverType) : base(connectionString, serverType)
        {
            ServerType = serverType;
            ConnectionString = connectionString;
            Add("--Print Table--");
            Add("--Edit table--");
            Add("--Add new row--");
            Add("--Delete Table--");
            Add("--Return--");
        }

        public void Start(DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable)
        {           
            int option = Control();

            switch(option)
            {
                case 0:
                TablePrinter.Print(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 1:
                TableEditor.Control(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 2:
                RowAdder.Add(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 3:
                TableDeleter.Delete(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 4:
                tableDialog.Start(databaseDialog, whichDatabase, this);
                break;

            }
        }
    }
}