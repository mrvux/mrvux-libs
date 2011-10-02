using System;
using System.Collections.Generic;
using System.Text;

namespace MrVux.Utilities.EventGenerics
{
    /// <summary>
    /// Simple generic event handler for a single parameter
    /// </summary>
    /// <typeparam name="T">Event parameter type</typeparam>
    /// <param name="args">Event argument</param>
    public delegate void GenericEventHandler<T>(T args);


    /// <summary>
    /// Simple generic event handler for two parameters
    /// </summary>
    /// <param name="sender">Event sender (or anything else)</param>
    /// <typeparam name="S">Event sender type (or another type)</typeparam>
    /// <typeparam name="T">Event parameter type</typeparam>
    /// <param name="args">Event argument</param>
    public delegate void GenericEventHandler<S, T>(S sender,T args);    
}
