using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common
{
    public class vCard
    {
        private Dictionary<string, string> _dictionary;
        public vCard()
        {
            _dictionary = new Dictionary<string, string>();
        }
        public void Put(string key, string value)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = value;
            }
            else
            {
                _dictionary.Add(key, value);
            }
        }
        public void Put(vCard.Key key, string value)
        {
            Put(key.KeyName, value);
        }
        /*
         BEGIN:VCARD
         VERSION:3.0
         FN:任侠
         TEL;CELL;VOICE:15201280000
         TEL;WORK;VOICE:010-62100000
         TEL;WORK;FAX:010-62100001
         EMAIL;PREF;INTERNET:lzw#lzw.me
         URL:http://lzw.me
         orG:志文工作室
         ROLE:产品部
         TITLE:CTO
         ADR;WORK;POSTAL:北京市朝阳区北四环中路35号;100101
         REV:2012-12-27T08:30:02Z
         END:VCARD
         */
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("BEGIN:VCARD");
            builder.AppendLine("VERSION:3.0");
            foreach (string key in _dictionary.Keys)
            {
                builder.AppendLine(key + ":" + _dictionary[key]);
            }
            builder.AppendLine("REV:" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            builder.AppendLine("END:VCARD");
            return builder.ToString();
        }
        public class Key
        {
            protected Key(string name)
            {
                KeyName = name;
            }
            internal string KeyName { get; set; }
            public static implicit operator string(Key key)
            {
                return key.KeyName;
            }
            public static Key FirstName = new Key("FN");
            public static Key Mobile = new Key("TEL;CELL;VOICE");
            public static Key WorkPhone = new Key("TEL;WORK;VOICE");
            public static Key FAX = new Key("TEL;WORK;FAX");
            public static Key EMAIL = new Key("EMAIL;PREF;INTERNET");
            public static Key WEBSITE = new Key("URL");
            public static Key ORG = new Key("ORG");
            public static Key ROLE = new Key("ROLE");
            public static Key TITLE = new Key("TITLE");
            public static Key ADDR = new Key("ADR;WORK;POSTAL");
        }
    }
}
