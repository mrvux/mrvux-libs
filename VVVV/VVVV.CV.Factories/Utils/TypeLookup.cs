using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using MrVux.Utilities.EventGenerics;

namespace MrVux.Utilities.Reflection
{
    public class TypeLookup
    {
        //private Assembly[] assemblies;
        public event GenericEventHandler<Type> TypeFound;

        #region Constructors
        public TypeLookup()
        {
            //this.assemblies = alook.Lookup().ToArray();
        }
        #endregion

        public void LookupService(Assembly assembly, Type serviceType, bool concreteonly)
        {
            try
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (serviceType.IsAssignableFrom(t))
                    {
                        if (!concreteonly || !t.IsAbstract)
                        {
                            if (TypeFound != null)
                            {
                                TypeFound(t);
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException rex)
            {
                Console.WriteLine(rex.Message);
                foreach (Exception e in rex.LoaderExceptions)
                {
                    Console.WriteLine(e.Message);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public void LookupService(Type serviceType, bool concreteonly)
        {
            //foreach (Assembly assembly in Assembly.GetExecutingAssembly())
            //{
            Assembly assembly = Assembly.GetExecutingAssembly();
            this.LookupService(assembly, serviceType, concreteonly);
        }

    }
}
