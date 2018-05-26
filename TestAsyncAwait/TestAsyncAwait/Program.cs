using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncAwait
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("{0}: Start testNonAsync..", DateTime.Now);
            TestNonAsync(10);
            Console.WriteLine("{0}: after call testNonAsync..", DateTime.Now);
            // Fire and forget.... dont care
            Console.WriteLine("{0}: Start FireAndForget  this process would take {1} ", DateTime.Now, 10);
            Task.Run(() => FireAndForget(10));
            Console.WriteLine("{0}: Start Async..", DateTime.Now);
            execAsycnMethod(10);
            Console.WriteLine("{0}: After launch myAsync...", DateTime.Now);
            Console.ReadKey();
        }

        private static void TestNonAsync(int delayInSecs)
        {
            Console.WriteLine("start in test Async...{0}", delayInSecs);
            Thread.Sleep(delayInSecs * 1000);
            Console.WriteLine("End in test Async...");
        }

        public static async void execAsycnMethod(int delayInSecs)
        {
            Console.WriteLine("start in MyMethodAsync");
            Task<int> longRunningTask = LongRunningProcessAsync(delayInSecs);

            // independent work which doesn't need the result of LongRunningOperationAsync can be done here
            Console.WriteLine("start independent task: " + DateTime.Now);
            await Task.Delay(3000);

            Console.WriteLine("end independent task: " + DateTime.Now);
            //and now we call await on the task 
            int result = await longRunningTask;
            //use the result 
            Console.ReadKey();

        }

        private static async Task<int> LongRunningProcessAsync(int delayInSecs)
        {
            return await Task<int>.Run(() =>
            {
                while (delayInSecs > 0)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Remaining time .....{0}: {1}", DateTime.Now.ToLocalTime(), delayInSecs);
                    delayInSecs--;
                }

                return 1000;
            });
        }

        private static void FireAndForget(int delayInSecs)
        {

            Thread.Sleep(delayInSecs * 1000);
            return;
        }


        public static async Task<int> LongRunningOperationAsync(int secs) // assume we return an int from this long running operation 
        {
            return await Task.Run(() =>
            {
                Console.WriteLine("start LongRunningOperationAsync " + DateTime.Now);
                var count = 0;
                while (count++ < secs)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("runing : " + DateTime.Now);
                }
                Console.WriteLine("end LongRunningOperationAsync: " + DateTime.Now);
                return secs;

            });
        }
    }
}
