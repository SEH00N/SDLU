using System;

namespace H00N.Network
{
    public class JobQueue : IFlushable<Action>
    {
        private Queue<Action> jobQueue = new Queue<Action>();
        private object locker = new object();
        private bool flush = false;

        public void Push(Action job)
        {
            lock(locker)
            {
                jobQueue.Enqueue(job);

                if (flush == false)
                    flush = true;
            }

            if (flush)
                Flush();
        }

        public void Flush()
        {
            while(true)
            {
                Action job = null;
                lock(locker)
                {
                    if(jobQueue.Count <= 0)
                    {
                        flush = false;
                        break;
                    }

                    job = jobQueue.Dequeue();
                }

                job?.Invoke();
            }
        }
    }
}
