using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            double a = Convert.ToDouble(Console.ReadLine());
            double b = Convert.ToDouble(Console.ReadLine());
            double sum = a + b;
            double diff = a - b;
            double prd = a * b;
            Console.WriteLine("Sum is "+sum);
            Console.WriteLine("Difference is: " + diff);
            Console.WriteLine("Product is: " + prd);
            Console.ReadLine();
        }
    }
}
