using System;
using System.Threading;

namespace MyThreading
{
    public class ThreadPoolTest
    {
        public void Start()
        {
            // 스레드 풀을 사용한 방식 (무조건 매개변수를 받아야 함)
            ThreadPool.QueueUserWorkItem(Work1);
            ThreadPool.QueueUserWorkItem(Work2);

            Console.WriteLine("This is main");

            Console.ReadKey();
        }

        private void Work1(object obj)
        {
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"Work1 : {i}");
        }

        private void Work2(object obj)
        {
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"Work2 : {i}");
        }
    }
}
