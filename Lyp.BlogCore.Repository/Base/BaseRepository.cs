using Lyp.BlogCore.IRepository.Base;
using Lyp.BlogCore.Repository.MySqlEFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lyp.BlogCore.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class,new()
    {
        //private readonly MySqlDbContext db=new MySqlDbContext();
        private readonly MySqlDbContext db;
        internal MySqlDbContext dbContext { get; set; }

        internal DbSet<TEntity> _dbSet;

        public BaseRepository(MySqlDbContext dbContext)
        {
            this.db = dbContext;
            this.dbContext = dbContext;
            _dbSet = db.Set<TEntity>();
        }
        public async Task<bool> Add(TEntity model)
        {
            await _dbSet.AddAsync(model);
            return await dbContext.SaveChangesAsync()>0;
        }

        public async Task<bool> Delete(TEntity model)
        {
            _dbSet.Remove(model);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteById(object objId)
        {
            var model = await _dbSet.FindAsync(objId);
            _dbSet.Remove(model);
            return await dbContext.SaveChangesAsync()>0;
        }

        public async Task<bool> DeleteByIds(object[] lstIds)
        {
            var list = await _dbSet.FindAsync(lstIds);
            _dbSet.RemoveRange(list);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<TEntity>> Query()
        {
            return await Task.Run(() => _dbSet.ToList());
        }
        //public async Task<List<TEntity>> Query(string strWhere)
        //{
        //    //_dbSet.FindAsync(strWhere);
        //    return await Task.Run(()=>_dbSet.FindAsync(strWhere)) 

        //}
        public List<TEntity> QueryWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();

        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return whereExpression != null ? await Task.Run(() => _dbSet.Where(whereExpression).AsNoTracking().ToList()) : await Task.Run(() => _dbSet.AsQueryable<TEntity>().AsNoTracking().ToList());
        }

        public async Task<List<TEntity>> QueryOrder(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            if(isAsc)
            {
                return whereExpression != null ? await Task.Run(() => _dbSet.Where(whereExpression).OrderBy(orderByExpression).AsNoTracking().ToList()) : await Task.Run(() => _dbSet.AsNoTracking<TEntity>().ToList());
            }
            return whereExpression != null ? await Task.Run(() => _dbSet.Where(whereExpression).OrderByDescending(orderByExpression).AsNoTracking().ToList()) : await Task.Run(() => _dbSet.AsNoTracking<TEntity>().ToList());
        }

        public async Task<IEnumerable<TEntity>> QueryById(Expression<Func<TEntity, bool>> whereExpression)
        {
            return whereExpression != null ? await Task.Run(() => _dbSet.Where(whereExpression).AsNoTracking()) : await Task.Run(() => _dbSet.AsQueryable<TEntity>().AsNoTracking());
        }

        public async Task<List<TEntity>> QueryPage<TKey>(Expression<Func<TEntity, bool>> whereExpression,Expression<Func<TEntity, TKey>> orderByExpression, int intPageIndex, int intPageSize = 20, bool isAsc=true)
        {
            int rowcount = _dbSet.Count(whereExpression);
            if(isAsc)
            {
                return await Task.Run(() => _dbSet.Where(whereExpression).OrderBy(orderByExpression).Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).AsNoTracking().ToList());
            }
            return await Task.Run(() => _dbSet.Where(whereExpression).OrderByDescending(orderByExpression).Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).AsNoTracking().ToList());
        }

        public async Task<bool> Update(TEntity model)
        {
            _dbSet.Update(model);
            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
