using System;
using System.Threading;

namespace Threading
{
    internal class ThreadTest1
    {
        public void Start()
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);

            thread1.Name = "1번 스레드";
            thread2.Name = "2번 스레드";

            thread1.Start();
            thread2.Start();
        }

        private void Thread1()
        {
            Console.WriteLine($"Thread1, {Thread.CurrentThread.Name}");
        }

        private void Thread2()
        {
            Console.WriteLine($"Thread2, {Thread.CurrentThread.Name}");

        }
    }
}
