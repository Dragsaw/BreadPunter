using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using BP.BLL.Interface.Services;
using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Entities.Users;
using BP.DAL.Interface.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using BP.BLL.Mappers;

namespace BP.BLL.Concrete
{
    public class UserService : IUserService
    {
        private readonly IRepository<DalUser> userRepo;
        private readonly IUnitOfWork uow;
        private IRepository<DalUserSkill> userSkillRepo;

        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
            this.userRepo = uow.GetRepository<DalUser>();
        }

        public BalUser Find(int id)
        {
            return userRepo.Find(id).ToBal();
        }

        public BalUser Find(string email)
        {
            DalUser user = userRepo.Find(u => u.Email == email);
            return user == null ? null : user.ToBal();
        }

        public IEnumerable<BalUser> Get(IEnumerable<BalUserSkill> skills)
        {
            List<BalUser> result = new List<BalUser>();
            if (userSkillRepo == null)
                userSkillRepo = uow.GetRepository<DalUserSkill>();

            List<DalUserSkill> userSkills = new List<DalUserSkill>();
            foreach (var item in skills)
            {
                userSkills.AddRange(userSkillRepo.Get(s => s.Skill.Id == item.Id && s.Level >= item.Level));
            }
            
            return userSkills.GroupBy(k => k.User).Where(d => d.Count() == skills.Count()).Select(d => d.Key.ToBal());
        }

        public IEnumerable<BalUser> GetAll()
        {
            return userRepo.GetAll().Select(d => d.ToBal());
        }

        public void Create(BalUser entity)
        {
            userRepo.Create(entity.ToDal());
            uow.Save();
        }

        public void Update(BalUser entity)
        {
            userRepo.Update(entity.ToDal());
            uow.Save();
        }

        public void Remove(BalUser entity)
        {
            userRepo.Delete(entity.ToDal());
            uow.Save();
        }
    }
}
