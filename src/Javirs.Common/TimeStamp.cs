using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common
{
    /// <summary>
    /// 时间戳对象
    /// </summary>
    public class TimeStamp
    {
        DateTime _basicTime;
        /// <summary>
        /// 时间戳
        /// </summary>
        public TimeStamp()
        {
            this.LocalDate = DateTime.Now;
            _basicTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            this.Seconds = (this.LocalDate - _basicTime).TotalSeconds;
        }
        private TimeStamp(double seconds)
            : this()
        {
            this.LocalDate = _basicTime.AddSeconds(seconds);
            this.Seconds = seconds;
        }
        private TimeStamp(DateTime time)
            : this()
        {
            this.LocalDate = time;
            this.Seconds = (time - this._basicTime).TotalSeconds;
        }
        /// <summary>
        /// DateTime的隐式转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static implicit operator TimeStamp(DateTime time)
        {
            return new TimeStamp(time);
        }
        /// <summary>
        /// double的隐式转换
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static implicit operator TimeStamp(double seconds)
        {
            return new TimeStamp(seconds);
        }
        /// <summary>
        /// 时间戳到double的隐式转换
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static implicit operator double(TimeStamp ts)
        {
            if (ts == null)
                return 0d;
            return ts.Seconds;
        }
        public double UnixTimeStamp { get { return this.Seconds; } }
        /// <summary>
        /// 秒数
        /// </summary>
        public double Seconds { get; private set; }
        /// <summary>
        /// 时间戳对应的时间
        /// </summary>
        public DateTime LocalDate { get; private set; }
        /// <summary>
        /// UTC时间
        /// </summary>
        public DateTime UtcDate { get { return this.LocalDate.ToUniversalTime(); } }
    }
}
