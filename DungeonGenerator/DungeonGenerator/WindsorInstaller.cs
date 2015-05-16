using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DungeonGenerator.Corridors;
using DungeonGenerator.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator
{
    public class WindsorInstaller : IWindsorInstaller
    {

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                    .BasedOn<IRoomGenerator>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());
            container.Register(Classes.FromThisAssembly()
                    .BasedOn<ICorridorGenerator>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());
            container.Register(Component.For<Dungeon>().ImplementedBy<Dungeon>());
            container.Register(Component.For<Random>().ImplementedBy<Random>().LifestyleSingleton());

        }
    }
}
