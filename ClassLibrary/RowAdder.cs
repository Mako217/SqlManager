using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ClassLibrary
{
    public static class RowAdder
    {
         public static void Add(string connectionString, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]}");
            SqlDataAdapter adapter = new SqlDataAdapter($"Select Column_Name, Data_Type From INFORMATION_SCHEMA.COLUMNS where Table_Name = '{tableDialog.options[whichTable]}' ", connection);
            connection.Open();
            adapter.Fill(dataTable);
            List<string> columnList = new List<string>();
            string columns = "";
            string values = "";
            for(int i = 0; i<dataTable.Rows.Count; i++)
            {
                Console.Clear();
                DataRow row = dataTable.Rows[i];
                columns += (string)row.ItemArray[0] + ", ";
                Console.WriteLine("Add " + (string)row.ItemArray[0] + " value:");
                if((string)row.ItemArray[1] == "varchar")
                {
                    values += "'" + Console.ReadLine() + "', ";
                }
                else
                {
                    values += Console.ReadLine() + ", ";
                }
            }
            columns = columns.Substring(0, columns.Length - 2);
            values = values.Substring(0, values.Length - 2);
            dataTable.Dispose();
            adapter.Dispose();

            SqlCommand command = new SqlCommand($"INSERT INTO [{tableDialog.options[whichTable]}]({columns}) VALUES({values})", connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
                Console.ReadLine();
            }
            command.Dispose();
            connection.Close();
            connection.Dispose();

            tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);

        }
    }
}