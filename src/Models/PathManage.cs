using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meGaton.src.Models
{
    public static class PathManage
    {
        public static readonly string MY_BIN_PATH = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
        public static readonly string GAMES_ROOT_PATH = MY_BIN_PATH+"\\Games";
    }
}
