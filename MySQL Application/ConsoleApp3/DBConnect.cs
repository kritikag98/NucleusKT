using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace ConsoleApp3
{

    internal class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "mysql";
            uid = "root";
            password = "homelane@123";
            string connectionString;
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

        //Insert statement
        public void Insert()
        {
            //Validation for int
            Console.WriteLine("Enter the roleid you want to enter: ");
            int x;
            var xAsString = Console.ReadLine();
            while (!int.TryParse(xAsString, out x))
            {
                Console.WriteLine("This is not a number!");
                xAsString = Console.ReadLine();
            }

            //Validation for string
            Console.WriteLine("Enter the description for the given roleid: ");
            string y = Console.ReadLine();

            string query = "INSERT INTO role (roleid, description) VALUES(@roleid, @description)";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@roleid", x);
                cmd.Parameters.AddWithValue("@description", y);
                cmd.Prepare();
                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }
        //Update statement
        public void Update()
        {
            Console.WriteLine("Enter the name you want to change: ");
            string w = Console.ReadLine();

            //Validation for int
            Console.WriteLine("Enter the roleid for which you want to change the name: ");
            int z;
            var zAsString = Console.ReadLine();
            while (!int.TryParse(zAsString, out z))
            {
                Console.WriteLine("This is not a number!");
                zAsString = Console.ReadLine();
            }

            Console.WriteLine("Enter the new name you want to add: ");
            string c = Console.ReadLine();

            string query = "UPDATE users SET name=@name, roleid=@roleid WHERE name=@oldname";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@oldname", w);
                cmd.Parameters.AddWithValue("@roleid", z);
                cmd.Parameters.AddWithValue("@name", c);

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            Console.WriteLine("Enter the name you want to delete: ");
            string y;
            y = Console.ReadLine();
            string query = "DELETE FROM users WHERE name=@name";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", y);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public void Select()
        {
            string query = "SELECT * FROM users";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Display the contents of users table
                Console.WriteLine("The contents of the users table are: ");
                while (dataReader.Read())
                {
                    Console.WriteLine("{0} {1} {2} ", dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetInt32(2));
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

            }
        }
         public void SelectRole()
        {
            string query = "SELECT * FROM role";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Display the contents of users table
                Console.WriteLine("The contents of the role table are: ");
                while (dataReader.Read())
                {
                    Console.WriteLine("{0} {1}", dataReader.GetInt32(0), dataReader.GetString(1));
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

            }
        }
    }
}
