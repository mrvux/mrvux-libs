using System;
using System.Collections.Generic;
using System.Text;

namespace NTP.Utilities.Reflection
{
    public class GenericsHelper
    {
        public static T GetGenericInstance<T>(Type type,params Type[] typeargs) 
        {
            Type closedtype = type.MakeGenericType(typeargs);
            object o = Activator.CreateInstance(closedtype);
            return (T)o;
        }

        public static T GetGenericInstance<T>(Type type, Type[] typeargs, params object[] args)
        {
            Type closedtype = type.MakeGenericType(typeargs);
            object o = Activator.CreateInstance(closedtype,args);
            return (T)o;
        }
    }
}
