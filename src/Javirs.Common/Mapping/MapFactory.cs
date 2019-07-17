using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
#if netstandard2_0
using Microsoft.AspNetCore.Http;
#endif
namespace Javirs.Common.Mapping
{
    public static class MapFactory
    {
        public static IMapping GetMapping(HttpRequest request)
        {
            return new WebRequestMapping(request);
        }
        public static IMapping GetMapping(DataTable table)
        {
            return new DataTableMapping(table);
        }
        public static IMapping GetMapping(object obj)
        {
            if (obj is DataTable)
            {
                return new DataTableMapping((DataTable)obj);
            }
            if (obj is HttpRequest)
            {
                return new WebRequestMapping((HttpRequest)obj);
            }
            return new ObjectMapping(obj);
        }
    }
}
