﻿namespace King.Service.ServiceFabric
{
    using Microsoft.ServiceFabric.Services.Runtime;
    using System;
    using System.Diagnostics;
    using System.Fabric;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Role Service
    /// </summary>
    /// <typeparam name="T">Configuration Type</typeparam>
    public class RoleService<T>
    {
        #region Members
        /// <summary>
        /// Configuration
        /// </summary>
        protected readonly T configuration;

        /// <summary>
        /// Task Manager
        /// </summary>
        protected readonly IRoleTaskManager<T> taskManager;
        #endregion

        #region Constructors
        /// <summary>
        /// Role Service Constructor
        /// </summary>
        /// <param name="serviceContext">Service Context</param>
        /// <param name="taskManager">Task Manager</param>
        /// <param name="config">Configuration</param>
        public RoleService(IRoleTaskManager<T> taskManager, T config = default(T))
        {
            if (null == taskManager)
            {
                throw new ArgumentNullException("taskManager");
            }

            this.taskManager = taskManager;
            this.configuration = config;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run Async
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Task</returns>
        protected async Task RunAsync(CancellationToken cancellationToken)
        {
            if (this.taskManager.OnStart(this.configuration))
            {
                this.taskManager.Run();

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    Trace.TraceError("Task Canceled Exception, can be normal: {0}", ex);
                }

                this.taskManager.OnStop();
            }
            else
            {
                Trace.TraceError("Task Manager failed to start. Service did not complete start process successfully.");
            }
        }
        #endregion
    }
}