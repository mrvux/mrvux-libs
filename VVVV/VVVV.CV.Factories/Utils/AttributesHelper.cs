using System;
using System.Collections.Generic;
using System.Text;

namespace NTP.Utilities.Reflection
{
    #region Exception
    /// <summary>
    /// Exception for attributes lookup
    /// </summary>
    public class AttributeException : Exception 
    {
        private readonly Type attr;

        #region Constructor
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="attributetype">Attribute type</param>
        public AttributeException(string message, Type attributetype) : base(message)
        {
            this.attr = attributetype;
        }
        #endregion

        #region Attribute Type
        /// <summary>
        /// Attribute type which has not been found
        /// </summary>
        public Type AttributeType
        {
            get { return attr; }
        }
        #endregion
    }

    public class AttributeNotFoundException : AttributeException 
    {
        public AttributeNotFoundException(string message, Type attributetype) : base(message, attributetype) { }
    }

    public class MultipleAttributeFoundException : AttributeException
    {
        public MultipleAttributeFoundException(string message, Type attributetype) : base(message, attributetype) { }
    }
    #endregion


    /// <summary>
    /// Utility class To retrieve class attributes using generics.
    /// </summary>
    public class AttributesHelper
    {
        #region Get Attributes
        /// <summary>
        /// This methods returns a list of attributes from a certain type.
        /// </summary>
        /// <typeparam name="A">Attribute type to look for</typeparam>
        /// <param name="type">Type to look attribute into</param>
        /// <returns>List of matching attributes</returns>
        public static List<A> GetAttributes<A>(Type type)
        {
            List<A> res = new List<A>();
            
            object[] o;
            o = type.GetCustomAttributes(typeof(A), false);

            foreach (object attr in o) 
            {
                res.Add((A)attr);
            }
            return res;
        }
        #endregion

        #region Returns a single attribute
        /// <summary>
        /// This method returns a single attri
        /// </summary>
        /// <typeparam name="A">Attribute type</typeparam>
        /// <param name="type">Type to look for attributes</param>
        /// <returns>Attribute details</returns>
        /// <exception cref="NTP.Utilities.Reflection.AttributeNotFoundException">
        /// Thrown when attribute has not been found
        /// </exception>
        /// <exception cref="NTP.Utilities.Reflection.MultipleAttributeFoundException">
        /// Thrown when multiple attributes has been found
        /// </exception>
        public static A GetSingleAttribute<A>(Type type)
        {
            object[] o;
            o = type.GetCustomAttributes(typeof(A), false);
            
            if (o.Length > 0)
            {
                if (o.Length == 1)
                {
                    A attr = (A)o[0];
                    return attr;
                }
                else
                {
                    throw new MultipleAttributeFoundException("Multiple Attributes found", typeof(A));
                }
            }
            else
            {
                throw new AttributeNotFoundException("Attribute type not found", typeof(A));
            }
        }
        #endregion

        #region Returns a single attribute
        public static bool HasSingleAttribute<A>(Type type)
        {
            object[] o;
            o = type.GetCustomAttributes(typeof(A), false);
            return o.Length == 1;
        }
        #endregion
    }
}
