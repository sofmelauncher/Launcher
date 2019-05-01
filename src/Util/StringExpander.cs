using System;

namespace meGaton.Util{
    public static class StringExpander{
        public static string ReplaceNewLineCode(this string txt){
            return txt.Replace("\\n", Environment.NewLine+"");
        }

        public static string ReplaceNewLineCodeAndIndent(this string txt) {
            return txt.Replace("\\n", Environment.NewLine + " ");
        }
    }
}
