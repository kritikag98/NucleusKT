using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Code;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

namespace ConsoleApp3
{

    internal class DBConnection
    {
        //List for adding the changes made in a file
        List<Changes> list = new List<Changes>();

        //Path to the report
        public string path = @"C:\Users\kritika.g\Documents\Report.csv";

        //List of column values to be ignored while adding changes to the report
        static string[] ignoredVals = {"ModifiedById", "ModifiedDate", "CreatedDate", "CreatedById", "CreatedByID", "ModifiedByID"};
        List<string> ignoredColumns = new List<string>(ignoredVals);


        //Variables for establishing a connection to the connection string
        private MySqlConnection connection;
        private string server;
        private string database;
        private string userId;
        private string password;
        private string connectionString;


        //Establish a connection string for the publish database
        private string PublishString()
        {
            server = "localhost";
            database = "nucleus_config_homelane_sandbox_published";
            userId = "root";
            password = "homelane@123";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + userId + ";" + "PASSWORD=" + password + ";";
            return connectionString;
        }

        //Establishing a connection for the live database
        private string LiveString()
        {
            server = "localhost";
            database = "nucleus_config_homelane_sandbox_unpublished";
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
        
        //Function for getting the last publish date
        public string LastPublishDate()
        {
            string query = "select PublishDate from publish ORDER BY PublishDate DESC LIMIT 1";
            DataTable lastPublishDt = Read(query, PublishString());
            if (lastPublishDt != null && lastPublishDt.Rows.Count > 0)
            {
                return lastPublishDt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        //Function for reading and returning a live table
        public DataTable LiveTableRead(string tablename)
        {
            string lastPublishDate = Convert.ToDateTime(LastPublishDate()).ToString("yyyy-MM-dd hh:mm:ss");
            string query = String.Format("select * from {0} where ModifiedDate > '{1}'", tablename, lastPublishDate);
            DataTable liveDt = Read(query, LiveString());
            return liveDt;
        }

        //Function for returning the publish datatable
        public DataTable PublishTableRead(string tablename)
        {
            string query = String.Format("select * from {0}", tablename);
            DataTable publishDt = Read(query, PublishString());
            return publishDt;
        }

        //Function for returning the name of the person who modified the details using his userId
        public string ModifiedBy(string usrId)
        {
            string query = String.Format("select Fullname from users where UserID = {0}", usrId);
            DataTable modifiedBy = Read(query, PublishString());
            return modifiedBy.Rows[0][0].ToString();
        }

        //Comparing the publish and live table and writing the results in the file
        public void Comparison(string tablename, string column_identifiers)
        {

            //live datatable
            DataTable liveDt = LiveTableRead(tablename);

            //publish datatable
            DataTable publishDt = PublishTableRead(tablename);

            //Check if the publish table is empty or not
            if (liveDt != null && liveDt.Rows.Count > 0)
            {
                //Primary Key column name
                string primaryKey = liveDt.Columns[0].ColumnName;

                //Iterating over every row in liveDt
                foreach (DataRow lrow in liveDt.Rows)
                {
                    //Getting the name of the person who modified the details using the id
                    string name = ModifiedBy(lrow["ModifiedById"].ToString());

                    //Selecting the row from publishDt where its primary key column value is equal to that in publishDt
                    DataRow prow = publishDt.Select(String.Format("{0} = {1}", primaryKey, lrow[primaryKey])).FirstOrDefault();

                    //If that row is not null
                        if(prow != null) 
                        {
                            
                        //Iterate over every column in the liveDt
                            foreach (DataColumn lcolumn in liveDt.Columns)
                            {
                                // Test case 1: Existing column modified
                                //If the publishDt contains a particular column present in the liveDt
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
                                                (lcolumn.ColumnName != null) ? lcolumn.ColumnName : "NA",
                                                (name != null) ? name : "NA",
                                                (lrow["ModifiedDate"] != DBNull.Value) ? lrow["ModifiedDate"].ToString() : "NA",
                                                (prow[lcolumn.ColumnName] != DBNull.Value) ? prow[lcolumn.ColumnName].ToString() : "NA",
                                                (lrow[lcolumn.ColumnName] != DBNull.Value) ? lrow[lcolumn.ColumnName].ToString() : "NA",
                                                (prow[column_identifiers] != DBNull.Value) ? prow[column_identifiers].ToString() : "NA"));
                                        }
                                    }

                                }

                                //Test case 2: New column added
                                else
                                {
                                    //If the publishDt does not contain a particular column present in liveDt
                                    if (!publishDt.Columns.Contains(lcolumn.ColumnName))
                                    {
                                    //Ignore the column values which are present in ignoredcolumns list
                                    if (!ignoredColumns.Contains(lcolumn.ColumnName))
                                    {
                                            //if the column value is not null
                                            if (lrow[lcolumn.ColumnName] != DBNull.Value)
                                            {
                                            //Create a new object for the class Changes and add it to the list
                                            list.Add
                                                (new Changes
                                                (tablename,
                                                (lcolumn.ColumnName != null) ? lcolumn.ColumnName : "NA",
                                                (name != null) ? name : "NA",
                                                (lrow["ModifiedDate"] != DBNull.Value) ? lrow["ModifiedDate"].ToString() : "NA",
                                                "NA",
                                                (lrow[lcolumn.ColumnName] != DBNull.Value) ? lrow[lcolumn.ColumnName].ToString() : "NA",
                                                (prow[column_identifiers] != DBNull.Value) ? prow[column_identifiers].ToString() : "NA"));
                                            }

                                        }
                                    }

                                    //Test case 3: Existing column deleted
                                    //Iterate over every column in the publishDt
                                    foreach (DataColumn pcolumn in publishDt.Columns)
                                    {
                                    //If the liveDt does not contain that particular column
                                        if (!liveDt.Columns.Contains(pcolumn.ColumnName))
                                        {
                                        //Ignore the column values which are present in ignoredcolumns list
                                        if (!ignoredColumns.Contains(lcolumn.ColumnName))
                                        {
                                            //Create a new object for the list 
                                                list.Add
                                                (new Changes
                                                (tablename,
                                                (pcolumn.ColumnName != null) ? pcolumn.ColumnName : "NA",
                                                (name != null) ? name : "NA",
                                                (lrow["ModifiedDate"] != DBNull.Value) ? lrow["ModifiedDate"].ToString() : "NA",
                                                (lrow[lcolumn.ColumnName] != DBNull.Value) ? lrow[lcolumn.ColumnName].ToString() : "NA",
                                                "NA",
                                                (prow[column_identifiers] != DBNull.Value) ? prow[column_identifiers].ToString() : "NA"));
                                            }
                                        }
                                    }

                                }

                            }
                        }
                    else
                    {
                        //Test case 4: New row is added

                        list.Add
                        (new Changes
                             (tablename,
                             (primaryKey != null) ? primaryKey : "NA",
                             (name != null) ? name : "NA",
                             (lrow["ModifiedDate"] != DBNull.Value) ? lrow["ModifiedDate"].ToString() : "NA",
                             "NA",
                             (lrow[primaryKey].ToString() != null) ? lrow[primaryKey].ToString():"NA",
                             (lrow[column_identifiers].ToString()!=null)? lrow[column_identifiers].ToString():"NA")    
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
                            (item.column_name != null) ? item.column_name : "NA",
                            (item.name != null) ? item.name : "NA",
                            (item.mDate != null) ? item.mDate : "NA",
                            (item.old_data != null) ? item.old_data : "NA",
                            (item.new_data != null) ? item.new_data : "NA",
                            (item.row_data != null) ? item.row_data : "NA");
                    }
                }
            }

            //Clear the list
            list.Clear();
        }
        //Test case 5: A row is deleted
        public void RowComparison(string tablename, string columnIdentifier)
        {

            //live datatable
            string query1 = String.Format("select * from {0}", tablename);
            DataTable liveDt = Read(query1, LiveString());

            //Primary Key
            string pkey = liveDt.Columns[0].ColumnName;

            //Resultant DataTable
            DataTable dt = new DataTable();

            //publish datatable
            string query = String.Format("select * from {0}", tablename);
            DataTable publishDt = Read(query, PublishString());

            foreach (DataRow prow in publishDt.Rows)
            {
                DataRow[] deleted = liveDt.Select(string.Format("{0} = {1}", pkey, prow[pkey].ToString()));
                if (deleted.Count() == 0)
                {
                    foreach(DataColumn pcolumn in publishDt.Columns)
                    {
                        //Ignore the column values which are present in ignoredcolumns list
                        if (!ignoredColumns.Contains(pcolumn.ColumnName))
                        {
                            //Create a new object and add it to the list
                            list.Add
                                (new Changes
                                (tablename,
                                 (pcolumn.ColumnName != null) ? pcolumn.ColumnName : "NA",
                                 "NA",
                                 "NA",
                                 (prow[pcolumn.ColumnName].ToString() != null) ? prow[pcolumn.ColumnName].ToString() : "NA",
                                 "NA",
                                 (prow[columnIdentifier].ToString() != null) ? prow[columnIdentifier].ToString() : "NA"
                                ));
                        }
                    }
                }
            }
            //Using the streamwriter write the contents into the file
            using (StreamWriter sr = File.AppendText(path))
            {
                foreach (var item in list)
                {
                    sr.WriteLine("{0},{1},{2},{3},{4},{5},{6}",
                        (tablename != null) ? tablename : "NA",
                        (item.column_name != null) ? item.column_name : "NA",
                        (item.name != null) ? item.name : "NA",
                        (item.mDate != null) ? item.mDate : "NA",
                        (item.old_data != null) ? item.old_data : "NA",
                        (item.new_data != null) ? item.new_data : "NA",
                        (item.row_data != null) ? item.row_data : "NA");
                }
            }
            //Clear the list
            list.Clear();
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
