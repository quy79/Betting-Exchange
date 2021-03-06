﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.Infrastructure
{
    /// <summary>
    /// Dependency resolver
    /// </summary>
    public interface IDependencyResolver
    {     
        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="type">Type</param>
        /// <returns>Result</returns>
        T Resolve<T>(Type type);

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="type">Type</param>
        /// <param name="name">Name</param>
        /// <returns>Result</returns>
        T Resolve<T>(Type type, string name);

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Result</returns>
        T Resolve<T>();

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="name">Name</param>
        /// <returns>Result</returns>
        T Resolve<T>(string name);

        /// <summary>
        /// Resolve all
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Result</returns>
        IEnumerable<T> ResolveAll<T>();
    }
}
