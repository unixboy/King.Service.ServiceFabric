﻿using System;

namespace King.Service.ServiceFabric.Demo.Dequeue
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                var rt = new RunTime<Service>("DequeueType");
                rt.Run();
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}