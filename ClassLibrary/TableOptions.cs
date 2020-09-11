namespace ClassLibrary
{
    public class TableOptions : Dialog
    {
        public TableOptions(string connectionString, string serverType) : base(connectionString, serverType)
        {
            ServerType = serverType;
            ConnectionString = connectionString;
            //Since the options list for table won't change, we dont need UpdateList(), and we create all the options in the Constructor
            Add("--Print Table--");
            Add("--Search in table--");
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
            //Start the Control() method, and set returned value as option          
            int option = Control();

            switch(option)
            {
                case 0:
                //If --Print Table-- is selected print the table
                TablePrinter.Print(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 1:
                //If --Search in table-- is selected, print table only with content matching the search
                TableSearcher.Search(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 2:
                //If --Edit table-- is selected let user select the cell to edit 
                TableEditor.SelectCell(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 3:
                //If --Add new row-- is selected start adding new row
                RowAdder.Add(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 4:
                //If --Add new column-- is selected start adding new column
                ColumnAdder.Add(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 5:
                //If --Delete column-- is selected let user select the column to delete
                ColumnDeleter.SelectColumn(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 6:
                //If --Delete row-- is selected let user select the row to delete
                RowDeleter.SelectRow(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 7:
                //If --Delete table-- is selected start deleting currently selected table
                TableDeleter.Delete(ConnectionString, ServerType, databaseDialog, whichDatabase, tableDialog, whichTable, this);
                break;
                case 8:
                //If --Return-- is selected return to the tableDialog
                tableDialog.Start(databaseDialog, whichDatabase, this);
                break;

            }
        }
    }
}