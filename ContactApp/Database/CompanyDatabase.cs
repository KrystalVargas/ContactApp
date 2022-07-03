using ContactApp.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ContactApp.Database
{
    class CompanyDatabase : IDatabase
    {
        public String AddNewCompanyText = "Add New Company...";
        private List<CompanyInfoModel> defaultCompanies = new List<CompanyInfoModel> {
            new CompanyInfoModel { Company = "ACME Acids",          VendorCode = "A001" },
            new CompanyInfoModel { Company = "Berenstain Biology",  VendorCode = "A002" },
            new CompanyInfoModel { Company = "Flick's Fluidics",    VendorCode = "A003" },
            new CompanyInfoModel { Company = "Radical Reagents",    VendorCode = "D004" },
            new CompanyInfoModel { Company = "BBST Paper Products", VendorCode = "G065" }
            };

        public void CreateDatabase()
        {
            // Create the connection.
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String queryString = "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name LIKE 'Companies') CREATE TABLE " +
                    "Companies(Company varchar(255) NOT NULL," +
                    "VendorCode varchar(4) NOT NULL)";

                // Create a SqlCommand
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
            List<String> companies = new List<String>();
            // Create the connection.
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String queryString = "SELECT * FROM dbo.Companies";

                // Create a SqlCommand
                using (SqlCommand sqlCommand = new SqlCommand(queryString, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Get Company Names and add to Company List
                                companies.Add(reader.GetString(0));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                        // Add line to select for adding a new company
                        companies.Add(AddNewCompanyText);
                    }
                }
            }
            return companies;
        }


        public void SaveToDatabase(object company)
        {
            CompanyInfoModel companyInfo = company as CompanyInfoModel;
            if (companyInfo != null)
            {
                // Create the connection.
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    String queryString = "INSERT INTO dbo.Companies (Company, VendorCode) VALUES (@Company, @VendorCode)";

                    // Create a SqlCommand
                    using (SqlCommand sqlCommand = new SqlCommand(queryString, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Company", companyInfo.Company);
                        sqlCommand.Parameters.AddWithValue("@VendorCode", companyInfo.VendorCode);

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

        public Boolean CompanyExist(CompanyInfoModel companyInfo)
        {
            int companyCount = 0;
            // Create the connection.
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String queryString = "SELECT COUNT(1) FROM Companies WHERE Company LIKE @Company OR VendorCode LIKE @VendorCode";

                // Create a SqlCommand
                using (SqlCommand sqlCommand = new SqlCommand(queryString, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@Company", companyInfo.Company);
                    sqlCommand.Parameters.AddWithValue("@VendorCode", companyInfo.VendorCode);

                    try
                    {
                        connection.Open();
                        companyCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
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

            if (companyCount > 0)
                return true;
            else
                return false;
        }
        // Fill Company list with master list when first created
        private void AddDefaultCompanies()
        {
            foreach (CompanyInfoModel company in defaultCompanies)
            {
                if (!CompanyExists(company))
                {
                    SaveToDatabase(company);
                }
            }         
        }
    }
}
