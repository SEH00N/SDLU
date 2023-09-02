using System;
using System.Threading;

namespace MyThread
{
    public class ThreadTest3
    {
        public void Start()
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);

            thread1.Start();
            thread2.Start();

            // 백그라운드 스레드 => 메인 스레드가 종료되면 같이 종료 됨
            // 포그라운드 스레드 => 메인 스레드가 종료되면 메인스레드로 연임 됨
            thread1.IsBackground = false;
            thread2.IsBackground = false;

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
