using System;
using System.Diagnostics;

namespace debug_analyzer
{
    class FileAnalyze
    {
        private String _fileName;
        private int _outputTraffic;
        private int _inputTraffic;
        private int _sumTraffic;
        private TimeSpan _minResponseTime;
        private TimeSpan _maxResponseTime;
        private TimeSpan _analyzeTime;

        public FileAnalyze(String fileName)
        {
            _fileName = fileName;
        }
        
        protected void CalculateOutputTraffic()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _outputTraffic = DataHandler.OutsideTraffic(_fileName);
            stopwatch.Stop();
            _analyzeTime.Add(stopwatch.Elapsed);
        }

        protected void CalculateInputTraffic()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _inputTraffic = DataHandler.InsideTraffic(_fileName);
            stopwatch.Stop();
            _analyzeTime += stopwatch.Elapsed;
        }

        protected void CalculateSumTraffic()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _sumTraffic = _inputTraffic + _outputTraffic;
            stopwatch.Stop();
            _analyzeTime += stopwatch.Elapsed;
        }

        protected void CalculateMinResponseTime()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _minResponseTime = DataHandler.minResponseTime(_fileName);
            stopwatch.Stop();
            _analyzeTime += stopwatch.Elapsed;
        }

        protected void CalculateMaxResponseTime()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _maxResponseTime = DataHandler.maxResponseTime(_fileName);
            stopwatch.Stop();
            _analyzeTime += stopwatch.Elapsed;
        }

        public void Analyze()
        {
            CalculateInputTraffic();
            CalculateOutputTraffic();
            CalculateSumTraffic();
            CalculateMinResponseTime();
            CalculateMaxResponseTime();
        }

        public override string ToString()
        {
            String formatTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                              _analyzeTime.Hours, _analyzeTime.Minutes, _analyzeTime.Seconds,
                                              _analyzeTime.Milliseconds / 10);
            String formatMaxTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                              _maxResponseTime.Hours, _maxResponseTime.Minutes, _maxResponseTime.Seconds,
                                              _maxResponseTime.Milliseconds / 10);
            String formatMinTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                              _minResponseTime.Hours, _minResponseTime.Minutes, _minResponseTime.Seconds,
                                              _minResponseTime.Milliseconds / 10);
            return "File name: " + _fileName +
                   "\nInput: " + _inputTraffic.ToString() +
                   "\nOutput: " + _outputTraffic.ToString() +
                   "\nSum: " + _sumTraffic.ToString() +
                   "\nMin response time: " + formatMinTime +
                   "\nMax response time: " + formatMaxTime +
                   "\nAnalyze time: " + formatTime + " \n";
        }
    }
}
