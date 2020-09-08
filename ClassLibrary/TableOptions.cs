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
            Add("--Add new column--");
            Add("--Delete column--");
            Add("--Delete row--");
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
                TableEditor.SelectCell(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 2:
                RowAdder.Add(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 3:
                ColumnAdder.Add(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 4:
                ColumnDeleter.SelectColumn(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 5:
                RowDeleter.SelectRow(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 6:
                TableDeleter.Delete(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 7:
                tableDialog.Start(databaseDialog, whichDatabase, this);
                break;

            }
        }
    }
}