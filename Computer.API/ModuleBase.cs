using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Computer.API
{
    public abstract class ModuleBase
    {
        public string Name { get; private set; }
        public bool Loaded { get; private set; }
        public bool Enabled { get; set; }
        private List<string> dependencies;
        public ReadOnlyCollection<string> Dependencies
        {
            get
            {
                return new ReadOnlyCollection<string>(this.dependencies);
            }
        }

        public ModuleBase()
        {
            this.Name = this.GetType().Name;
            this.dependencies = new List<string>();
            this.Enabled = true;
        }
        internal void LoadModule()
        {
            foreach (FieldInfo field in this.GetType().GetFields((BindingFlags)124))
            {
                if (!field.IsDefined(typeof(Setting)))
                {
                    continue;
                }
                field.SetValue(this, Settings.GetValue($"{this.Name}.{field.Name}", field.GetValue(this)));
            }
            this.Load();
            this.Loaded = true;
        }
        protected virtual void Load() { }
        internal void StartModule()
        {
            this.Start();
        }
        protected virtual void Start() { }
        internal void SaveModule()
        {
            this.Save();
        }
        protected virtual void Save() { }
        protected void Log(string sText)
        {
            Logger.Log($"{this.Name}: {sText}");
        }
        protected void LogErr(string sText)
        {
            Logger.LogErr($"{this.Name}: {sText}");
        }
        protected void Log(Exception exc)
        {
            Logger.Log(this.Name, exc);
        }
    }
}
