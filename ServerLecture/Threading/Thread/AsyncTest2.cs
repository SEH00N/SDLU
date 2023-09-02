using System;
using System.Threading.Tasks;

namespace MyThreading
{
    public class AsyncTest2
    {
        public async void Start()
        {
            // Hello Async 라는 작업이 끝날 때까지 기다리기 위해 await 키워드 사용
            // await을 하기 위해선 HelloAsync는 Task를 반환해야 함
            await HelloAsync(() => Console.WriteLine("Task done"));

            // HelloAsync의 모든 작업이 끝나면 Hello, Main! 이 출력됨
            Console.WriteLine("Hello, Main!");
        }

        // 근데 이런 함수에선 값을 반환하지 못함
        // 어떻게 반환할 수 있을까
        private async Task HelloAsync(Action callback)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Hello, Async! : {i}");

                await Task.Delay(500);
            }

            callback?.Invoke();
        }
    }
}
