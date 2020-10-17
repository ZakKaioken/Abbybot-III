using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Mysql
{
    class MysqlCore
    {
        

       public static string mysqlbinpath;
        public static async Task CheckMysql(string localpath)
        {
            Load(localpath);
            Process[] mysqls = Process.GetProcessesByName("mysqld");
            if (mysqls.Length > 0)
                return;
            //mysql\bin\mysqld --defaults-file=mysql\bin\my.ini --standalone

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = @$"{mysqlbinpath}\mysqld.exe",
                WorkingDirectory = mysqlbinpath,
                Arguments = @$" --defaults-file={mysqlbinpath}\my.ini --standalone",
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi);
        }

        public static void Save(string path)
        {
            File.WriteAllText(path, mysqlbinpath);
        }

        public static void Load(string path)
        {
            mysqlbinpath = File.ReadAllText(path);
        }

    }
}
