using System;
using System.Linq;
using Task1.Tests;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var customers = DataSource.Customers;
            var suppliers = DataSource.Suppliers;
            // var result = customers.Join(suppliers, c =>  c.City, s => s.City, (c, s) => (customers.Where(p=>p.City==s.City)));

         

            Console.WriteLine(result.ToList().Count);
        }
    }
}
