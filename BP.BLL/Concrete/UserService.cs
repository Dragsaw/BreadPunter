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
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.Web.Helpers;

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

        public BalUser Find(string uniqueKey)
        {
            if (uniqueKey == null)
                return null;

            DalUser user = userRepo.Find(u => u.Email == uniqueKey);
            return user == null ? null : user.ToBal();
        }

        public IEnumerable<BalUser> Get(IEnumerable<BalUserSkill> skills)
        {
            List<BalUser> result = new List<BalUser>();
            if (userSkillRepo == null)
                userSkillRepo = uow.GetRepository<DalUserSkill>();

            List<DalUserSkill> userSkills = new List<DalUserSkill>();
            foreach (var item in skills.Where(s => s != null))
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
            entity.Password = Crypto.HashPassword(entity.Password);
            userRepo.Create(entity.ToDal());
            uow.Save();
        }

        public void Create(string email, string password, BalRole role)
        {
            Create(new BalUser { Email = email, Password = password, Role = role });
        }

        public void Update(BalUser entity)
        {
            userRepo.Update(entity.ToDal());
            uow.Save();
        }

        public bool Remove(int id)
        {
            BalUser user = Find(id);
            if (user == null)
                return false;
            userRepo.Remove(user.Id);
            uow.Save();
            return true;
        }

        public bool Remove(BalUser entity)
        {
            if (entity != null)
                return Remove(entity.Id);
            else return false;
        }

        public bool Remove(string uniqueKey)
        {
            return Remove(Find(uniqueKey));
        }

        public bool Exists(string email, string password)
        {
            BalUser user = Find(email);
            if (user == null)
                return false;

            return Crypto.VerifyHashedPassword(user.Password, password);
        }
    }
}
