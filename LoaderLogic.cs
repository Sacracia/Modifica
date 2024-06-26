﻿using ModificaWPF.Pages;
using ModLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZeroFormatter;

namespace ModificaWPF
{
    [ZeroFormattable]
    public class UserModConfig : INotifyPropertyChanged
    {
        [Index(0)]
        public virtual string Naming { get; set; }
        [Index(1)]
        public virtual int OptionsNumber { get; set; }
        [Index(2)]
        public virtual string Description { get; set; }
        [Index(3)]
        public virtual string ProcName { get; set; }
        [Index(4)]
        public virtual string ModPath { get; set; }
        [Index(5)]
        public virtual string Nspace { get; set; }
        [Index(6)]
        public virtual string Klass { get; set; }
        [Index(7)]
        public virtual string Method { get; set; }
        [Index(8)]
        public virtual int ProcId { get; set; }
        [Index(9)]
        public virtual string HarmonyVersion { get; set; }
        [Index(10)]
        public virtual int PosInArr { get; set; }
        [Index(11)]
        [IgnoreFormat]
        public virtual int CardPos { get; set; }
        public UserModConfig() { }
        internal UserModConfig(string _name, int _optionsNum, string description, string _procName, string _modPath, string _nspace, string _klass, string _method, string harmonyVersion, int posInArr)
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
            PosInArr = posInArr;
        }

        public UserModConfig(UserModConfig cfg)
        {
            Naming = cfg.Naming;
            OptionsNumber = cfg.OptionsNumber;
            Description = cfg.Description;
            ProcName = cfg.ProcName;
            ModPath = cfg.ModPath;
            Nspace = cfg.Nspace;
            Klass = cfg.Klass;
            Method = cfg.Method;
            ProcId = -1;
            HarmonyVersion = cfg.HarmonyVersion;
            PosInArr = cfg.PosInArr;
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
        public UserModConfig[] userConfigs = new UserModConfig[8];
        public ModConfig EtgConfig { get; private set; }
        public ModConfig TE2Config { get; private set; }

        private LoaderLogic()
        {
            EtgConfig = new ModConfig(
                "EtG",
                "https://github.com/Sacracia/EtGModMenu/releases/download/v1.0/EtGModMenu.dll",
                "https://github.com/Sacracia/EtGModMenu/releases/download/v1.0/0Harmony.dll",
                "EtGModMenu", "Loader", "Init");
            TE2Config = new ModConfig(
                "TheEscapists2",
                "https://github.com/Sacracia/TE2ModMenu/releases/download/v1.0/TE2ModMenu.dll",
                "https://github.com/Sacracia/TE2ModMenu/releases/download/v1.0/0Harmony.dll",
                "TE2ModMenu", "Loader", "Init");
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

        public bool AreArgsCorrect(string naming, string optsnum, string desc, string procName, string modPath, string nSpace, string klass, string method)
        {
            return !new List<string> { naming, optsnum, desc, procName, modPath, nSpace, klass, method }.Contains(string.Empty);
        }

        public bool AreArgsCorrect(UserModConfig cfg)
        {
            return !new List<string> { cfg.Naming, cfg.OptionsNumber.ToString(), cfg.Description, cfg.ProcName, cfg.ModPath, cfg.Nspace, cfg.Klass, cfg.Method, cfg.HarmonyVersion }.Contains(string.Empty);
        }

        public bool AddConfig(string naming, string optsnum, string desc, string procName, string modPath, string nSpace, string klass, string method, string harmonyVersion)
        {
            if (AreArgsCorrect(naming, optsnum, desc, procName, modPath, nSpace, klass, method))
            {
                if (int.TryParse(optsnum, out int val) && App.Current.FindResource("MyModsPage") is MyModsPage myModsPage && myModsPage.counter < 8)
                {
                    int pos = FindFreePosition();
                    if (pos < 0)
                        return false;
                    UserModConfig cfg = new UserModConfig(naming, val, desc, procName, modPath, nSpace, klass, method, harmonyVersion, pos);
                    userConfigs[pos] = cfg;
                    cfg.PosInArr = pos;
                    myModsPage.AddCard(cfg);
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
                    cfg.PosInArr = pos;
                    myModsPage.AddCard(cfg);
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

        public async Task<(int, string)> Load(ModConfig cfg)
        {
            Process[] res = Process.GetProcessesByName(cfg.ProcName);
            if (res.Length == 0)
                return (1, "Process not found");
            if (res[0].Id == cfg.ProcId)
            {
                return (1, "Mod already loaded");
            }
            AppNotifier.Info("Loading...");
            using (MonoProcess mp = new MonoProcess(res[0].Id))
            {
                byte[] harmonyInBytes = await Downloader.DownloadFile(cfg.HarmonyUrl);
                if (harmonyInBytes == null)
                    return (1, $"Unable to access {cfg.HarmonyUrl}");
                int status = mp.LoadDependency(harmonyInBytes, harmonyInBytes.Length);
                if (status != 0)
                    return (1, $"Dependency:{StatusToString(status)}");
                byte[] modInBytes = await Downloader.DownloadFile(cfg.ModUrl);
                if (modInBytes == null)
                    return (1, $"Unable to access {cfg.ModUrl}");
                status = mp.LoadMod(modInBytes, modInBytes.Length, cfg.Namespace, cfg.Class, cfg.Method);
                if (status != 0)
                    return (1, StatusToString(status));
                cfg.ProcId = res[0].Id;
                return (0, "Success");
            }
        }

        public async Task<(int, string)> LoadCustomMod(UserModConfig cfg)
        {
            await Task.Delay(0);
            Process[] res = Process.GetProcessesByName(cfg.ProcName);
            if (res.Length == 0)
                return (1, "Process not found");
            if (res[0].Id == cfg.ProcId)
                return (1, "Mod already loaded");

            using (MonoProcess mp = new MonoProcess(res[0].Id))
            {
                AppNotifier.Info("Loading...");
                int status = 0;
                if (cfg.HarmonyVersion != "None")
                    status = mp.LoadDependencyFrom($"{System.AppDomain.CurrentDomain.BaseDirectory}\\Harmony\\net{cfg.HarmonyVersion}\\0Harmony.dll");
                if (status != 0)
                    return (1, $"Dependency:{StatusToString(status)}");
                status = mp.LoadModFrom(cfg.ModPath, cfg.Nspace, cfg.Klass, cfg.Method);
                if (status != 0)
                    return (1, StatusToString(status));
                cfg.ProcId = res[0].Id;
                return (0, "Success");
            }
        }

        public void Serialize()
        {
            try
            {
                using (FileStream fs = new FileStream("config", FileMode.Create))
                {
                    byte[] fileInBytes = ZeroFormatterSerializer.Serialize(userConfigs);
                    fs.Write(fileInBytes, 0, fileInBytes.Length);
                }
            }
            catch (Exception ex)
            {
                AppNotifier.Error(ex.Message);
            }
        }

        public void Deserialize()
        {
            try
            {
                if (!File.Exists("config"))
                    return;
                byte[] fileInBytes = File.ReadAllBytes("config");
                var userConfigsCopy = ZeroFormatterSerializer.Deserialize<UserModConfig[]>(fileInBytes);
                for (int i = 0; i < 8; i++)
                    if (userConfigsCopy[i] != null)
                        AddConfig(new UserModConfig(userConfigsCopy[i]));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}