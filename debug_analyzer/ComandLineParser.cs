using System;
using System.Collections.Generic;

namespace debug_analyzer
{
    class ComandLineParser
    {
        private String[] _args;

        public ComandLineParser(String[] args)
        {
            _args = args;
        }
        
        public int WorkingType()
        {
            if (_args[0].Equals("single"))
            {
                return 1;
            } else
            {
                return Convert.ToInt32(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));
            }
        }

        public List<FileAnalyze> GetFile()
        {
            List<FileAnalyze> files = new List<FileAnalyze>();
            for (int i = 1; i < _args.Length; i++)
            {
                files.Add(new FileAnalyze(_args[i]));
            }
            return files;
        }
    }
}
