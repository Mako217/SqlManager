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
            SqlDataAdapter adapter = new SqlDataAdapter($"Select Column_Name From INFORMATION_SCHEMA.COLUMNS where Table_Name = '{tableDialog.options[whichTable]}' ", connection);
            connection.Open();
            adapter.Fill(dataTable);
            List<string> columnList = new List<string>();
            List<string> values = new List<string>();
            string columns = "";
            string columnsAt = "";
            for(int i = 0; i<dataTable.Rows.Count; i++)
            {
                Console.Clear();
                DataRow row = dataTable.Rows[i];
                columns += (string)row.ItemArray[0] + ", ";
                columnsAt += "@" + (string)row.ItemArray[0] + ", ";
                columnList.Add("@" + (string)row.ItemArray[0]);
                Console.WriteLine("Add " + (string)row.ItemArray[0] + " value:");
                values.Add(Console.ReadLine());
            }
            columns = columns.Substring(0, columns.Length - 2);
            columnsAt = columnsAt.Substring(0, columnsAt.Length - 2);
            dataTable.Dispose();
            adapter.Dispose();

            SqlCommand command = new SqlCommand($"INSERT INTO [{tableDialog.options[whichTable]}]({columns}) VALUES({columnsAt})", connection);
            for(int i = 0; i<columnList.Count; i++)
            {
                command.Parameters.AddWithValue(columnList[i], values[i]);
            }
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Error");
                Console.ReadLine();
            }
            command.Dispose();
            connection.Close();
            connection.Dispose();

            tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);

        }
    }
}