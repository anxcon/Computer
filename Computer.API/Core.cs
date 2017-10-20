using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Computer.API
{
    public sealed class Core
    {
        public static Core Instance { get; private set; }
        private Dictionary<string, ModuleBase> modules;
        public static ReadOnlyDictionary<string, ModuleBase> Modules
        {
            get
            {
                return new ReadOnlyDictionary<string, ModuleBase>(Core.Instance.modules);
            }
        }
        public Core()
        {
            Core.Instance = this;
            this.modules = new Dictionary<string, ModuleBase>();
        }
        public void Load()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Utilities.VerifyFolder($"{path}Plugins/");
            Settings.Load(path);
            LoadPlugins(path);
            LoadModules();
            if (this.modules.Count == 0)
            {
                //no modules found, should exit
                
            }
            Settings.Save(path);
        }
        public void Start()
        {
            Logger.Log("Starting modules...");
            int started = 0;
            foreach (ModuleBase module in this.modules.Values)
            {
                if (!module.Enabled || !module.Loaded)
                {
                    continue;
                }
                try
                {
                    module.StartModule();
                    started += 1;
                }
                catch (Exception exc)
                {
                    Logger.Log(exc);
                }
            }
            Logger.Log($"Started {started} modules");
        }
        private void LoadPlugins(string path)
        {
            Logger.Log("Checking for plugins...");
            List<string> files = Utilities.GetFiles($"{path}Plugins/", "*.dll");
            Logger.Log($"Loading {files.Count} plugins...");
            foreach (string file in files)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!type.IsSubclassOf(typeof(ModuleBase)) || type.IsAbstract)
                        {
                            continue;
                        }
                        ModuleBase module = Activator.CreateInstance(type) as ModuleBase;
                        this.modules.Add(module.Name, module);
                    }
                }
                catch (Exception exc)
                {
                    Logger.Log(exc);
                }
            }
            Logger.Log($"Found {this.modules.Count} modules");
        }
        private void LoadModules()
        {
            Logger.Log("Loading modules...");
            int loaded = 0;
            foreach (ModuleBase module in this.modules.Values)
            {
                if (LoadModule(module))
                {
                    loaded += 1;
                }
            }
            Logger.Log($"Loaded {loaded} modules");
        }
        private bool LoadModule(ModuleBase module)
        {
            if (module.Loaded)
            {
                return true;
            }
            if(!module.Enabled)
            {
                return false;
            }
            foreach (string dep in module.Dependencies)
            {
                ModuleBase depmod = this.modules[dep];
                if (depmod == null)
                {
                    Logger.LogErr($"Required dependency {dep} is missing");
                    module.Enabled = false;
                    return false;
                }
                if (!depmod.Enabled || !LoadModule(depmod))
                {
                    Logger.LogErr($"Required dependency {dep} not loaded");
                    module.Enabled = false;
                    return false;
                }
            }
            Logger.Log($"Loading {module.Name}...");
            try
            {
                module.LoadModule();
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                module.Enabled = false;
                return false;
            }
            return true;
        }
        public void Run()
        {
            while (true)
            {
                string line = Console.ReadLine();
                switch (line)
                {
                    case "exit":
                        return;
                    default:
                        break;
                }
            }
        }
    }
}
