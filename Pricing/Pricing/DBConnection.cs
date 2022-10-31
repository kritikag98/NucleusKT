using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pricing
{
    internal class DBConnection
    {
        //List for adding the changes made in a file
        List<Changes> list = new List<Changes>();

        //Publish Database
        string publishDb = "nucleus_prices_homelane_sandbox_published";

        //Live DataBase
        string liveDb = "nucleus_prices_homelane_sandbox_unpublished";

        //Path to the report
        public string path = @"C:\Users\kritika.g\Documents\Report1.csv";

        //List of column values to be ignored while adding changes to the report
        List<string> ignoredColumns = new List<string> { "ModifiedById", "CreatedDate", "CreatedById", "CreatedByID", "ModifiedByID" };

            string[] uquery =
                    {
                 String.Format("SELECT CONCAT(PVID, RID, PRID, CSubTID, CMID, CityID) AS PKEY," +
                "{0}.corematerialmarkups.* " +
                "FROM {0}.corematerialmarkups", "nucleus_prices_homelane_sandbox_unpublished"),

                 String.Format("SELECT CONCAT(PVID, CMRID) AS PKEY," +
                "{0}.corematerialprices.* " +
                "FROM {0}.corematerialprices WHERE", "nucleus_prices_homelane_sandbox_unpublished"),

                 String.Format("SELECT CONCAT(PVID, EBID) AS PKEY," +
                "{0}.edgebandprices.* " +
                "FROM {0}.edgebandprices", "nucleus_prices_homelane_sandbox_unpublished"),

                 String.Format("SELECT CONCAT(PVID, GID) AS PKEY," +
                "{0}.glassprices.* " +
                "FROM {0}.glassprices", "nucleus_prices_homelane_sandbox_unpublished"),

                 String.Format("SELECT CONCAT(PVID, HandleID) AS PKEY," +
                "{0}.handleprices.* " +
                "FROM {0}.handleprices", "nucleus_prices_homelane_sandbox_unpublished"),

                 String.Format("SELECT CONCAT(PVID, HID) AS PKEY," +
                "{0}.hardwareprices.* " +
                "FROM {0}.hardwareprices", "nucleus_prices_homelane_sandbox_unpublished"),

                 String.Format("SELECT CONCAT(PVID, LegID) AS PKEY," +
                "{0}.legprices.* " +
                "FROM {0}.legprices", "nucleus_prices_homelane_sandbox_unpublished"),

        String.Format("SELECT PVID" +
       " FROM {0}.priceversions", "nucleus_prices_homelane_sandbox_unpublished"),

        String.Format("SELECT CONCAT(PVID, RID, PRID, CSubTID, FTSID, DesignID, CityID) AS PKEY," +
       "{0}.variantmarkups.* " +
       "FROM {0}.variantmarkups", "nucleus_prices_homelane_sandbox_unpublished"),

        String.Format("SELECT CONCAT(PVID, FTSID) AS PKEY," +
       "{0}.variantprices.* " +
       "FROM {0}.variantprices", "nucleus_prices_homelane_sandbox_unpublished"),

        String.Format("SELECT CONCAT(PVID, Brand, Category, Lifestyle, CMID, CityID) AS PKEY," +
       "{0}.wismarkups.* " +
       "FROM {0}.wismarkups", "nucleus_prices_homelane_sandbox_unpublished")
        };

        string[] pquery =
       {
                 String.Format("SELECT CONCAT(PVID, RID, PRID, CSubTID, CMID, CityID) AS PKEY," +
                "{0}.corematerialmarkups.* " +
                "FROM {0}.corematerialmarkups", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT CONCAT(PVID, CMRID) AS PKEY," +
                "{0}.corematerialprices.* " +
                "FROM {0}.corematerialprices", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT CONCAT(PVID, EBID) AS PKEY," +
                "{0}.edgebandprices.* " +
                "FROM {0}.edgebandprices", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT CONCAT(PVID, GID) AS PKEY," +
                "{0}.glassprices.* " +
                "FROM {0}.glassprices", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT CONCAT(PVID, HandleID) AS PKEY," +
                "{0}.handleprices.* " +
                "FROM {0}.handleprices", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT CONCAT(PVID, HID) AS PKEY," +
                "{0}.hardwareprices.* " +
                "FROM {0}.hardwareprices", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT CONCAT(PVID, LegID) AS PKEY," +
                "{0}.legprices.* " +
                "FROM {0}.legprices", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT PVID" +
                " FROM {0}.priceversions", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT CONCAT(PVID, RID, PRID, CSubTID, FTSID, DesignID, CityID) AS PKEY," +
                "{0}.variantmarkups.* " +
                "FROM {0}.variantmarkups", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT CONCAT(PVID, FTSID) AS PKEY," +
       "{0}.variantprices.* " +
       "FROM {0}.variantprices", "nucleus_prices_homelane_sandbox_published"),

                 String.Format("SELECT CONCAT(PVID,  Brand, Category, Lifestyle, CMID, CityID) AS PKEY," +
                "{0}.wismarkups.* " +
                "FROM {0}.wismarkups", "nucleus_prices_homelane_sandbox_published")
                 };

        //Variables for establishing a connection to the connection string
        private string server;
        private string database;
        private string userId;
        private string password;
        private string connectionString;

        //Establish connection string for a particular database
        private string ConnectionString(string databaseName)
        {
            server = "localhost";
            database = databaseName;
            userId = "root";
            password = "homelane@123";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + userId + ";" + "PASSWORD=" + password + ";";
            return connectionString;
        }

        //Function for reading a datatable for a given query
        public DataTable Read(string sSQLQuery, string connection)
        {

            DataTable dtResult = new DataTable();

            using (MySqlConnection myConnection = new MySqlConnection(connection))
            {
                using (MySqlCommand myCommand = new MySqlCommand(sSQLQuery, myConnection))
                {
                    using (MySqlDataAdapter myAdapter = new MySqlDataAdapter(myCommand))
                    {
                        myConnection.Open();
                        myAdapter.Fill(dtResult);
                    }
                }
            }

            return dtResult;
        }

        //Function for returning the publish datatable
        public DataTable PublishTableRead(string q)
        {
            DataTable publishDt = Read(q, ConnectionString(publishDb));
            return publishDt;
        }

        int i = 0;
        int j = 0;
        //Function for reading and returning a live table
        public DataTable LiveTableRead(string q)
        {
                DataTable dt  = Read(q, ConnectionString(liveDb));
                return dt;
        }

        //Method to add headers
        public void Add()
        {
            //To write the headers
            using (StreamWriter sr = File.AppendText(path))
            {
                sr.WriteLine("{0},{1},{2},{3},{4},{5},{6}",
                "TableName", "Column Identifier", "Identifier Value", "Column Name", "Old Data", "New Data", "Status");
            }
        }

        //Comparing the publish and live table and writing the results in the file
        public void Comparison(string tablename)
        {

            if (i < 11 && j<11)
            {
                //live datatable
                DataTable liveDt = LiveTableRead(uquery[i]);
                i = i + 1;


                //publish datatable
                DataTable publishDt = PublishTableRead(pquery[j]);
                j = j + 1;

                //Check if the publish table is empty or not
                if (liveDt != null && liveDt.Rows.Count > 0)
                {
                    //Primary Key column name
                    string primaryKey = liveDt.Columns[0].ColumnName;

                    //Iterating over every row in liveDt
                    foreach (DataRow lrow in liveDt.Rows)
                    {
                        //Selecting the row from publishDt where its primary key column value is equal to that in publishDt
                        DataRow prow = publishDt.Select(String.Format("{0} = '{1}'", primaryKey, lrow[primaryKey])).FirstOrDefault();

                        //If that row is not null
                        if (prow != null)
                        {
                            //Iterate over every column in the liveDt
                            foreach (DataColumn lcolumn in liveDt.Columns)
                            {
                                // Test case 1: Existing column modified
                                if (publishDt.Columns.Contains(lcolumn.ColumnName))
                                {
                                    //Check whether the publishDt column value is equal to the liveDt column value or not
                                    if (!Equals(prow[lcolumn.ColumnName].ToString(), lrow[lcolumn.ColumnName].ToString()))
                                    {
                                        //Ignore the column values which are present in ignoredcolumns list
                                        if (!ignoredColumns.Contains(lcolumn.ColumnName))
                                        {
                                            //Create a new object for the class Changes and add it to the list
                                            list.Add
                                                (new Changes
                                                (tablename,
                                                "Column Modified",
                                                primaryKey,
                                                (lcolumn.ColumnName != null) ? lcolumn.ColumnName : "NA",
                                                (prow[lcolumn.ColumnName] != DBNull.Value) ? prow[lcolumn.ColumnName].ToString() : "NA",
                                                (lrow[lcolumn.ColumnName] != DBNull.Value) ? lrow[lcolumn.ColumnName].ToString() : "NA",
                                                (lrow[primaryKey] != DBNull.Value) ? lrow[primaryKey].ToString() : "NA"));
                                        }
                                    }

                                }

                            }
                        }
                        else
                        {
                            //Test case 3: New row is added
                            list.Add
                            (new Changes
                                 (tablename,
                                 "Row Added",
                                 primaryKey,
                                 (primaryKey != null) ? primaryKey : "NA",
                                 "NA",
                                 (lrow[primaryKey].ToString() != null) ? lrow[primaryKey].ToString() : "NA",
                                 (lrow[primaryKey].ToString() != null) ? lrow[primaryKey].ToString() : "NA")
                            );

                        }
                    }


                    //using the StreamWriter to write the details in the file
                    using (StreamWriter sr = File.AppendText(path))
                    {
                        foreach (var item in list)
                        {
                            sr.WriteLine("{0},{1},{2},{3},{4},{5},{6}",
                                (tablename != null) ? tablename : "NA",
                                (primaryKey != null) ? primaryKey.ToString() : "NA",
                                (item.row_data != null) ? item.row_data : "NA",
                                (item.column_name != null) ? item.column_name : "NA",
                                (item.old_data != null) ? item.old_data : "NA",
                                (item.new_data != null) ? item.new_data : "NA",
                                (item.status != null) ? item.status : "NA");
                        }
                    }
                }

                //Clear the list
                list.Clear();
            }
            }

            //Function for deleting the file if it already exists
            public void FileDelete()
            {
                //Deleting the file for a given path
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
