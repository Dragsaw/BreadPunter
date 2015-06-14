using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.BLL.Concrete;
using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Services;
using BP.DAL.Interface.Repositories;
using BP.DAL.Interface.Entities;
using BP.BLL.Mappers;

namespace BP.BLL.Concrete
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<DalRole> roleRepo;
        private readonly IUnitOfWork uow;

        public RoleService(IUnitOfWork uow)
        {
            this.uow = uow;
            roleRepo = uow.GetRepository<DalRole>();
        }

        public BalRole Find(int id)
        {
            return roleRepo.Find(id).ToBal();
        }

        public BalRole Find(string uniqueKey)
        {
            if (uniqueKey == null)
                return null;
            return roleRepo.Find(x => x.Name == uniqueKey).ToBal();
        }

        public IEnumerable<BalRole> GetAll()
        {
            return roleRepo.GetAll().Select(x => x.ToBal());
        }

        public void Create(BalRole entity)
        {
            roleRepo.Create(entity.ToDal());
            uow.Save();
        }

        public void Create(string name)
        {
            Create(new BalRole { Name = name });
        }

        public void Update(BalRole entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            roleRepo.Update(entity.ToDal());
            uow.Save();
        }

        public bool Remove(int id)
        {
            DalRole role = roleRepo.Find(id);
            if (role == null)
                return false;
            roleRepo.Remove(id);
            uow.Save();
            return true;
        }

        public bool Remove(BalRole entity)
        {
            if (entity != null)
                return Remove(entity.Id);
            else return false;
        }

        public bool Remove(string uniqueKey)
        {
            return Remove(Find(uniqueKey));
        }
    }
}
