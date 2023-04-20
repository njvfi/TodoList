using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.DAL.Contexts;
using TodoList.DAL.Entities;

namespace TodoList.DAL.Repositories
{
    public class GoalRepository
    {
        private readonly GoalContext _goalContext;

        public GoalRepository(GoalContext goalContext)
        {
            _goalContext = goalContext;
        }
        #region Goal
        public async Task<List<Goal>> GetGoalsAsync()
        {
            return await _goalContext.Goals.Include<Goal, Entities.Type>(pr => pr.Type).ToListAsync();
        }

        public async Task<List<Goal>> GetGoalsAsync(int? type, string? name, int userid)
        {
            IQueryable<Goal> goal = _goalContext.Goals.Include<Goal, Entities.Type>(g => g.Type).Where(g => g.UserId == userid);

            //goal = await _goalContext.Goals.Where(g => g.UserId == userid).ToListAsync();

            if (type != null && type != 0)
            {
                goal = goal.Where(g => g.TypeId == type);
            }
            if (!string.IsNullOrEmpty(name))
            {
                goal = goal.Where(g => g.Name!.Contains(name));
            }

            return await goal.ToListAsync();
        }

        public async Task<List<Goal>> GetGoalsAsync(int? type, string? name, int userid, SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Goal> goal = _goalContext.Goals.Include<Goal, Entities.Type>(g => g.Type).Where(g => g.UserId == userid);

            if (type != null && type != 0)
            {
                goal = goal.Where(g => g.TypeId == type);
            }
            if (!string.IsNullOrEmpty(name))
            {
                goal = goal.Where(g => g.Name!.Contains(name));
            }

            goal = sortOrder switch
            {
                SortState.NameDesc => goal.OrderByDescending(s => s.Name),
                SortState.PriceAsc => goal.OrderBy(s => s.Time),
                SortState.PriceDesc => goal.OrderByDescending(s => s.Time),
                SortState.TypeAsc => goal.OrderBy(s => s.Type!.Name),
                SortState.TypeDesc => goal.OrderByDescending(s => s.Type!.Name),
                _ => goal.OrderBy(s => s.Name),
            };

            return await goal.ToListAsync();
        }

        public async Task<Goal> GetGoalIncludeAsync(int id)
        {
            return await _goalContext.Goals.Include<Goal, Entities.Type>(pr => pr.Type).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Goal> GetGoalAsync(int id)
        {
            return await _goalContext.Goals.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateGoalAsync(Goal goal)
        {
            _goalContext.Goals.Add(goal);
            await _goalContext.SaveChangesAsync();
        }

        public async Task EditGoalAsync(Goal goal)
        {
            _goalContext.Goals.Update(goal);
            await _goalContext.SaveChangesAsync();
        }

        public async Task DeleteGoalAsync(int id)
        {
            Goal goal = new Goal { Id = id };
            _goalContext.Entry(goal).State = EntityState.Deleted;
            await _goalContext.SaveChangesAsync();
        }

        public async Task SwitchGoalStatusAsync(int id)
        {
            var goal = await GetGoalAsync(id);
            goal.Status = !goal.Status;
            await EditGoalAsync(goal);
        }
        #endregion

        public async Task<List<Entities.Type>> GetTypesAsync()
        {
            return await _goalContext.Types.ToListAsync();
        }

        public async Task<bool> LoginAsync(User user)
        {
            var result = await _goalContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
            user.Id = result.Id;
            return result != null;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            var result = await _goalContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
            if (result == null)
            {
                Entities.Type not_defined = new Entities.Type { Name = "Без категорії" };
                Goal initgoal = new Goal { Name = "Ознайомитися з платформою", Description = "Ознайомитися з платформою, навчитися користуватися всіма можливостями", Type = not_defined, Status = false, UserId = user.Id };
                User newbe = new User { Email = user.Email, Name = user.Name, Password = user.Password};
                newbe.Goals.Add(initgoal);
                _goalContext.Users.Add(newbe);
                _goalContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task InitAsync() 
        {
            // добавим начальные данные для тестирования
            if (!_goalContext.Types.Any())
            {
                Entities.Type not_defined = new Entities.Type { Name = "Без категорії" };
                Entities.Type home = new Entities.Type { Name = "Роботи по дому" };
                Entities.Type sport = new Entities.Type { Name = "Спорт" };
                Entities.Type work = new Entities.Type { Name = "Робота" };

                Goal goal1 = new Goal { Name = "Ознайомитися з платформою", Description = "Ознайомитися з платформою, навчитися користуватися всіма можливостями", Type = not_defined, Status = false, UserId = 0 };

                User tom = new User {Email = "test@gmail.com", Name = "Test",  Id = 0, Password = "test"};
                tom.Goals.Add(goal1);

                _goalContext.Types.AddRange(not_defined, home, home, work);
                _goalContext.Goals.Add(goal1);
                _goalContext.Users.Add(tom);
                _goalContext.SaveChanges();
            }
        }
    }
}
