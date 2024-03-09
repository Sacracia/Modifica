using ModLoader;
using System;
using System.Diagnostics;
using ToastNotifications;


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

    enum InjectorStatus 
    {
        OK = 0,
        NO_PROCESS = 1,
        NO_MONO = 2,
        UNSUPPORTED_MACHINE = 4,
        MONO_EXPORTS_FAIL = 8,
        THREAD_FAIL = 16,
        ACCESS_VIOLATION = 32
    }

    enum ErrorLocation
    {
        NONE = 0,
        GET_ROOT_DOMAIN = 64,
        OPEN_IMAGE_FROM_DATA = 128,
        OPEN_ASSEMBLY_FROM_IMAGE = 256,
        GET_IMAGE_FROM_ASSEMBLY = 512,
        GET_CLASS_FROM_NAME = 1024,
        GET_METHOD_FROM_NAME = 2048,
        GET_CLASS_NAME = 4096,
        RUNTIME_INVOKE = 8192,
        OPEN_ASSEMBLY_FROM = 16384
    }

    public class LoaderLogic
    {
        private static LoaderLogic s_instance = null;
        public LoadConfig etgConfig;

        private LoaderLogic() {
            etgConfig = new LoadConfig("EtG", "C:\\Users\\gleb\\Downloads\\0Harmony1.dll", "C:\\Users\\gleb\\Downloads\\EtGModMenu.dll", "EtGModMenu", "Loader", "Init");
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

        private string StatusToString(int status)
        {
            InjectorStatus injectorStatus = InjectorStatus.OK;
            ErrorLocation errorLocation = ErrorLocation.NONE;
            foreach (int injSt in Enum.GetValues(typeof(InjectorStatus)))
                if ((status & injSt) == injSt)
                    injectorStatus = (InjectorStatus)injSt;
            foreach (int errLoc in Enum.GetValues(typeof(ErrorLocation)))
                if ((status & errLoc) != 0)
                    errorLocation = (ErrorLocation)errLoc;
            if (injectorStatus == InjectorStatus.OK && errorLocation == ErrorLocation.NONE)
                return "Successfully injected";
            return $"{errorLocation}({injectorStatus})";
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
                            AppNotifier.Error(StatusToString(status));
                    }
                    else
                    {
                        AppNotifier.Error($"Dependency:{StatusToString(status)}");
                    }
                }
            }
            else
            {
                AppNotifier.Error("Process not found");
            }
        }

    }
}
