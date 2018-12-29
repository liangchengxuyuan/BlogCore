using Lyp.BlogCore.IServices.Base;
using Lyp.BlogCore.IRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lyp.BlogCore.Services.Base
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        public IBaseRepository<TEntity> baseDal;
        public async Task<bool> Add(TEntity model)
        {
            return await baseDal.Add(model);
        }

        public async Task<bool> Delete(TEntity model)
        {
            return await baseDal.Delete(model);
        }

        public async Task<bool> DeleteById(object objId)
        {
            return await baseDal.DeleteById(objId);
        }

        public async Task<bool> DeleteByIds(object[] lstIds)
        {
            return await baseDal.DeleteByIds(lstIds);
        }

        public async Task<List<TEntity>> Query()
        {
            return await baseDal.Query();
        }
        public List<TEntity> QueryWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return baseDal.QueryWhere(predicate);
        }
        public async Task<IEnumerable<TEntity>> QueryById(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await baseDal.QueryById(whereExpression);
        }
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await baseDal.Query(whereExpression);
        }

        public async Task<List<TEntity>> QueryOrder(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await baseDal.QueryOrder(whereExpression, orderByExpression, isAsc);
        }

        public async Task<List<TEntity>> QueryPage<TKey>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TKey>> orderByExpression, int intPageIndex, int intPageSize = 20, bool isAsc = true)
        {
            return await baseDal.QueryPage(whereExpression, orderByExpression, intPageIndex, intPageSize, isAsc);
        }

        public async Task<bool> Update(TEntity model)
        {
            return await baseDal.Update(model);
        }
    }
}
