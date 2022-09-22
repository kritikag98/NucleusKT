using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using NucleusComparison;

namespace ConsoleApp3
{

    internal class DBConnection
    {
        List<Changes> list = new List<Changes>();
        public string path = @"C:\Users\kritika.g\Documents\b1.csv";
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string connectionString;

        /*private string createConnectionString()
        {
            server = "hl-nucleus-catalog-db-stage-1.cpis3h9hmzgj.ap-south-1.rds.amazonaws.com";
            database = "nucleus_config_homelane_sandbox_published";
            uid = "deltacad";
            password = "vjwtbDsLtrcJFcxJ";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            return connectionString;
        }
        private string createConnectionString1()
        {
            server = "hl-nucleus-catalog-db-stage-1.cpis3h9hmzgj.ap-south-1.rds.amazonaws.com";
            database = "nucleus_config_homelane_sandbox_unpublished";
            uid = "deltacad";
            password = "vjwtbDsLtrcJFcxJ";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            return connectionString;
        }*/

        private string createConnectionString()
        {
            server = "localhost";
            database = "nucleus_config_homelane_sandbox_published";
            uid = "root";
            password = "homelane@123";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            return connectionString;
        }
        private string createConnectionString1()
        {
            server = "localhost";
            database = "nucleus_config_homelane_sandbox_unpublished";
            uid = "root";
            password = "homelane@123";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            return connectionString;
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public void Select(string tablename)
        {
            string query = String.Format("SELECT * FROM {0}", tablename);
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                List<string> lst = new List<string>();
                foreach (DataColumn dc in dt.Columns)
                    lst.Add(dc.ColumnName);
                sda.Fill(dt);
            }
            //close Connection
            this.CloseConnection();
        }

        public DataTable Read(string sSQLQuery, string connection)
        {
            /*DbDataAdapter adapter = new MySqlDataAdapter(sSQLQuery, new MySqlConnection(connection));
            DataTable dtCities = new DataTable();
            adapter.Fill(dtCities);*/

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
        /*public DataTable Read(string sSQLQuery, string connection)
        {
            DbDataAdapter adapter = new MySqlDataAdapter(sql, (MySqlConnection)connection);
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
        }*/
        public string LastPublishDate()
        {
            string query = "select PublishDate from publish ORDER BY PublishDate DESC LIMIT 1";
            DataTable x = Read(query, createConnectionString());
            if (x != null && x.Rows.Count > 0)
            {
                return x.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        //live datatable
        public DataTable liveTableRead(string tablename)
        {
            string y = Convert.ToDateTime(LastPublishDate()).ToString("yyyy-MM-dd hh:mm:ss");
            //string y = "2022-08-01 04:06:37";
            string query = String.Format("select * from {0} where ModifiedDate > '{1}'", tablename, y);
            DataTable x = Read(query, createConnectionString1());
            return x;
        }

        //publish datatable
        public DataTable publishTableRead(string tablename)
        {
            string query = String.Format("select * from {0}", tablename);
            DataTable publishDt = Read(query, createConnectionString());
            return publishDt;
        }

        /*int i = 0;
        //Datarow for iterating over the publish datatable
        public DataRow publishRow(string tablename, string pkey, string val)
        {
            DataTable publishDt = publishTableRead(tablename);
            string query = String.Format("select * from {0} where {1} = {2}", tablename, pkey, val);
            DataTable x = Read(query, createConnectionString());
            return x.Rows[i++];
        }*/

        //getting the name of the person who modified a particular row
        public string modifiedBy(string usrId)
        {
            string query = String.Format("select Fullname from users where UserID = {0}", usrId);
            DataTable x = Read(query, createConnectionString());
            return x.Rows[0][0].ToString();
        }
        //Comparing the publish and live table and writing the results in the file
        ///*public void comparison(string tablename, string column_identifiers)
        //{
        //    //publish database
        //    DataTable publishDt = publishTableRead(tablename);

        //    //live database
        //    DataTable liveDt = liveTableRead(tablename);


        //    //Check if the publish table is empty or not
        //    if (liveDt != null && liveDt.Rows.Count > 0)
        //    {
        //        string pkey = liveDt.Columns[0].ColumnName;
        //        foreach (DataRow lrow in liveDt.Rows)
        //        {
        //            foreach (DataRow prow in publishDt.Select(String.Format("{0} = {1}", pkey, lrow[pkey])))
        //            /*DataRow prow = publishRow(tablename, pkey, lrow[pkey].ToString());
        //            if (prow != null)*/
        //            {
        //                string name = modifiedBy(lrow["ModifiedById"].ToString());
        //                foreach (DataColumn lcolumn in liveDt.Columns)
        //                {
        //                    // Test case 1: Existing column modified
        //                    if (publishDt.Columns.Contains(lcolumn.ColumnName))
        //                    {
        //                        if (!Equals(prow[lcolumn.ColumnName].ToString(), lrow[lcolumn.ColumnName].ToString()))
        //                        {
        //                            //string name = modifiedBy(lrow["ModifiedById"].ToString());
        //                            if (!Equals(lcolumn.ColumnName, "ModifiedDate") && !("ModifiedById".Equals(lcolumn.ColumnName, StringComparison.OrdinalIgnoreCase)))
        //                            {
        //                                list.Add
        //                                    (new Changes
        //                                    (tablename,
        //                                    (lcolumn.ColumnName != null) ? lcolumn.ColumnName : "NA",
        //                                    (name != null) ? name : "NA",
        //                                    (lrow["ModifiedDate"] != DBNull.Value) ? lrow["ModifiedDate"].ToString() : "NA",
        //                                    (prow[lcolumn.ColumnName] != DBNull.Value) ? prow[lcolumn.ColumnName].ToString() : "NA",
        //                                    (lrow[lcolumn.ColumnName] != DBNull.Value) ? lrow[lcolumn.ColumnName].ToString() : "NA",
        //                                    (prow[column_identifiers] != DBNull.Value) ? prow[column_identifiers].ToString() : "NA"));
        //                            }
        //                        }

        //                    }

        //                    Test case 2: New column added
        //                    else
        //                    {
        //                        if (!publishDt.Columns.Contains(lcolumn.ColumnName))
        //                        {
        //                            if (!Equals(lcolumn.ColumnName, "ModifiedDate") && !("ModifiedById".Equals(lcolumn.ColumnName, StringComparison.OrdinalIgnoreCase)))
        //                            {
        //                                if (lrow[lcolumn.ColumnName] != DBNull.Value)
        //                                {
        //                                    list.Add
        //                                    (new Changes
        //                                    (tablename,
        //                                    (lcolumn.ColumnName != null) ? lcolumn.ColumnName : "NA",
        //                                    (name != null) ? name : null,
        //                                    (lrow["ModifiedDate"] != DBNull.Value) ? lrow["ModifiedDate"].ToString() : "NA",
        //                                    null,
        //                                    (lrow[lcolumn.ColumnName] != DBNull.Value) ? lrow[lcolumn.ColumnName].ToString() : "NA",
        //                                    (prow[column_identifiers] != DBNull.Value) ? prow[column_identifiers].ToString() : "NA"));
        //                                }

        //                            }
        //                        }

        //                        //Test case 3: Existing column deleted
        //                        foreach (DataColumn pcolumn in publishDt.Columns)
        //                        {
        //                            if (!liveDt.Columns.Contains(pcolumn.ColumnName))
        //                            {
        //                                if (!Equals(lcolumn.ColumnName, "ModifiedDate") && !("ModifiedById".Equals(lcolumn.ColumnName, StringComparison.OrdinalIgnoreCase)))
        //                                {
        //                                    list.Add
        //                                   (new Changes
        //                                   (tablename,
        //                                   (pcolumn.ColumnName != null) ? pcolumn.ColumnName : "NA",
        //                                   (name != null) ? name : null,
        //                                   (lrow["ModifiedDate"] != DBNull.Value) ? lrow["ModifiedDate"].ToString() : "NA",
        //                                   (lrow[lcolumn.ColumnName] != DBNull.Value) ? lrow[lcolumn.ColumnName].ToString() : "NA",
        //                                   null,
        //                                   (prow[column_identifiers] != DBNull.Value) ? prow[column_identifiers].ToString() : "NA"));
        //                                }*/
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //        }*/
        //        //Storing the details in the table b1 if the column value is modified

        //        using (StreamWriter sr = File.AppendText(path))
        //        {
        //           foreach (var item in list)
        //             {
        //                sr.WriteLine("{0},{1},{2},{3},{4},{5},{6}", 
        //                    (tablename!=null)?tablename:"NA", 
        //                    (item.column_name != null) ? item.column_name : "NA", 
        //                    (item.name != null) ? item.name : "NA",
        //                    (item.mDate != null) ? item.mDate : "NA",
        //                    (item.old_data != null) ? item.old_data : "NA", 
        //                    (item.new_data != null) ? item.new_data : "NA", 
        //                    (item.row_data != null) ? item.row_data : "NA");
        //             }
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("The publish table is empty.");
        //        return;
        //    }
        //    list.Clear();
            
        //}*/

        public void deletedRows(string tablename)
        {
            DataTable x = Read(tablename, createConnectionString());
        }
        public void fileDelete()
        {
            if (File.Exists(path))
                File.Delete(path);
        }

    }
}
