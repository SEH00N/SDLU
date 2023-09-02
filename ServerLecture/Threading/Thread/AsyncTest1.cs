using System;
using System.Threading.Tasks;

namespace MyThreading
{
    public class AsyncTest1
    {
        public void Start()
        {
            // Async 비동기를 사용한 방식 (내부적으로는 Task로 작동 됨)
            HelloAsync(() => Console.WriteLine("Task done"));
            Console.WriteLine("Hello, Main!");

            // 이렇게 했을 때 Hello, Main!이 출력 된 후 바로 프로세스가 종료 됨
        }

        private async void HelloAsync(Action callback)
        {
            for(int i = 0; i < 10; i ++)
            {
                Console.WriteLine($"Hello, Async! : {i}");

                // Task.Delay는 Task를 반환
                // await은 작업을 기다리는 키워드. async 함수 안에서만 쓸 수 있음
                // 유니티의 yield return 처럼 생각하면 됨
                await Task.Delay(500);
            }

            callback?.Invoke();
        }
    }
}
