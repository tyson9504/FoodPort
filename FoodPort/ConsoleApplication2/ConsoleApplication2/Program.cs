using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FoodPort_Payment
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(BankService));
            host.Open();
            Console.WriteLine("Bank Service Started");
            Console.ReadLine();
        }
    }
}
