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
    public class PropertyMap<TEntity, TDal> : ExpressionVisitor, IPropertyMap<TEntity, TDal>
        where TEntity : class
        where TDal : class, IEntity
    {
        private readonly string paramName = "x";
        private readonly Dictionary<MemberInfo, MemberInfo> mapDictionary;
        private readonly ParameterExpression paramExpr;

        public PropertyMap()
        {
            mapDictionary = new Dictionary<MemberInfo, MemberInfo>();
            paramExpr = Expression.Parameter(typeof(TEntity), paramName);
        }

        public Expression<Func<TEntity, bool>> MapExpression(
            Expression<Func<TDal, bool>> sourceExpression)
        {
            return Expression.Lambda<Func<TEntity, bool>>(Visit(sourceExpression.Body), paramExpr);
        }

        public IPropertyMap<TEntity, TDal> Map<TProperty>(
            Expression<Func<TDal, TProperty>> targetProp,
            Expression<Func<TEntity, TProperty>> sourceProp)
        {
            mapDictionary.Add(targetProp.GetPropertyInfo(), sourceProp.GetPropertyInfo());

            return this;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (IsParameterProperty(node))
            {
                return Expression.MakeMemberAccess(paramExpr, mapDictionary[node.Member]);
            }
            else return node;
        }

        private bool IsParameterProperty(MemberExpression node)
        {
            if (node.Expression == null)
                return false;

            if (node.Expression.NodeType == ExpressionType.Parameter)
                return true;

            if (node.Expression.NodeType != ExpressionType.MemberAccess)
                return false;

            return IsParameterProperty((MemberExpression)node.Expression);
        }
    }
}
