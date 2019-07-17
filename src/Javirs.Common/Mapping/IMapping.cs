using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Mapping
{
    public interface IMapping
    {
        T Map<T>() where T : class;
        void Map<T>(T t) where T : class;        
    }
}
