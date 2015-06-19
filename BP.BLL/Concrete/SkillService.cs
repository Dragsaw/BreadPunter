using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Services;
using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.BLL.Mappers;

namespace BP.BLL.Concrete
{
    public class SkillService : IService<BalSkill>
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<DalSkill> repository;

        public SkillService(IUnitOfWork uow, IRepository<DalSkill> repository)
        {
            this.uow = uow;
            this.repository = repository;
        }

        public BalSkill Find(int id)
        {
            return repository.Find(id).ToBal();
        }

        public BalSkill Find(string uniqueKey)
        {
            return repository.Find(x => x.Name == uniqueKey).ToBal();
        }

        public IEnumerable<BalSkill> GetAll()
        {
            return repository.GetAll().Select(x => x.ToBal());
        }

        public void Create(BalSkill entity)
        {
            repository.Create(entity.ToDal());
        }

        public void Update(BalSkill entity)
        {
            repository.Update(entity.ToDal());
        }

        public bool Remove(int id)
        {
            return Remove(repository.Find(id));
        }

        public bool Remove(BalSkill entity)
        {
            return Remove(entity.Id);
        }

        public bool Remove(string uniqueKey)
        {
            BalSkill skill = Find(uniqueKey);
            if (skill == null)
                return false;
            return Remove(skill.ToDal());
        }

        private bool Remove(DalSkill skill)
        {
            if (skill != null)
            {
                repository.Remove(skill);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
