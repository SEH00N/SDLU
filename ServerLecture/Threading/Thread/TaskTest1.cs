using System;
using System.Threading.Tasks;

namespace MyThreading
{
    public class TaskTest1
    {
        public void Start()
        {
            // Task 객체를 생성해서 실행시키는 방식
            Task task1 = new Task(Task1);
            Task task2 = new Task(Task2);

            task1.Start();
            task2.Start();

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
