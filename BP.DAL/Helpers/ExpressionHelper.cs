using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Helpers
{
    public static class ExpressionHelper
    {
        public static PropertyInfo GetPropertyInfo<TSource, TValue>(
            this Expression<Func<TSource, TValue>> expression)
        {
            return (PropertyInfo)((MemberExpression)expression.Body).Member;
        }
    }
}
