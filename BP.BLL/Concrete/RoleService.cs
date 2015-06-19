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

        public BllRole Find(int id)
        {
            return roleRepo.Find(id).ToBal();
        }

        public BllRole Find(string uniqueKey)
        {
            if (uniqueKey == null)
                return null;
            return roleRepo.Find(x => x.Name == uniqueKey).ToBal();
        }

        public IEnumerable<BllRole> GetAll()
        {
            return roleRepo.GetAll().Select(x => x.ToBal());
        }

        public void Create(BllRole entity)
        {
            roleRepo.Create(entity.ToDal());
            uow.Save();
        }

        public void Create(string name)
        {
            Create(new BllRole { Name = name });
        }

        public void Update(BllRole entity)
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

        public bool Remove(BllRole entity)
        {
            if (entity != null)
                return Remove(entity.Id);
            else return false;
        }

        public bool Remove(string uniqueKey)
        {
            return Remove(Find(uniqueKey));
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
