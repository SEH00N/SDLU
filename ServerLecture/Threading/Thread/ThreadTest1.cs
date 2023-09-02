using System;
using System.Threading;

namespace MyThreading
{
    public class ThreadTest1
    {
        public void Start()
        {
            // 스레드 생성자 => 실행할 작업을 매개변수로 넘겨줄 수 있음
            Thread thread1 = new Thread(Thread1);  
            Thread thread2 = new Thread(Thread2);

            // 스레드의 이름 변경
            thread1.Name = "1번 스레드";
            thread2.Name = "2번 스레드";

            // Start => 스레드 작업 시작
            thread1.Start();
            thread2.Start();
        }

        private void Thread1()
        {
            // Thread.CurrentThread => 현재 코드가 실행되고 있는 스레드를 받아옴
            Console.WriteLine($"Thread1, {Thread.CurrentThread.Name}");
        }

        private void Thread2()
        {
            Console.WriteLine($"Thread2, {Thread.CurrentThread.Name}");
        }
    }
}
