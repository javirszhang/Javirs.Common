using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common
{
    /// <summary>
    /// 性能监视器
    /// </summary>
    public class PerformanceMonitor
    {
        private List<Checkpoint> _checkpoints;
        private Action<string, TimeSpan> _action;
        /// <summary>
        /// 性能监视器
        /// </summary>
        /// <param name="action"></param>
        public PerformanceMonitor(Action<string, TimeSpan> action)
            : this()
        {
            this._action = action;

        }
        /// <summary>
        /// 性能监视器
        /// </summary>
        public PerformanceMonitor() { InitMembers(); }
        private void InitMembers()
        {
            this._checkpoints = new List<Checkpoint>();
            this._checkpoints.Add(new Checkpoint { Name = "PerformanceMonitor.Begin", Now = DateTime.Now });
        }
        /// <summary>
        /// 创建一个检查点
        /// </summary>
        /// <param name="checkpointName"></param>
        public void CheckPoint(string checkpointName)
        {
            var checkpoint = new Checkpoint();
            checkpoint.Now = DateTime.Now;
            checkpoint.Name = string.IsNullOrEmpty(checkpointName) ? _checkpoints.Count.ToString() : checkpointName;
            _checkpoints.Add(checkpoint);
            if (this._action != null)
            {
                this._action(checkpointName, this._checkpoints[this._checkpoints.Count - 2].Now - checkpoint.Now);
            }
        }
        /// <summary>
        /// 输出监视器内容
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (_checkpoints.Count <= 0)
            {
                return "NO-DATA";
            }
            for (int i = 0; i < this._checkpoints.Count; i++)
            {
                Checkpoint p = this._checkpoints[i];
                builder.Append("[").Append(p.Name.PadLeft(30, (char)0x20)).Append("]");
                builder.Append("[检查点于").Append(p.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append("]");
                if (i != 0)
                {
                    var timespan = p.Now - this._checkpoints[i - 1].Now;
                    builder.Append("[距离上一个检查点耗时").Append(((int)timespan.TotalMilliseconds).ToString().PadLeft(10, (char)0x20)).Append("毫秒]");
                }
                builder.Append(Environment.NewLine);
            }
            return builder.ToString();
        }
        protected class Checkpoint
        {
            public string Name { get; set; }
            public DateTime Now { get; set; }
        }
    }
}
