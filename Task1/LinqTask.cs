using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Task1.DoNotChange;

namespace Task1
{
    
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            var result = from c in customers where c.Orders.Sum(o=>o.Total) > limit select c;
            return result;
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            if (suppliers == null) throw new ArgumentNullException(nameof(suppliers));

            return
              customers.Select(c =>(c, suppliers.Where(p=>p.Country==c.Country && p.City ==c.City)));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            if (suppliers == null) throw new ArgumentNullException(nameof(suppliers));
            
            var result = customers.GroupJoin(suppliers,
                              c => new { c.Country, c.City },
                              s => new { s.Country, s.City },
                              (c, ss) => ( customer : c, suppliers : ss ));
            return result;
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
           if (customers == null) throw new ArgumentNullException(nameof(customers));
           return  customers.Where(c => c.Orders.Any(o => o.Total > limit)); 
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            return  customers.Where(c=>c.Orders.Length>0).Select(x => (customer: x, dateOfEntry: x.Orders.Min(o=>o.OrderDate)));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            var res = Linq4(customers);
            return res.OrderBy(p => p.dateOfEntry.Year).ThenBy(p => p.dateOfEntry.Month).ThenByDescending(p => p.customer.Orders.Sum(p => p.Total)).ThenBy(p => p.customer.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            var res = from c in customers
                      where c.PostalCode == null || c.PostalCode.Any(p => !char.IsDigit(p))
                                                 || string.IsNullOrWhiteSpace(c.Region)
                                                 || !c.Phone.StartsWith("(")
                      select c;
            return res;
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {

            if (products == null) throw new ArgumentNullException(nameof(products));
            var res = from p in products
                         group p by p.Category
                         into categoryGroup
                         select new Linq7CategoryGroup()
                         {
                             Category = categoryGroup.Key,
                             UnitsInStockGroup = from p in categoryGroup
                                        group p by p.UnitsInStock
                                                   into availabilityGroup
                                        select new Linq7UnitsInStockGroup()
                                        {
                                            UnitsInStock= availabilityGroup.Key,
                                            Prices = from p in availabilityGroup
                                                       orderby p.UnitPrice
                                                       select p.UnitPrice
                                        }
                         };

            return res;
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            if (products == null) throw new ArgumentNullException(nameof(products));
            var result = products
                    .GroupBy(p => p.UnitPrice <= cheap ? cheap
                                : p.UnitPrice <= middle ? middle
                                : expensive).Select(x => (x.Key, from k in x select k));
            return result;
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            var res = customers.GroupBy(p => p.City).Select(x =>(x.Key, (int)Math.Round(x.Average(c=>c.Orders.Sum(o=>o.Total))), (int)x.Average(c=>c.Orders.Length)));
            return res;
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            var stringBuilder = new StringBuilder();
            var res = suppliers.OrderBy(p => p.Country.Length).ThenBy(p=> p.Country).GroupBy(p => p.Country);
            res.ToList().ForEach(x => stringBuilder.Append(x.Key));
            return stringBuilder.ToString();
        }
    }
}