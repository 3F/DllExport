//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

using System;
using System.ComponentModel.Design;

namespace RGiesecke.DllExport
{
    internal static class DllExportServiceProviderExtensions
    {
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider.GetService(typeof(TService));
        }

        public static TServiceProvider AddService<TServiceProvider, TService>(this TServiceProvider serviceProvider, TService service) where TServiceProvider : IServiceContainer
        {
            serviceProvider.AddService(typeof(TService), (object)service);
            return serviceProvider;
        }

        public static TServiceProvider AddService<TServiceProvider, TService>(this TServiceProvider serviceProvider, TService service, bool promote) where TServiceProvider : IServiceContainer
        {
            serviceProvider.AddService(typeof(TService), (object)service, promote);
            return serviceProvider;
        }

        public static TServiceProvider AddServiceFactory<TServiceProvider, TService>(this TServiceProvider serviceProvider, Func<IServiceProvider, TService> serviceFactory) where TServiceProvider : IServiceContainer
        {
            serviceProvider.AddService(typeof(TService), (ServiceCreatorCallback)((sp, t) => (object)serviceFactory((IServiceProvider)sp)));
            return serviceProvider;
        }

        public static TServiceProvider AddServiceFactory<TServiceProvider, TService>(this TServiceProvider serviceProvider, Func<IServiceProvider, TService> serviceFactory, bool promote) where TServiceProvider : IServiceContainer
        {
            serviceProvider.AddService(typeof(TService), (ServiceCreatorCallback)((sp, t) => (object)serviceFactory((IServiceProvider)sp)), promote);
            return serviceProvider;
        }
    }
}
