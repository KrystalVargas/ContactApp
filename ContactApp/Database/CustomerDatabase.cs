using Microsoft.Data.SqlClient;
using System;
using System.Windows;
using System.Data;
using ContactApp.Model;

namespace ContactApp.Database
{
    class CustomerDatabase : IDatabase
    {
        public void CreateDatabase()
        {
            // Create the connection.
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String queryString = "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name LIKE 'Customers') CREATE TABLE " +
                    "Customers(Name varchar(255) NOT NULL," +
                    "Company varchar(255) NOT NULL," +
                    "PhoneNumber varchar(255) NOT NULL," +
                    "Address varchar(255) NOT NULL," +
                    "Notes varchar(MAX) NULL)";

                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand sqlCommand = new SqlCommand(queryString, connection))
                {
                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public object ReadFromDatabase()
        {
            DataTable dt = new DataTable();

            // Create the connection.
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String queryString = "SELECT * FROM dbo.Customers";

                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand sqlCommand = new SqlCommand(queryString, connection))
                {
                    try
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand))
                        {                            
                            dataAdapter.Fill(dt);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
            return dt;
        }

        public void SaveToDatabase(object customer)
        {
            CustomerInfoModel customerInfo = customer as CustomerInfoModel;
            if (customerInfo != null)
            {
                // Create the connection
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    String queryString = "INSERT INTO dbo.Customers (Name, Company, PhoneNumber, Address, Notes) VALUES (@Name, @Company, @PhoneNumber, @Address, @Notes)";

                    // Create a SqlCommand, and identify it as a stored procedure.
                    using (SqlCommand sqlCommand = new SqlCommand(queryString, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Name", customerInfo.Name);
                        sqlCommand.Parameters.AddWithValue("@Company", customerInfo.Company);
                        sqlCommand.Parameters.AddWithValue("@PhoneNumber", customerInfo.PhoneNumber);
                        sqlCommand.Parameters.AddWithValue("@Address", customerInfo.Address);

                        if (String.IsNullOrEmpty(customerInfo.Notes))
                        {
                            sqlCommand.Parameters.AddWithValue("@Notes", DBNull.Value);
                        }
                        else
                            sqlCommand.Parameters.AddWithValue("@Notes", customerInfo.Notes);

                        try
                        {
                            connection.Open();
                            sqlCommand.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }
    }
}
