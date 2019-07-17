using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Javirs.Common
{
    public class ClassActivitor
    {
        public static object ActivitorInstance(Type type)
        {
            object obj = Activator.CreateInstance(type);
            return obj;
        }

        public static object ActivitorInstance(string assemblyFile, string className)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.LoadFrom(assemblyFile);//不需要重启应用程序池，但是需要指定完整路径
            }
            catch
            {
                assembly = Assembly.Load(assemblyFile);//只需要指定程序集名，需要重启应用程序池
            }
            Type type = assembly.GetType(className);
            return ActivitorInstance(type);
        }
        
    }
}
