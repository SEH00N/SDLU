using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    internal class TaskTest1
    {
        public void Start()
        {
            Task task1 = new Task(Task1);
            Task task2 = new Task(Task2);

            task1.Start();
            task2.Start();

            Task.WaitAll(task1, task2);

            Console.WriteLine("This is main");
        }

        private void Task1()
        {
            for(int i = 0; i < 100; i ++)
                Console.WriteLine($"Task1, {i}");
        }

        private void Task2()
        {
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"Task2, {i}");
        }
    }
}
