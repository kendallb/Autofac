﻿using System;
using System.Reflection;
using Xunit;

namespace Autofac.Specification.Test.Registration
{
    public class AssemblyScanningTests
    {
        public interface IMyService
        {
        }

        [Fact]
        public void OnlyServicesAssignableToASpecificTypeAreRegisteredFromAssemblies()
        {
            var container = new ContainerBuilder().Build().BeginLifetimeScope(b =>
                b.RegisterAssemblyTypes(GetType().GetTypeInfo().Assembly)
                    .AssignableTo(typeof(IMyService)));

            Assert.Single(container.ComponentRegistry.Registrations);
            Assert.True(container.TryResolve(typeof(MyComponent), out object obj));
            Assert.False(container.TryResolve(typeof(MyComponent2), out obj));
        }

        public sealed class MyComponent : IMyService
        {
        }

        public sealed class MyComponent2
        {
        }
    }
}
