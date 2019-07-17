using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Javirs.Common
{
    /// <summary>
    /// 身份证
    /// </summary>
    public class IDCard
    {
        private string _cardno;
        /// <summary>
        /// 使用身份证号码初始化
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentException">非18位身份证异常</exception>
        /// <exception cref="System.ArgumentNullException">身份证号码传空异常</exception>
        /// <param name="cardno"></param>
        public IDCard(string cardno)
        {
            this._cardno = cardno;
            Load();
        }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public UserSex Sex { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 所属省份
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 性别枚举
        /// </summary>
        public enum UserSex
        {
            /// <summary>
            /// Female
            /// </summary>
            女 = 0,
            /// <summary>
            /// Male
            /// </summary>
            男 = 1
        }
        private void Load()
        {
            if (string.IsNullOrEmpty(this._cardno))
                throw new ArgumentNullException("cardno");
            if (_cardno.Length < 18)
                throw new ArgumentException("不是18位的二代身份证号码");
            this.IsValid = IdValid(this._cardno);
            if (this.IsValid)
            {
                this.ProvinceName = provinceCode[_cardno.Substring(0, 2)];
                this.Birthday = Convert.ToDateTime(Regex.Replace(this._cardno.Substring(6, 8), @"(\d{4})(\d{2})(\d{2})", "$1-$2-$3"));
                this.Sex = int.Parse(this._cardno[16].ToString()) % 2 == 0 ? UserSex.女 : UserSex.男;
            }
        }
        private static readonly Dictionary<string, string> provinceCode = new Dictionary<string, string>();
        private static bool IdValid(string idcardno)
        {
            string[] validcode = new string[] { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            int sum = 0;
            for (int i = 17; i > 0; i--)
                sum += (int)Math.Pow(2, i) * Convert.ToInt32(idcardno[17 - i].ToString());
            int result = sum % 11;
            return validcode[result].Equals(idcardno[17].ToString(), StringComparison.OrdinalIgnoreCase);
        }
        static IDCard()
        {
            provinceCode.Add("11", "北京");
            provinceCode.Add("12", "天津");
            provinceCode.Add("13", "河北");
            provinceCode.Add("14", "山西");
            provinceCode.Add("15", "内蒙古");
            provinceCode.Add("21", "辽宁");
            provinceCode.Add("22", "吉林");
            provinceCode.Add("23", "黑龙江");
            provinceCode.Add("31", "上海");
            provinceCode.Add("32", "江苏");
            provinceCode.Add("33", "浙江");
            provinceCode.Add("34", "安徽");
            provinceCode.Add("35", "福建");
            provinceCode.Add("36", "江西");
            provinceCode.Add("37", "山东");
            provinceCode.Add("41", "河南");
            provinceCode.Add("42", "湖北");
            provinceCode.Add("43", "湖南");
            provinceCode.Add("44", "广东");
            provinceCode.Add("45", "广西");
            provinceCode.Add("46", "海南");
            provinceCode.Add("50", "重庆");
            provinceCode.Add("51", "四川");
            provinceCode.Add("52", "贵州");
            provinceCode.Add("53", "云南");
            provinceCode.Add("54", "西藏");
            provinceCode.Add("61", "陕西");
            provinceCode.Add("62", "甘肃");
            provinceCode.Add("63", "青海");
            provinceCode.Add("64", "宁夏");
            provinceCode.Add("65", "新疆");
            provinceCode.Add("71", "台湾");
            provinceCode.Add("81", "香港");
            provinceCode.Add("82", "澳门");
            provinceCode.Add("91", "国外");
        }
        /// <summary>
        /// 已重载，打印格式 “身份证号码:{0},是否有效:{4},所在省份:{1},性别:{2},出生日期:{3}”
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("身份证号码:{0},是否有效:{4},所在省份:{1},性别:{2},出生日期:{3}", this._cardno, this.ProvinceName, this.Sex.ToString(), this.Birthday.ToShortDateString(), this.IsValid);
        }
    }
}
