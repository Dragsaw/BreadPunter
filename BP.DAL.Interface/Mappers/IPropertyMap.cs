using BP.DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Interface.Mappers
{
    public interface IPropertyMap<TSource, TTarget>
        where TSource : class
        where TTarget : class, IEntity
    {
        IPropertyMap<TSource, TTarget> Map<TProperty>(
            Expression<Func<TTarget, TProperty>> targetProp,
            Expression<Func<TSource, TProperty>> sourceProp);

        Expression<Func<TSource, bool>> MapExpression(
            Expression<Func<TTarget, bool>> sourceExpression);
    }
}
