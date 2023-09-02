using System;
using System.Threading.Tasks;

namespace MyThreading
{
    public class AsyncTest4
    {
        public void Start()
        {
            HelloAsync(() => {
                Console.WriteLine("Task done");
                Start();
            });
        }

        private async Task HelloAsync(Action callback)
        {
            int i = 0;
            for (; i < 10; i++)
            {
                Console.WriteLine($"Hello, Async! : {i}");

                await Task.Delay(500);
            }

            callback?.Invoke();
        }
    }
}
