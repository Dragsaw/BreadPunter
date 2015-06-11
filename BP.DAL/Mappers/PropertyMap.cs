using BP.DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BP.DAL.Interface.Mappers;
using System.Reflection;
using BP.DAL.Helpers;

namespace BP.DAL.Mappers
{
    public class PropertyMap<TSource, TTarget> : IPropertyMap<TSource, TTarget>
        where TSource : class
        where TTarget : class, IEntity
    {
        private readonly string paramName = "x";
        private readonly List<MemberAssignment> memberAssignments;
        private readonly ParameterExpression paramExpr;
        private Expression<Func<TSource, TTarget>> mapExpression;

        public Expression<Func<TSource, TTarget>> MapExpression
        {
            get
            {
                return mapExpression ?? (mapExpression = CreateMapExpression());
            }
        }

        public PropertyMap()
        {
            memberAssignments = new List<MemberAssignment>();
            paramExpr = Expression.Parameter(typeof(TSource), paramName);
        }

        public IPropertyMap<TSource, TTarget> Map<TProperty>(
            Expression<Func<TTarget, TProperty>> targetProp, 
            Expression<Func<TSource, TProperty>> sourceProp)
        {
            MemberExpression expr = CreatePropertyMemberExpression((MemberExpression)targetProp.Body);
            MemberAssignment propInit = Expression.Bind(targetProp.GetPropertyInfo(), expr);
            memberAssignments.Add(propInit);

            return this;
        }

        private MemberExpression CreatePropertyMemberExpression(MemberExpression expression, 
            IList<PropertyInfo> properties = null)
        {
            if (properties == null)
                properties = new List<PropertyInfo>();

            if (expression.Expression.NodeType == ExpressionType.Parameter)
            {
                properties.Add((PropertyInfo)expression.Member);
                return CreatePropertyMemberExpressionFromList(properties);
            }
            if (expression.Expression.NodeType != ExpressionType.MemberAccess)
                throw new NotSupportedException();

            properties.Add((PropertyInfo)expression.Member);

            return CreatePropertyMemberExpression((MemberExpression)expression.Expression, properties);
        }

        private MemberExpression CreatePropertyMemberExpressionFromList(IList<PropertyInfo> properties)
        {
            Expression result = paramExpr;

            foreach (var item in properties.Reverse())
            {
                result = Expression.MakeMemberAccess(result, item);
            }

            return (MemberExpression)result;
        }

        private Expression<Func<TSource, TTarget>> CreateMapExpression()
        {
            throw new NotImplementedException();
        }
    }
}
