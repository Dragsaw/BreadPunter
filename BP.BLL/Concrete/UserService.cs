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
        private readonly IUnitOfWork uow;
        private readonly IRepository<DalUser> userRepo;
        private readonly IRepository<DalUserSkill> userSkillRepo;
        private readonly IRepository<DalFilter> filterRepo;

        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
            this.userRepo = uow.GetRepository<DalUser>();
            this.userSkillRepo = uow.GetRepository<DalUserSkill>();
            this.filterRepo = uow.GetRepository<DalFilter>();
        }

        public BllUser Find(int id)
        {
            DalUser user = userRepo.Find(id);
            FillAdditionalProperties(user);
            return user.ToBal();
        }

        public BllUser Find(string uniqueKey)
        {
            if (uniqueKey == null)
                return null;

            DalUser user = userRepo.Find(u => u.Email == uniqueKey);
            FillAdditionalProperties(user);
            return user.ToBal();
        }

        public IEnumerable<BllUser> Get(IEnumerable<BllUserSkill> skills)
        {
            List<BllUser> result = new List<BllUser>();

            List<DalUserSkill> userSkills = new List<DalUserSkill>();

            foreach (var item in skills.Where(s => s != null))
            {
                userSkills.AddRange(userSkillRepo.Get(s => s.Skill.Id == item.Skill.Id && s.Level >= item.Level));
            }

            return userSkills.GroupBy(k => k.User.Id).Where(d => d.Count() == skills.Count()).Select(d => Find(d.Key));
        }

        public IEnumerable<BllUser> GetAll()
        {
            return userRepo.GetAll().Select(d => d.ToBal());
        }

        public void Create(BllUser entity)
        {
            entity.Password = Crypto.HashPassword(entity.Password);
            userRepo.Create(entity.ToDal());
            uow.Save();
        }

        public void Create(string email, string password, BllRole role)
        {
            Create(new BllUser { Email = email, Password = password, Role = role });
        }

        public void Update(BllUser entity)
        {
            if (entity is BllProgrammer)
            {
                UpdateUserSkills((BllProgrammer)entity);
            }
            else if (entity is BllManager)
            {
                UpdateFilters((BllManager)entity);
            }

            userRepo.Update(entity.ToDal());
            uow.Save();
        }

        public bool Remove(int id)
        {
            BllUser user = Find(id);
            if (user == null)
                return false;
            userRepo.Remove(user.Id);
            uow.Save();
            return true;
        }

        public bool Remove(BllUser entity)
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
            BllUser user = Find(email);
            if (user == null)
                return false;

            return Crypto.VerifyHashedPassword(user.Password, password);
        }

        private void FillAdditionalProperties(DalUser user)
        {
            if (user as DalProgrammer != null)
            {
                ((DalProgrammer)user).Skills = userSkillRepo.Get(x => x.User.Id == user.Id);
            }
            else if (user as DalManager != null)
            {
                ((DalManager)user).Filters = filterRepo.Get(x => x.UserId == user.Id);
            }
        }

        private void UpdateFilters(BllManager balManager)
        {
            var dbFilters = filterRepo.Get(x => x.UserId == balManager.Id);
            foreach (var filter in balManager.Filters)
            {
                if (dbFilters.Any(x => x.Id == filter.Id))
                    filterRepo.Update(filter.ToDal(balManager.Id));
                else filterRepo.Create(filter.ToDal(balManager.Id));
            }

            foreach (var filter in dbFilters)
            {
                if (!balManager.Filters.Any(x => x.Id == filter.Id))
                    filterRepo.Remove(filter.Id);
            }
        }

        private void UpdateUserSkills(BllProgrammer balProgrammer)
        {
            var dbUserSkills = userSkillRepo.Get(x => x.User.Id == balProgrammer.Id);
            foreach (var skill in balProgrammer.Skills)
            {
                DalUserSkill dalUserSkill = new DalUserSkill
                {
                    User = (DalProgrammer)balProgrammer.ToDal(),
                    Skill = skill.Key.ToDal(),
                    Level = skill.Value
                };

                if (dbUserSkills.Any(x => x.Skill.Id == skill.Key.Id))
                    userSkillRepo.Update(dalUserSkill);
                else userSkillRepo.Create(dalUserSkill);
            }

            foreach (var skill in dbUserSkills)
            {
                if (!balProgrammer.Skills.Any(x => x.Key.Id == skill.Skill.Id))
                {
                    userSkillRepo.Remove(skill);
                }
            }
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
