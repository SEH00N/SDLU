using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    public class AsyncTest2
    {
        public async void Start()
        {
            await HelloAsync(() => Console.WriteLine("Task Done"));
            Console.WriteLine("Hello, Main1");
        }

        private async Task HelloAsync(Action callback)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Hello, Async : {i}");

                await Task.Delay(500);
            }

            callback?.Invoke();
        }
    }
}
