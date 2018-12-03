using System;
using LibCore.Configuration;
using LibCore.Data;
using LibCore.Infrastructure.DependencyManagement;

namespace LibCore.Infrastructure
{

    /// <summary>
    /// Class Engine
    /// </summary>
    public class Engine : IEngine
    {

        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Properties

        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion

        public void Initialize(EnterpriseConfig config)
        {
            var databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();
            if (databaseInstalled)
            {
                //startup tasks
                //RunStartupTasks();
            }
        }

        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }
    }
}
