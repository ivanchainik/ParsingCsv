using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main()
        {
            List<Organization> oldModelsList = new List<Organization>();
            oldModelsList = Organization.ReadFile("пример тз.csv");
            Organization.WriteFile(oldModelsList, "Processed Data.csv");
        }

    }
}
