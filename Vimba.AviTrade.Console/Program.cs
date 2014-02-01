using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Repositories.Helpers;

namespace Vimba.AviTrade.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            var orders = 1;
            var daysBetweenOrderEvents = 20;
            
            if (0 == 1)
            {
                var seeder = new DatabaseSeeder();
                seeder.CreateInitialData();
                orders = 10;
                seeder.CreateOrders(orders, daysBetweenOrderEvents);
            }
            else if (0 == 0)
            {
                var seeder = new DatabaseSeeder(false);
                seeder.CreateOrders(orders, daysBetweenOrderEvents);
            }

            Console.WriteLine("Completed " + orders + " orders at " + DateTime.Now + "! Each has a max of days between order events of " + daysBetweenOrderEvents + ". The process took: " + DateTime.Now.Subtract(startTime).TotalMinutes + " minutes.");
            Console.ReadLine();
        }
    }
}
