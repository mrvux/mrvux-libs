using System;
using System.Collections.Generic;
using System.Text;
using MrVux.Utilities.EventGenerics;
using System.Reflection;

namespace MrVux.Utilities.Reflection
{
    public abstract class BaseTypeLocator<T>
    {
        protected List<Type> types = new List<Type>();
        protected TypeLookup typelookup;

        #region Total Items
        public int Total
        {
            get
            {
                return types.Count;
            }
        }
        #endregion

        protected abstract bool RegisterType(Type type);

        #region Constructor
        public BaseTypeLocator(TypeLookup typelookup)
        {
            this.typelookup = typelookup;
            this.typelookup.TypeFound += typelookup_TypeFound;
        }
        #endregion

        #region Lookup
        protected virtual void Lookup()
        {
            typelookup.LookupService(typeof(T), true);
        }
        #endregion

        public void Lookup(Assembly a)
        {
            typelookup.LookupService(a,typeof(T), true);
        }

        #region Event When type is found
        private void typelookup_TypeFound(Type t)
        {
            if (RegisterType(t))
            {
                types.Add(t);
            }
        }
        #endregion


    }
}
