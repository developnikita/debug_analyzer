using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Globalization;

namespace debug_analyzer
{
    class DataHandler
    {
        private const string dateTimePattern = "(0[1-9]|1[0-9]|2[0-9]|3[01])\\.(0[1-9]|1[012])\\.(20\\d\\d)\\s" +
                                                    "(0|[1-9]|0[1-9]|1[0-9]|2[0-4]):(0|00|0[1-9]|[1-5][0-9]|60):(\\d{2}\\.\\d{3})";
        private static string dateTimeFormat = "dd/MM/yyyy H:mm:ss.fff";

        protected static string formatDateString(String dateTime)
        {
            String[] splitDate = dateTime.Split(' ');
            splitDate[0] = splitDate[0].Replace('.', '/');
            return String.Join(" ", splitDate);
        }

        public static TimeSpan minResponseTime(String path)
        {
            TimeSpan minTime = TimeSpan.MaxValue;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string line;
                DateTime temp = new DateTime();
                DateTime sendTime = new DateTime();
                DateTime responseTime = new DateTime();
                while ((line = sr.ReadLine()) != null)
                {
                    Match match = Regex.Match(line, dateTimePattern);
                    if (match.Length != 0)
                    {
                        string t = formatDateString(match.Value);
                        temp = DateTime.ParseExact(t, dateTimeFormat, CultureInfo.InvariantCulture);
                    }
                    if (line.Contains("<-- Отправлен запрос"))
                    {
                        sendTime = temp;
                    }
                    if (line.Contains("--> Получен ответ"))
                    {
                        responseTime = temp;
                    }
                    if (!sendTime.Equals(new DateTime()) && !responseTime.Equals(new DateTime()))
                    {
                        TimeSpan ts = responseTime - sendTime;
                        if (minTime.CompareTo(ts) == 1)
                        {
                            minTime = ts;
                        }
                        sendTime = new DateTime();
                        responseTime = new DateTime();
                    }
                }
            }
            return minTime;
        }

        public static TimeSpan maxResponseTime(String path)
        {
            TimeSpan maxTime = TimeSpan.MinValue;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string line;
                DateTime temp = new DateTime();
                DateTime sendTime = new DateTime();
                DateTime responseTime = new DateTime();
                while ((line = sr.ReadLine()) != null)
                {
                    Match match = Regex.Match(line, dateTimePattern);
                    if (match.Length != 0)
                    {
                        string t = formatDateString(match.Value);
                        temp = DateTime.ParseExact(t, dateTimeFormat, CultureInfo.InvariantCulture);
                    }
                    if (line.Contains("<-- Отправлен запрос"))
                    {
                        sendTime = temp;
                    }
                    if (line.Contains("--> Получен ответ"))
                    {
                        responseTime = temp;
                    }
                    if (!sendTime.Equals(new DateTime()) && !responseTime.Equals(new DateTime()))
                    {
                        TimeSpan ts = responseTime - sendTime;
                        if (maxTime.CompareTo(ts) == -1)
                        {
                            maxTime = ts;
                        }
                        sendTime = new DateTime();
                        responseTime = new DateTime();
                    }
                }
            }
            return maxTime;
        }

        public static int InsideTraffic(String path)
        {
            int sum = 0;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.IndexOf("<-- Отправлен запрос") != -1)
                    {
                        MatchCollection match = Regex.Matches(line, @"(\d+)");

                        int value;
                        string resultString = string.Join(string.Empty, Regex.Matches(line, @"(\d+)").OfType<Match>().Select(m => m.Value));
                        int.TryParse(resultString, out value);
                        sum += value;
                    }
                }
            }
            return sum;
        }

        public static int OutsideTraffic(String path)
        {
            int sum = 0;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.IndexOf("--> Получен ответ") != -1)
                    {
                        int value;
                        int.TryParse(string.Join("", line.Where(c => char.IsDigit(c))), out value);
                        sum += value;
                    }
                }
            }
            return sum;
        }
    }
}
