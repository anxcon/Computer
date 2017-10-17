using System;

using Computer.API;

namespace Computer
{
    class Program
    {
        static void Main(string[] args)
        {
            Core core = new Core();
            try
            {
                core.Load();
                core.Start();
                core.Run();
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }
        }
    }
}
