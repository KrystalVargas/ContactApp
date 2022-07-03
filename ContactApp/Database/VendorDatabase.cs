using ContactApp.Model;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace ContactApp.Database
{
    class VendorDatabase : IDatabase
    {
        public void CreateDatabase()
        {
            // Create the connection.
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String queryString = "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name LIKE 'Vendors') CREATE TABLE " +
                    "Vendors(Name varchar(255) NOT NULL," +
                    "Company varchar(255) NOT NULL," +
                    "PhoneNumber varchar(255) NOT NULL," +
                    "Address varchar(255) NOT NULL)";

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
                String queryString = "SELECT * FROM dbo.Vendors";

                // Create a SqlCommand
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

        public void SaveToDatabase(object vendor)
        {
            VendorInfoModel vendorInfo = vendor as VendorInfoModel;
            if (vendorInfo != null)
            {
                // Create the connection.
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    String queryString = "INSERT INTO dbo.Vendors (Name, Company, PhoneNumber, Address) VALUES (@Name, @Company, @PhoneNumber, @Address)";

                    // Create a SqlCommand
                    using (SqlCommand sqlCommand = new SqlCommand(queryString, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Name", vendorInfo.Name);
                        sqlCommand.Parameters.AddWithValue("@Company", vendorInfo.Company);
                        sqlCommand.Parameters.AddWithValue("@PhoneNumber", vendorInfo.PhoneNumber);
                        sqlCommand.Parameters.AddWithValue("@Address", vendorInfo.Address);

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
