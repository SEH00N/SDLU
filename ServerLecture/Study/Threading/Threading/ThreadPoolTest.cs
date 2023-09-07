using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    internal class ThreadPoolTest
    {
        public void Start()
        {
            ThreadPool.QueueUserWorkItem(Work1, "This is String");
            ThreadPool.QueueUserWorkItem(Work2, 255);

            Console.WriteLine("This is main");

            Console.ReadKey();
        }

        private void Work1(object param)
        {
            Console.WriteLine($"Work1 : {param}");
        }

        private void Work2(object param)
        {
            Console.WriteLine($"Work2 : {param}");
        }
    }
}
