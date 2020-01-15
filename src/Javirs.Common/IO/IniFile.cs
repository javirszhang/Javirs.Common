using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Javirs.Common.IO
{
    /// <summary>
    /// ini文件操作
    /// </summary>
    public class IniFile
    {
        private readonly string path;
        private IniFile(string path)
        {
            this.path = path;
            this.Sections = new List<IniSection>();
        }
        /// <summary>
        /// 读取ini文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IniFile Read(string path)
        {
            var file = new IniFile(path);
            file.Init();
            return file;
        }
        private void Init()
        {
            if (!File.Exists(path))
            {
                return;
            }
            string[] lines = File.ReadAllLines(this.path);
            if (lines == null || lines.Length <= 0)
            {
                return;
            }
            Queue<string> commentQueue = new Queue<string>();
            foreach (string line in lines)
            {
                string s = line?.Trim();
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }
                if (s.StartsWith("#"))
                {
                    commentQueue.Enqueue(s.Substring(1)?.Trim());
                }
                else if (s.StartsWith("["))
                {
                    IniSection section = new IniSection();
                    section.Name = Regex.Replace(s, @"^\[([^\]]+)\].*$", "$1");
                    List<string> comments = new List<string>();
                    while (commentQueue.Count > 0)
                    {
                        comments.Add(commentQueue.Dequeue());
                    }
                    if (comments.Count > 0)
                    {
                        section.Comments = comments.ToArray();
                    }
                    Sections.Add(section);
                }
                else
                {
                    IniSection section = Sections.Count == 0 ? new IniSection() : Sections.Last();
                    if (string.IsNullOrEmpty(section.Name))
                    {
                        section.Name = "default";
                    }
                    if (Sections.Count == 0) Sections.Add(section);

                    if (section.Properties == null) section.Properties = new List<IniProperty>();

                    var prop = new IniProperty(s);

                    section.Properties.Add(prop);
                    List<string> comments = new List<string>();
                    while (commentQueue.Count > 0) comments.Add(commentQueue.Dequeue());
                    if (comments.Count > 0) prop.Comments = comments.ToArray();
                }
            }
        }
        /// <summary>
        /// 所有配置
        /// </summary>
        public List<IniSection> Sections { get; set; } = new List<IniSection>();
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="createIfNotExist"></param>
        /// <returns></returns>
        public IniSection GetSection(string name, bool createIfNotExist = false)
        {
            var section = this.Sections.Find(it => it.Name == name);
            if (section == null && createIfNotExist)
            {
                section = new IniSection();
                section.Name = name;
                this.Sections.Add(section);
            }
            return section;
        }
        /// <summary>
        /// 保存到文件
        /// </summary>
        public void Save()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var sec in Sections)
            {
                builder.Append(sec.ToString(true));
                builder.AppendLine();
            }
            File.WriteAllText(path, builder.ToString());
        }
    }
}
