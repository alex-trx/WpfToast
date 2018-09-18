using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace PowerShill
{
    class Program
    {
        private static string powershell_script_path = @"C:\Work\Powershell\pompom.ps1";
        //private static string powershell_script_path = @"C:\Work\Powershell\mysql-create.ps1";

        static int Main(string[] args)
        {
            if (!File.Exists(powershell_script_path) || args.Length < 1 || !args[0].ToLower().EndsWith("pom.xml"))
            {
                return -1;
            }
            //string ps_script = File.ReadAllText(powershell_script_path);
            System.Console.WriteLine("PowerShill");
            /*using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript(ps_script);
                PowerShellInstance.Invoke();

            }*/
            string additional_params = " ";
            if(args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++) {
                    additional_params += args[i] + " ";
                }
            }
            string pom_path = args[0];
            var p = new Process();
            //p.StartInfo = new ProcessStartInfo("powershell.exe", powershell_script_path + @" C:\vmshare\*\pom.xml")
            //p.StartInfo = new ProcessStartInfo("powershell.exe")

            p.StartInfo = new ProcessStartInfo("powershell.exe", "-noprofile -executionpolicy bypass -file " + powershell_script_path + " " + args[0] + additional_params)


            {
                UseShellExecute = true
            };

            p.Start();
            //System.Diagnostics.Process.Start(@"Y:\Windows\system32\WindowsPowerShell\v1.0\powershell.exe -noprofile -executionpolicy bypass -file " + powershell_script_path);
            return 0;
        }
    }
}
