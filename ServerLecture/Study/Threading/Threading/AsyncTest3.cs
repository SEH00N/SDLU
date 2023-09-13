using System;
using System.Threading.Tasks;

namespace Threading
{
    public class AsyncTest3
    {
        public async void Start()
        {
            int printedCount = await HelloAsync(() => Console.WriteLine("Task Done"));
            Console.WriteLine($"Printed Count : {printedCount}");
        }

        private async Task<int> HelloAsync(Action callback)
        {
            int i = 0;
            for (; i < 10; i++)
            {
                Console.WriteLine($"Hello, Async! : {i}");

                await Task.Delay(500);
            }

            callback?.Invoke();

            return i;
        }
    }
}
