using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Javirs.Common.AsynNotify
{
    /// <summary>
    /// 异步通知提供者
    /// </summary>
    public class NotifyProvider
    {
        private const string urlPattern = "(127[.]0[.]0[.]1)|(localhost)|(10[.]\\d{1,3}[.]\\d{1,3}[.]\\d{1,3})|(172[.]((1[6-9])|(2\\d)|(3[01]))[.]\\d{1,3}[.]\\d{1,3})|(192[.]168[.]\\d{1,3}[.]\\d{1,3})";
        private NotifyUnitBase _unit;
        /// <summary>
        /// 异步通知提供者
        /// </summary>
        /// <param name="unit"></param>
        public NotifyProvider(NotifyUnitBase unit)
        {
            this._unit = unit;
        }
        /// <summary>
        /// 发起通知
        /// </summary>
        /// <returns></returns>
        public bool Notify()
        {
            try
            {
                if (this._unit == null)
                {
                    OnAlert("订单对象不能为空！");
                    return false;
                }
                if (!this._unit.CanNotify())
                {
                    OnAlert("当前订单不允许通知！");
                    return false;
                }
                if (Regex.IsMatch(this._unit.NotifyUrl, urlPattern, RegexOptions.IgnoreCase))
                {
                    OnAlert("通知地址属于内网，系统放弃通知！");
                    this._unit.AsynNotifyResult(false, false, "通知地址属于内网，系统放弃通知");
                    return false;
                }
                if (!this._unit.NotifyUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    OnAlert("无效的通知地址，系统放弃通知！");
                    this._unit.AsynNotifyResult(false, false, "无效的通知地址，系统放弃通知");
                    return false;
                }
                Net.HttpHelper helper = new Net.HttpHelper(this._unit.NotifyUrl);
                string respstr = "";
                if (this._unit.Method == HttpMethod.GET)
                {
                    respstr = helper.Get(this._unit.CreateQueryString());
                }
                else
                {
                    respstr = helper.Post(this._unit.CreateQueryString(), false);
                }
                OnAlert(respstr);
                bool notifyresult = helper.StatusCode == 200;
                if (!notifyresult)
                {
                    OnAlert("获得http错误，错误码：" + helper.StatusCode);
                }
                if (string.IsNullOrEmpty(respstr))
                {
                    respstr = "未获取商户系统响应";
                }
                if (respstr.Length > 100)
                    respstr = respstr.Substring(0, 100);
                if (!this._unit.AsynNotifyResult(respstr.StartsWith("SUCCESS", StringComparison.OrdinalIgnoreCase), true, respstr))
                {
                    string info = string.Format("通知{0},更新数据失败", (notifyresult ? "成功" : "失败"));
                    OnAlert(info);
                    return false;
                }
                return notifyresult;
            }
            catch (Exception ex)
            {
                OnAlert(string.Format("通知异常！\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
        /// <summary>
        /// 错误信息上抛
        /// </summary>
        public event Action<string> Alert;
        /// <summary>
        /// 事件通知
        /// </summary>
        /// <param name="obj"></param>
        protected void OnAlert(string obj)
        {
            if (Alert != null)
            {
                Alert(obj);
            }
        }
    }
}
