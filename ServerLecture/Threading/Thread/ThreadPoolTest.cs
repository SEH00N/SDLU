using System;
using System.Threading;

namespace MyThreading
{
    public class ThreadPoolTest
    {
        public void Start()
        {
            // 스레드 풀을 사용한 방식 (무조건 매개변수를 받아야 함)
            ThreadPool.QueueUserWorkItem(Work1, 10);
            ThreadPool.QueueUserWorkItem(Work2, "This is String");

            Console.WriteLine("This is main");

            Console.ReadKey();
        }

        private void Work1(object obj)
        {
            Console.WriteLine($"Work1 : {obj}");
        }

        private void Work2(object obj)
        {
            Console.WriteLine($"Work2 : {obj}");
        }
    }
}
