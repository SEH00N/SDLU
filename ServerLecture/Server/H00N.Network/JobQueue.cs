using System;
using System.Collections.Generic;

namespace H00N.Network
{
    /// <summary>
    /// 작업을 모아두고 한 번에 플러시 시킬 수 있는 Queue 형태의 자료구조
    /// </summary>
    public class JobQueue : IFlushable<Action>
    {
        private Queue<Action> jobQueue = new Queue<Action>(); // 작업을 저장해둘 큐
        private object locker = new object(); // jobQueue 락커
        private bool flush = false; // 비동기 동기화, 예외처리용 불변수

        public void Push(Action job) // 작업을 예약하는 함수
        {
            lock (locker) // jobQueue 락킹
            {
                jobQueue.Enqueue(job); // 작업 예약

                if (flush == false) // 현재 플러시하고있지 않으면
                    flush = true; // 플러시 예약
            }

            if (flush) // 플러시가 예약되어 있으면
                Flush(); // jobQueue 플러시
        }

        public void Flush() // 작업을 플러시하는 함수
        {
            while (true) // jobQueue가 빌 때까지
            {
                Action job = null; // 작업
                lock (locker)
                {
                    if (jobQueue.Count <= 0) // jobQueue가 비어있으면
                    {
                        flush = false; // 플러시를 끝내고
                        break; // 루프 종료
                    }

                    job = jobQueue.Dequeue(); // 작업 빼내고
                }
                
                job?.Invoke(); // 작업 실행
            }
        }
    }
}
