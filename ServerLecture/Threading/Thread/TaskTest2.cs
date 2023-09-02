using System;
using System.Threading.Tasks;

namespace MyThreading
{
    public class TaskTest2
    {
        public void Start()
        {
            // Task의 스태틱 메소드 Run을 사용하여 실행시키는 방식
            // 내부적으로는 스레드풀로 작동 됨
            Task task1 = Task.Run(Task1);
            Task task2 = Task.Run(Task2);

            Task.WaitAll(task1, task2);

            Console.WriteLine("This is main");
        }

        private void Task1()
        {
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"Task1 : {i}");
        }

        private void Task2()
        {
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"Task2 : {i}");
        }
    }
}
