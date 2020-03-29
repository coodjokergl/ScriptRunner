using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptRunner
{
    public static class Helper
    {
         public static System.Drawing.Rectangle GetRectangle(string ret)
        {
            if(string.IsNullOrEmpty(ret)) return System.Drawing.Rectangle.Empty;
            var pos = ret.Split(',').Select(q=>Convert.ToInt32(q)).ToList();
            return new System.Drawing.Rectangle(new System.Drawing.Point(pos[0],pos[1]),new System.Drawing.Size(pos[2],pos[3]));
        }

         public static System.Drawing.Point? GetPoint(string posVal)
        {
            if(string.IsNullOrEmpty(posVal)) return null;
            var pos = posVal.Split(',').Select(q=>Convert.ToInt32(q)).ToList();
            return new System.Drawing.Point(pos[0],pos[1]);
        }

        public static List<string> Values(string val)
        {
            if(string.IsNullOrEmpty(val)) return new List<string>();

            return val.Split(',').ToList();
        }
    }
}
