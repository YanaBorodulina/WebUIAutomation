using System;
using System.Linq;

namespace Sample.Web.Core.Extensions
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute =>
            type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att
                ? valueSelector(att)
                : default;
    }
}