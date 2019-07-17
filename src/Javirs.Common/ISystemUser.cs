using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common
{
    /// <summary>
    /// 系统用户接口
    /// </summary>
    public interface ISystemUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        int NodeId { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        string NodeCode { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        string NodeName { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// 是否认证用户
        /// </summary>
        bool IsConfirmed { get; set; }
        /// <summary>
        /// 是否企业用户
        /// </summary>
        bool Isenterprise { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        string MobileNo { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        int NodeLevel { get; set; }
    }
}
