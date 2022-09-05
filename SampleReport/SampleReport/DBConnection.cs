using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using NucleusComparison;

namespace ConsoleApp3
{

    internal class DBConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string connectionString;
        //Constructor
        public DBConnection()
        {
            Initialize();
        }

        private string createConnectionString()
        {
            server = "localhost";
            database = "nucleuss3";
            uid = "root";
            password = "homelane@123";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            return connectionString;
        }
        private string createConnectionString1()
        {
            server = "localhost";
            database = "nucleus";
            uid = "root";
            password = "homelane@123";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            return connectionString;
        }
        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "nucleuss3";
            uid = "root";
            password = "homelane@123";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
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

        public DataTable publishTableRead(string tablename)
        {
            string y = Convert.ToDateTime(LastPublishDate()).ToString("yyyy-MM-dd hh:mm:ss");
            string query = String.Format("select * from {0} where ModifiedDate > '{1}'", tablename, y);
            DataTable x = Read(query, createConnectionString1());
            return x;
        }

        public DataTable liveTableRead(string tablename)
        {
            string query = String.Format("select * from {0}", tablename);
            DataTable x = Read(query, createConnectionString());
            return x;
        }

        public string modifiedBy(string usrId)
        {
            string query = String.Format("select Fullname from users where UserID = {0}", usrId);
            DataTable x = Read(query, createConnectionString());
            return x.Rows[0][0].ToString();
        }

        public void comparison(string tablename)
        {
            DataTable liveDt = liveTableRead(tablename);
            DataTable publishDt = publishTableRead(tablename);
            string pkey = liveDt.Columns[0].ColumnName;
            List<Changes> list = new List<Changes>();
            foreach (DataRow prow in publishDt.Rows)
            {
                foreach (DataRow lrow in liveDt.Select(String.Format("{0} = {1}", pkey, prow[pkey])))
                {
                    foreach (DataColumn lcolumn in liveDt.Columns)
                    {
                        if (publishDt.Columns.Contains(lcolumn.ColumnName))
                        {
                            if (!Equals(prow[lcolumn.ColumnName], lrow[lcolumn.ColumnName]))
                            {
                                string name = modifiedBy(lrow["ModifiedById"].ToString());
                                list.Add(new Changes(lcolumn.ColumnName, lrow[lcolumn.ColumnName].ToString(), prow[lcolumn.ColumnName].ToString(), prow[pkey].ToString(), name));
                            }
                        }
                    }
                }
            }
            FileStream f = new FileStream("C:\\Users\\kritika.g\\Documents\\b1.csv", FileMode.OpenOrCreate);

            Console.WriteLine("File opened");

            //declared stream writer
            StreamWriter s = new StreamWriter(f);

            Console.WriteLine("Writing data to file");

            Console.WriteLine("File Stream closed");

            foreach (var item in list)
            {
                s.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}", tablename, pkey, item.column_name, item.new_data, item.old_data, item.row_data, item.name);
            }

            //closing stream writer
            s.Close();
            f.Close();
        }
    }
}
