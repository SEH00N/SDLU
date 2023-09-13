using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    public class AsyncTest1
    {
        public void Start()
        {
            HelloAsync(() => Console.WriteLine("Task Done"));
            Console.WriteLine("Hello, Main1");
        }

        private async void HelloAsync(Action callback)
        {
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Hello, Async : {i}");

                await Task.Delay(500);
            }

            callback?.Invoke();
        }
    }
}
