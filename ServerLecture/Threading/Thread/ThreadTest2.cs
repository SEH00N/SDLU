using System;
using System.Threading;

namespace MyThreading
{
    public class ThreadTest2
    {
        public void Start()
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);

            thread1.Start();
            thread2.Start();

            // Join 함수 => 해당 스레드의 작업이 끝날 때까지 해당 스레드를 호출한 스레드를 멈춤
            thread1.Join();
            thread2.Join();

            // Join을 걸면 해당 메세지가 항상 마지막에 뜨지만 Join을 하지 않으면 처음에 뜰지 중간에 뜰지 마지막에 뜰지 모름
            Console.WriteLine("This is main");
        }

        private void Thread1()
        {
            Console.WriteLine($"Hello, Thread1!");
        }

        private void Thread2()
        {
            Console.WriteLine($"Hello, Thread2!");
        }
    }

}
