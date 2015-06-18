using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Mappers;
using BP.DAL.Interface.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Concrete
{
    public class GenericRepository<TEntity, TDal> : IRepository<TDal>
        where TEntity : class
        where TDal : class, IEntity
    {
        private readonly DbContext context;
        private readonly IMapper<TEntity, TDal> mapper;
        private readonly DbSet<TEntity> set;

        public GenericRepository(DbContext context, IMapper<TEntity, TDal> mapper)
        {
            this.context = context;
            this.mapper = mapper;
            set = context.Set<TEntity>();
        }

        public TDal Find(int id)
        {
            return mapper.ToDal(set.Find(id));
        }

        public TDal Find(Expression<Func<TDal, bool>> predicate)
        {
            return mapper.ToDal(set.FirstOrDefault(mapper.MapExpression(predicate)));
        }

        public IEnumerable<TDal> GetAll()
        {
            return set.AsEnumerable().Select(x => mapper.ToDal(x));
        }

        public IEnumerable<TDal> Get(Expression<Func<TDal, bool>> predicate)
        {
            return set.Where(mapper.MapExpression(predicate))
                .AsEnumerable()
                .Select(x => mapper.ToDal(x));
        }

        public void Create(TDal entity)
        {
            set.Add(mapper.ToDb(entity));
        }

        public void Update(TDal entity)
        {
            TEntity entityToUpdate = set.Find(entity.GetId());
            mapper.CopyFields(entity, entityToUpdate);
        }

        public void Remove(int id)
        {
            TEntity entityToRemove = set.Find(id);
            if (entityToRemove != null)
                set.Remove(entityToRemove);
        }

        public void Remove(TDal entity)
        {
            TEntity entityToDelete = set.Find(entity.GetId());
            set.Remove(entityToDelete);
        }
    }
}
