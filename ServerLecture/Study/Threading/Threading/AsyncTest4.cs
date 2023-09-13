using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    public class AsyncTest4
    {
        public void Start()
        {
            HelloAsync(() => {
                Console.WriteLine("Task Done");
                Start();
            });   
        }

        private async void HelloAsync(Action callback)
        {
            int i = 0;
            for(; i < 10; i++)
            {
                Console.WriteLine($"Hello, Async! : {i}");
                await Task.Delay(500);
            }

            callback?.Invoke();
        }
    }
}
