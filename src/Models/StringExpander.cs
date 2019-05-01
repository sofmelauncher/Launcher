using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meGaton.src.Models
{
    public static class StringExpander{
        public static string ReplaceNewLineCode(this string txt){
            return txt.Replace("\\n", Environment.NewLine+"");
        }

        public static string ReplaceNewLineCodeAndIndent(this string txt) {
            return txt.Replace("\\n", Environment.NewLine + " ");
        }
    }
}
