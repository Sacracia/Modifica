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
    public class LoadConfig
    {
        internal string ProcName { get; private set; }
        internal string HarmonyPath { get; private set; }
        internal string ModPath { get; private set; }
        internal string Nspace { get; private set; }
        internal string Klass { get; private set; }
        internal string Method { get; private set; }
        internal int ProcId { get; set; }
        internal LoadConfig(string _procName, string _harmonyPath, string _modPath, string _nspace, string _klass, string _method)
        {
            ProcName = _procName;
            HarmonyPath = _harmonyPath;
            ModPath = _modPath;
            Nspace = _nspace;
            Klass = _klass;
            Method = _method;
            ProcId = -1;
        }
    }

    public class LoaderLogic
    {
        private static LoaderLogic s_instance = null;
        public LoadConfig etgConfig;

        private LoaderLogic() {
            etgConfig = new LoadConfig("EtG", "C:\\Users\\gleb\\Downloads\\EtGModMenu (1)\\0Harmony.dll", "C:\\Users\\gleb\\Downloads\\EtGModMenu (1)\\EtGModMenu.dll", "EtGModMenu", "Loader", "Init");
        }

        public static LoaderLogic Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new LoaderLogic();
                return s_instance;
            }
        }

        public void Load(LoadConfig cfg)
        {
            Process[] res = Process.GetProcessesByName(cfg.ProcName);
            if (res.Length > 0)
            {
                if (res[0].Id == cfg.ProcId)
                {
                    AppNotifier.Error("Mod already loaded");
                    return;
                }

                using (MonoProcess mp = new MonoProcess(res[0].Id))
                {
                    int status = mp.LoadDependencyFrom(cfg.HarmonyPath);
                    if (status == 0)
                    {
                        status = mp.LoadModFrom(cfg.ModPath, cfg.Nspace, cfg.Klass, cfg.Method);
                        if (status == 0)
                        {
                            AppNotifier.Success("OK");
                            cfg.ProcId = res[0].Id;
                        }
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
