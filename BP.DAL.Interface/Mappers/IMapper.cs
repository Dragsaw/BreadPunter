using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Interface.Mappers
{
    public interface IMapper<TEntity, TDal>
    {
        TEntity ToDb(TDal obj);
        void CopyFields(TDal obj, TEntity entity);
        TDal ToDal(TEntity entity);
        Expression<Func<TEntity, bool>> MapExpression(
            Expression<Func<TDal, bool>> sourceExpression);
    }
}
