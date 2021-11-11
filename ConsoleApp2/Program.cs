using System;
using System.Linq;
using Task1.Tests;
using Task1;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var customers = DataSource.Customers;
            var suppliers = DataSource.Suppliers;


            var res1 = LinqTask.Linq2(customers, suppliers);
            var res2 = LinqTask.Linq2UsingGroup(customers, suppliers);
            Console.WriteLine(res1.ToList().Count);
            Console.WriteLine(res2.ToList().Count);





        }
    }
}
