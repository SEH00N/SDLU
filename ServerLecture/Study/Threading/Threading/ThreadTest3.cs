using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    internal class ThreadTest3
    {
        public void Start()
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);

            thread1.IsBackground = true;
            thread2.IsBackground = true;

            thread1.Start();
            thread2.Start();

            Console.WriteLine("This is main thread");
        }

        private void Thread1()
        {
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"Thread1 : {i}");
        }

        private void Thread2()
        {
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"Thread2 : {i}");
        }
    }
}
