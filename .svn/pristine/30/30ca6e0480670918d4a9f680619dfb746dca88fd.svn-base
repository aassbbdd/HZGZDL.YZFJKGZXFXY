using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    /// <summary>
    /// 时间帮助工具类
    /// @Author ZHUWEI
    /// @Version 1.0 
    /// </summary>
    public class DateTimeUtil
    {
        #region 根据时间返回几个月前,几天前,几小时前,几分钟前,几秒前
        //// <summary>
        /// 根据时间返回几个月前,几天前,几小时前,几分钟前,几秒前
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }

        #endregion

        #region 时间差计算

        /// <summary>
        /// 计算两个时间的差值,返回的是x天x小时x分钟x秒,DateTime2 - DateTime1
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            if (DateTime1 >= DateTime2)
            {
                return "0 秒";
            }

            StringBuilder builder = new StringBuilder();
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts2.Subtract(ts1).Duration();
            //TimeSpan ts=ts1.Add(ts2).Duration();
            int days = ts.Days;
            int hours = ts.Hours;
            int minutes = ts.Minutes;
            int seconds = ts.Seconds;

            if (days > 0)
            {
                builder.Append(days + "天");
            }
            if (hours > 0)
            {
                builder.Append(hours + "小时");
            }
            if (minutes > 0)
            {
                builder.Append(minutes + "分钟");
            }
            if (seconds > 0)
            {
                builder.Append(seconds + "秒");
            }
            return builder.ToString();
        }

        /// <summary>
        /// 时间相差值,返回时间差
        /// 调用时,isTotal为true时,返回的时带小数的天数,否则返回的是整数
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <param name="isTotal"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2, bool isTotal)
        {
            TimeSpan ts = DateTime2 - DateTime1;
            if (isTotal)
                //带小数的天数，比如1天12小时结果就是1.5 
                return ts.TotalDays.ToString();
            else
                //整数天数，1天12小时或者1天20小时结果都是1 
                return ts.Days.ToString();
        }
        /// <summary>
        /// 时间相差值,返回时间差 dateTime2 - dateTime1
        /// 
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <param name="isTotal"></param>
        /// <returns>毫秒数</returns>
        public static long DateTimeDiff(DateTime dateTime1, DateTime dateTime2)
        {
            return (long)dateTime2.Subtract(dateTime1).TotalMilliseconds;
            //TimeSpan ts = dateTime2 - dateTime1;
            //return ts.Milliseconds;
        }
        #endregion
        #region 日期比较
        /**/
        /// <summary>
        /// 日期比较
        /// </summary>
        /// <param name="today">当前日期</param>
        /// <param name="writeDate">输入日期</param>
        /// <param name="n">比较天数</param>
        /// <returns>大于天数返回true，小于返回false</returns>
        public static bool CompareDate(string today, string writeDate, int n)
        {
            DateTime Today = Convert.ToDateTime(today);
            DateTime WriteDate = Convert.ToDateTime(writeDate);
            WriteDate = WriteDate.AddDays(n);
            if (Today >= WriteDate)
                return false;
            else
                return true;
        }
        #endregion
        #region 根据英文的星期几返回中文的星期几
        /**/
        /// <summary>
        /// 根据英文的星期几返回中文的星期几
        /// 如WhichDay("Sunday"),返回星期日
        /// </summary>
        /// <param name="enWeek"></param>
        /// <returns></returns>
        public static string WhichDay(string enWeek)
        {
            switch (enWeek.Trim())
            {
                case "Sunday":
                    return "星期日";
                case "Monday":
                    return "星期一";
                case "Tuesday":
                    return "星期二";
                case "Wednesday":
                    return "星期三";
                case "Thursday":
                    return "星期四";
                case "Friday":
                    return "星期五";
                case "Saturday":
                    return "星期六";
                default:
                    return enWeek;
            }
        }
        #endregion
        #region 生日提醒
        /**/
        /// <summary>
        /// 根据出生年月进行生日提醒
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public static string GetBirthdayTip(DateTime birthday)
        {
            DateTime now = DateTime.Now;
            //TimeSpan span = DateTime.Now - birthday;
            int nowMonth = now.Month;
            int birtMonth = birthday.Month;
            if (nowMonth == 12 && birtMonth == 1)
                return string.Format("下月{0}号", birthday.Day);
            if (nowMonth == 1 && birtMonth == 12)
                return string.Format("上月{0}号", birthday.Day);
            int months = now.Month - birthday.Month;
            //int days = now.Day - birthday.Day;
            if (months == 1)
                return string.Format("上月{0}号", birthday.Day);
            else if (months == -1)
                return string.Format("下月{0}号", birthday.Day);
            else if (months == 0)
            {
                if (now.Day == birthday.Day)
                    return "今天";
                return string.Format("本月{0}号", birthday.Day);
            }
            else if (months > 1)
                return string.Format("已过{0}月", months);
            else
                return string.Format("{0}月{1}日", birthday.Month, birthday.Day);
        }
        #endregion

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <returns></returns>
        public static string GetNowDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
