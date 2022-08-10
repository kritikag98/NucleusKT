using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnect db = new DBConnect();
            //db.Insert();
            //db.Update();
            //db.Delete();
            db.Select();
            db.SelectRole();
            Console.ReadLine();
        }
    }
}
