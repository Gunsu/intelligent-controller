using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace IC.Modules.Menu.Tests.Mocks
{
	public sealed class MockUnityContainer : IUnityContainer
	{
		public IRegionViewRegistry ResolvedContentRegistryService;
		public Dictionary<Type, Type> Types = new Dictionary<Type, Type>();

		public IUnityContainer RegisterType<TFrom, TTo>() where TTo : TFrom
		{
			Types.Add(typeof(TFrom), typeof(TTo));
			return this;
		}

		public IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager) where TTo : TFrom
		{
			return RegisterType<TFrom, TTo>();
		}


		public IUnityContainer RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers) where TTo : TFrom
		{
			return RegisterType<TFrom, TTo>();
		}

		public T Resolve<T>()
		{
			if (typeof(T) == typeof(IRegionViewRegistry))
			{
				return (T)ResolvedContentRegistryService;
			}

			throw new Exception();
		}

		#region IUnityContainer common members

		public IUnityContainer AddExtension(UnityContainerExtension extension)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer AddNewExtension<TExtension>() where TExtension : UnityContainerExtension, new()
		{
			throw new System.NotImplementedException();
		}

		public object BuildUp(System.Type t, object existing, string name)
		{
			throw new System.NotImplementedException();
		}

		public object BuildUp(System.Type t, object existing)
		{
			throw new System.NotImplementedException();
		}

		public T BuildUp<T>(T existing, string name)
		{
			throw new System.NotImplementedException();
		}

		public T BuildUp<T>(T existing)
		{
			throw new System.NotImplementedException();
		}

		public object Configure(System.Type configurationInterface)
		{
			throw new System.NotImplementedException();
		}

		public TConfigurator Configure<TConfigurator>() where TConfigurator : IUnityContainerExtensionConfigurator
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer CreateChildContainer()
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer Parent
		{
			get { throw new System.NotImplementedException(); }
		}

		public IUnityContainer RegisterInstance(System.Type t, string name, object instance, LifetimeManager lifetime)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterInstance(System.Type t, string name, object instance)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterInstance(System.Type t, object instance, LifetimeManager lifetimeManager)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterInstance(System.Type t, object instance)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterInstance<TInterface>(string name, TInterface instance, LifetimeManager lifetimeManager)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterInstance<TInterface>(string name, TInterface instance)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterInstance<TInterface>(TInterface instance, LifetimeManager lifetimeManager)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterInstance<TInterface>(TInterface instance)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType(System.Type from, System.Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType(System.Type t, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType(System.Type t, string name, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType(System.Type t, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType(System.Type from, System.Type to, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType(System.Type from, System.Type to, string name, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType(System.Type from, System.Type to, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType(System.Type t, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType<T>(string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType<T>(string name, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType<T>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType<TFrom, TTo>(string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TTo : TFrom
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType<TFrom, TTo>(string name, params InjectionMember[] injectionMembers) where TTo : TFrom
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TTo : TFrom
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RegisterType<T>(params InjectionMember[] injectionMembers)
		{
			throw new System.NotImplementedException();
		}

		public IUnityContainer RemoveAllExtensions()
		{
			throw new System.NotImplementedException();
		}

		public object Resolve(System.Type t, string name)
		{
			throw new System.NotImplementedException();
		}

		public object Resolve(System.Type t)
		{
			throw new System.NotImplementedException();
		}

		public T Resolve<T>(string name)
		{
			throw new System.NotImplementedException();
		}

		public System.Collections.Generic.IEnumerable<object> ResolveAll(System.Type t)
		{
			throw new System.NotImplementedException();
		}

		public System.Collections.Generic.IEnumerable<T> ResolveAll<T>()
		{
			throw new System.NotImplementedException();
		}

		public void Teardown(object o)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region IDisposable members

		public void Dispose()
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}
