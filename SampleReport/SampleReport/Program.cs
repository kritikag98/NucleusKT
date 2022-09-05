using NucleusComparison;
using System;
using System.Collections;
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
            DBConnection db = new DBConnection();
            string[] tablename = { "cabinetsubtypes", "handles", "panels", "products", "wardrobemulticabinets" };
            for (int i = 0; i < tablename.Length; i++)
            {
                Console.WriteLine("For the table {0}", tablename[i]);
                db.comparison(tablename[i]);
            }
            Console.ReadLine();
        }
    }
}
