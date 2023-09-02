using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MyThreading
{
    public class LockTest
    {
        private long origin = 0;
        private object locker = new object();

        public void Start()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            {
                Task t1 = Task.Run(() => Add(0 * 10000 + 1, 2500 * 10000));
                Task t2 = Task.Run(() => Add(2500 * 10000 + 1, 5000 * 10000));
                Task t3 = Task.Run(() => Add(5000 * 10000 + 1, 7500 * 10000));
                Task t4 = Task.Run(() => Add(7500 * 10000 + 1, 10000 * 10000));

                Task.WaitAll(t1, t2, t3, t4);

                Console.WriteLine(origin);
            }

            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);
        }

        private void Add(long from, long to)
        {
            for (long i = from; i < to + 1; i++)
            {
                lock (locker)
                {
                    origin += i;
                }
            }
        }
    }
}
