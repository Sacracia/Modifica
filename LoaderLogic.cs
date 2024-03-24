﻿using ModificaWPF.Pages;
using ModLoader;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ModificaWPF
{
    public class UserModConfig : INotifyPropertyChanged
    {
        public string Naming { get; set; }
        public int OptionsNumber { get; set; }
        public string Description { get; set; }
        public string ProcName { get; set; }
        public string ModPath { get; set; }
        public string Nspace { get; set; }
        public string Klass { get; set; }
        public string Method { get; set; }
        public int ProcId { get; set; }
        public string HarmonyVersion { get; set; }
        public UserModConfig() { }
        internal UserModConfig(string _name, int _optionsNum, string description, string _procName, string _modPath, string _nspace, string _klass, string _method, string harmonyVersion)
        {
            Naming = _name;
            OptionsNumber = _optionsNum;
            Description = description;
            ProcName = _procName;
            ModPath = _modPath;
            Nspace = _nspace;
            Klass = _klass;
            Method = _method;
            ProcId = -1;
            HarmonyVersion = harmonyVersion;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ModConfig
    {
        public string ProcName { get; private set; }
        public string ModUrl { get; private set; }
        public string HarmonyUrl { get; private set; }
        public string Namespace { get; private set; }
        public string Class { get; private set; }
        public string Method { get; private set; }
        public int ProcId { get; set; }
        public ModConfig(string _procName, string _modUrl, string _harmonyUrl, string _namespace, string _class, string _method)
        {
            ProcName = _procName;
            ModUrl = _modUrl;
            Namespace = _namespace;
            Class = _class;
            Method = _method;
            HarmonyUrl = _harmonyUrl;
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
        public ModConfig etgConfig;
        public UserModConfig[] userConfigs = new UserModConfig[8];

        private LoaderLogic() {
            etgConfig = new ModConfig(
                "EtG", 
                "https://github.com/Sacracia/EtGModMenu/releases/download/v1.0/EtGModMenu.dll",
                "https://github.com/Sacracia/EtGModMenu/releases/download/v1.0/0Harmony.dll", 
                "EtGModMenu", "Loader", "Init");
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

        private int FindFreePosition()
        {
            for (int i = 0; i < 8; ++i)
                if (userConfigs[i] == null)
                    return i;
            return -1;
        }

        private int Count()
        {
            int count = 0;
            for (int i = 0; i < 8; ++i)
                if (userConfigs[i] != null)
                    count++;
            return count;
        }

        public bool AreArgsCorrect(string naming, string optsnum, string desc, string procName, string modPath, string nSpace, string klass, string method, string harmonyVersion)
        {
            return !new List<string> { naming, optsnum, desc, procName, modPath, nSpace, klass, method, harmonyVersion }.Contains(string.Empty);
        }

        public bool AreArgsCorrect(UserModConfig cfg)
        {
            return !new List<string> { cfg.Naming, cfg.OptionsNumber.ToString(), cfg.Description, cfg.ProcName, cfg.ModPath, cfg.Nspace, cfg.Klass, cfg.Method, cfg.HarmonyVersion }.Contains(string.Empty);
        }

        public bool AddConfig(string naming, string optsnum, string desc, string procName, string modPath, string nSpace, string klass, string method, string harmonyVersion)
        {
            if (AreArgsCorrect(naming, optsnum, desc, procName, modPath, nSpace, klass, method, harmonyVersion))
            {
                if (int.TryParse(optsnum, out int val) && App.Current.FindResource("MyModsPage") is MyModsPage myModsPage && myModsPage.counter < 8)
                {
                    int pos = FindFreePosition();
                    if (pos < 0)
                        return false;
                    UserModConfig cfg = new UserModConfig(naming, val, desc, procName, modPath, nSpace, klass, method, harmonyVersion);
                    userConfigs[pos] = cfg;
                    myModsPage.AddCard(cfg, pos);
                    return true;
                }
            }
            return false;
        }

        public bool AddConfig(UserModConfig cfg)
        {
            if (AreArgsCorrect(cfg))
            {
                if (App.Current.FindResource("MyModsPage") is MyModsPage myModsPage && myModsPage.counter < 8)
                {
                    int pos = FindFreePosition();
                    if (pos < 0)
                        return false;
                    userConfigs[pos] = cfg;
                    myModsPage.AddCard(cfg, pos);
                    return true;
                }
            }
            return false;
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

        public async Task Load(ModConfig cfg)
        {
            await Task.Run(() =>
            {
                App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
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
                            AppNotifier.Info("Loading...");
                            byte[] harmonyInBytes = new System.Net.WebClient().DownloadData(cfg.HarmonyUrl);
                            int status = mp.LoadDependency(harmonyInBytes, harmonyInBytes.Length);
                            if (status == 0)
                            {
                                byte[] modInBytes = new System.Net.WebClient().DownloadData(cfg.ModUrl);
                                status = mp.LoadMod(modInBytes, modInBytes.Length, cfg.Namespace, cfg.Class, cfg.Method);
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
                }));
            });
        }

        public void Serialize()
        {
            if (Count() == 0)
                return;
            try
            {
                using (FileStream fs = new FileStream("userMods.json", FileMode.OpenOrCreate))
                {
                    JsonSerializer.Serialize(fs, userConfigs.ToList());
                }
            }
            catch (Exception ex)
            {
                AppNotifier.Error(ex.Message);
            }
        }

        public async void Deserialize()
        {
            try
            {
                if (!File.Exists("userMods.json"))
                    return;
                using (FileStream fs = new FileStream("userMods.json", FileMode.Open))
                {
                    var mods = await JsonSerializer.DeserializeAsync<List<UserModConfig>>(fs);
                    foreach (UserModConfig cfg in mods)
                    {
                        if (cfg != null)
                            AddConfig(cfg);
                    }
                }
            }
            catch (Exception ex) {
                AppNotifier.Error(ex.Message);
            }
        }

    }
}
