using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    internal class ThreadTest2
    {
        public void Start()
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("This is main thread");
        }

        private void Thread1()
        {
            Console.WriteLine($"Thread1");
        }

        private void Thread2()
        {
            Console.WriteLine($"Thread2");

        }
    }
}
