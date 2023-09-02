using System;
using System.Threading.Tasks;

namespace MyThreading
{
    public class AsyncTest3
    {
        public async void Start()
        {
            // 작업 기다리기도 해야하고 값을 받기도 해야할 땐 Task<T> 형태를 반환
            // HelloAsync의 작업이 끝났을 때 반환되는 값을 받을 수 있음
            int printedCount = await HelloAsync(() => Console.WriteLine("Task done"));

            // HelloAsync의 모든 작업이 끝나면 반환받은 출력된 횟수 출력
            Console.WriteLine($"printed count : {printedCount}");
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

            // 모든 작업 후에 출력된 횟수 반환
            return i;
        }
    }
}
