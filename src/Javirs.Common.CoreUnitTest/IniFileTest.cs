using Javirs.Common.IO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Javirs.Common.CoreUnitTest
{
    public class IniFileTest
    {
        [Test]
        public void ReadTest()
        {
            string path = @"G:\Jason\documents\deployassistant.ini";
            IniFile file = IniFile.Read(path);
            var folder = file.GetSection("workfolder", true);
            folder.Properties.Add(new IniProperty() { Name = "test", Value = "d:\\web",Comments=new string[2] { "this is a test","test"} });
            file.Save();
            Assert.IsTrue(folder == null);
        }
    }
    
    
    
}
