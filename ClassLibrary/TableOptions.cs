namespace ClassLibrary
{
    public class TableOptions : Dialog
    {
        public TableOptions(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
            Add("--Print Table--");
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
                TablePrinter.Print(ConnectionString, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 1:
                RowAdder.Add(ConnectionString, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 2:
                TableDeleter.Delete(ConnectionString, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 3:
                tableDialog.Start(databaseDialog, whichDatabase, this);
                break;

            }
        }
    }
}