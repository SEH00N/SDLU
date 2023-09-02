using System;
using System.Threading;

namespace MyThreading
{
    public class ThreadTest3
    {
        public void Start()
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);

            // 백그라운드 스레드 => 모든 Foreground 스레드가 종료되면 같이 종료 됨
            // 포그라운드 스레드 => 하나의 Foreground 스레드라도 존재한다면 프로세스는 종료되지 않음
            thread1.IsBackground = true;
            thread2.IsBackground = true;

            thread1.Start();
            thread2.Start();

            Console.WriteLine("This is main");
        }

        private void Thread1()
        {
            for(int i = 0; i < 100; i++)
                Console.WriteLine($"Hello, Thread1! : {i}");
        }

        private void Thread2()
        {
            for(int i = 0; i < 100; i++)
                Console.WriteLine($"Hello, Thread2! : {i}");
        }
    }
}
