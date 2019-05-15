using System;
using System.Collections.Generic;

namespace debug_analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            ComandLineParser clp = new ComandLineParser(args);
            List<FileAnalyze> files = clp.GetFile();
            int threadCount = clp.WorkingType();
            ThreadController tc = new ThreadController(threadCount, threadCount);
            tc.AddTasks(files);
            tc.WaitAllTasks();

            foreach (FileAnalyze f in files)
            {
                Console.WriteLine(f.ToString());
            }
            Console.WriteLine(tc.ToString());

            Console.ReadLine();
        }
    }
}
