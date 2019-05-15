using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace debug_analyzer
{
    class ThreadController
    {
        private List<Task> tasks;
        private TimeSpan workingTime;

        public ThreadController(int minThreadCount, int maxThreadCount)
        {
            tasks = new List<Task>();
            workingTime = new TimeSpan();
            ThreadPool.SetMinThreads(minThreadCount, minThreadCount);
            ThreadPool.SetMaxThreads(maxThreadCount, maxThreadCount);
        }

        public static void SetThreadController(int minThreadCount, int maxThreadCount)
        {
            ThreadPool.SetMinThreads(minThreadCount, minThreadCount);
            ThreadPool.SetMaxThreads(maxThreadCount, maxThreadCount);
        }

        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public void AddTasks(List<FileAnalyze> files)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (FileAnalyze f in files)
            {
                AddTask(Task.Factory.StartNew(() => f.Analyze()));
            }
            stopwatch.Stop();
            workingTime += stopwatch.Elapsed;
        }

        public void WaitAllTasks()
        {
            Stopwatch stopwathc = new Stopwatch();
            stopwathc.Start();
            Task.WaitAll(tasks.ToArray());
            stopwathc.Stop();
            workingTime += stopwathc.Elapsed;
        }

        public override string ToString()
        {
            String workingTimeFormat = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                              workingTime.Hours, workingTime.Minutes, workingTime.Seconds,
                                              workingTime.Milliseconds / 10);
            return "Comulative file analyze time: " + workingTimeFormat;
        }
    }
}
