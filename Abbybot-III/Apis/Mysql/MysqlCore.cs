using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Mysql
{
    class MysqlCore
    {
        public static string mysqlbinpath;

        public static async Task CheckMysql(string localpath)
        {
            Process[] mysqls = Process.GetProcessesByName("mysqld");
            if (mysqls.Length > 0)
                return;

            Load(localpath);
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
            await Task.CompletedTask;
        }

        public static void Save(string path)
        {
            File.WriteAllText(path, mysqlbinpath);
        }

        public static void Load(string path)
        {
            var dir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var e = Path.GetFullPath(path).Replace(fileName, "");
            if (!Directory.Exists(e)) Directory.CreateDirectory(e);
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "Replace this text with the path to your mysql bin folder.");
            }

            var text = File.ReadAllText(path);
            mysqlbinpath = text;
            if (text.Contains("Replace") && text.Contains("bin folder"))
            {
                Abbybot.print($"AbbybotMemory: Abbybot's memory is not running and I can't find where abbybot's memory is to turn it back on... Did you forget to give me it? ({fileName} in {dir})");
            }
        }
    }
}