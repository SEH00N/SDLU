namespace Threading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ThreadTest1 test1 = new ThreadTest1();
            //test1.Start();

            //ThreadTest2 test2 = new ThreadTest2();
            //test2.Start();

            //ThreadTest3 test3 = new ThreadTest3();
            //test3.Start();

            //ThreadPoolTest test4 = new ThreadPoolTest();
            //test4.Start();

            //TaskTest1 test5 = new TaskTest1();
            //test5.Start();

            //TaskTest2 test6 = new TaskTest2();
            //test6.Start();

            //AsyncTest1 test7 = new AsyncTest1();
            //test7.Start();

            //AsyncTest2 test8 = new AsyncTest2();
            //test8.Start();

            //AsyncTest3 test9 = new AsyncTest3();
            //test9.Start();

            //AsyncTest4 test10 = new AsyncTest4();
            //test10.Start();

            //while(true)
            //{
            //    string msg = Console.ReadLine();
            //    Console.WriteLine(msg);
            //}

            LockTest test11 = new LockTest();
            test11.Start();
        }
    }
}