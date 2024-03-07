using ModLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ModificaWPF
{
    internal class LoaderLogic
    {
        private static LoaderLogic s_instance = null;

        private LoaderLogic() {}

        public static LoaderLogic Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new LoaderLogic();
                return s_instance;
            }
        }

        public void Load()
        {
            Process[] res = Process.GetProcessesByName("EtG");
            if (res.Length > 0)
            {
                using (MonoProcess mp = new MonoProcess(res[0].Id))
                {
                    int status = mp.LoadDependencyFrom("C:\\Users\\gleb\\Downloads\\Harmony.2.2.2.0\\net35\\0Harmony.dll");
                    if (status == 0)
                    {
                        status = mp.LoadModFrom("D:\\NIISI\\cheats\\GitHub\\ModificaWPF\\bin\\x64\\Release\\EtGModMenu.dll", "EtGModMenu", "Loader", "Init");
                        if (status == 0)
                            AppNotifier.Success("OK");
                        else
                            AppNotifier.Error(status.ToString());
                    }
                }
            }
            else
            {
                AppNotifier.Error("Game not found");
            }
        }

    }
}
