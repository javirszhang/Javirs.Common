using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Javirs.Common
{
    /// <summary>
    /// 人民币
    /// </summary>
    [DebuggerDisplay("{Amount} {Unit}")]
    public struct CNY
    {
        /// <summary>
        /// 人民币
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="unit"></param>
        public CNY(decimal amount, CnyUnit unit)
        {
            this.Unit = unit;
            this.Amount = amount;
        }
        /// <summary>
        /// 货币单位
        /// </summary>
        public CnyUnit Unit { get; private set; }
        /// <summary>
        /// 金额数值
        /// </summary>
        public decimal Amount { get; private set; }
        /// <summary>
        /// 转换到指定单位
        /// </summary>
        /// <param name="toUnit"></param>
        /// <returns></returns>
        public CNY ConvertTo(CnyUnit toUnit)
        {
            if (this.Unit == toUnit)
            {
                return this;
            }
            if (this.Unit == CnyUnit.分)
            {
                return new CNY { Unit = toUnit, Amount = this.Amount / 100m };
            }
            return new CNY { Unit = toUnit, Amount = this.Amount * 100m };
        }
        public override int GetHashCode()
        {
            return Unit.GetHashCode() ^ this.Amount.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            CNY target = (CNY)obj;
            var to = this.ConvertTo(target.Unit);
            return to.Amount == target.Amount;
        }
        public override string ToString()
        {
            return this.Amount.ToString();
        }
        public string ToString(string format, bool withUnit)
        {
            var val = string.Concat(this.Amount.ToString(format));
            if (withUnit)
            {
                val += " " + this.Unit.ToString();
            }
            return val;
        }
        /// <summary>
        /// 判等
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool operator ==(CNY c1, CNY c2)
        {
            return c1.Equals(c2);
        }
        /// <summary>
        /// 判不等
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool operator !=(CNY c1, CNY c2)
        {
            return !c1.Equals(c2);
        }
        /// <summary>
        /// 两个货币相加
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns>返回以c1单位的货币对象</returns>
        public static CNY operator +(CNY c1, CNY c2)
        {
            var tmp = c2.ConvertTo(c1.Unit);
            return new CNY(c1.Amount + tmp.Amount, c1.Unit);
        }
        /// <summary>
        /// 两个货币相减，注意c1 减去 c2的结果不允许小于零。
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns>返回以c1单位的货币对象</returns>
        public static CNY operator -(CNY c1, CNY c2)
        {
            var tmp = c2.ConvertTo(c1.Unit);
            if (c1.Amount < tmp.Amount)
            {
                throw new ApplicationException("两个货币相减的结果不允许小于零");
            }
            return new CNY(c1.Amount - tmp.Amount, c1.Unit);
        }
    }

    public enum CnyUnit
    {
        分 = 1,
        元 = 2
    }
}
