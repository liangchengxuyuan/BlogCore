using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lyp.BlogCore.IRepository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> Query();

        //Task<List<TEntity>> Query(string strWhere);
        Task<IEnumerable<TEntity>> QueryById(Expression<Func<TEntity, bool>> whereExpression);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

        //Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderFileds);

        Task<List<TEntity>> QueryOrder(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        Task<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression,Expression<Func<TEntity,bool>> orderByExpression, int intPageIndex, int intPageSize = 20, bool isAsc = true);

        //Task<List<TEntity>> QueryById(object objId);

        //Task<TEntity> QueryByIds(object[] lstIds);

        Task<int> Add(TEntity model);

        Task<bool> DeleteById(object objId);

        Task<bool> Delete(TEntity model);

        Task<bool> DeleteByIds(object[] lstIds);

        Task<bool> Update(TEntity model);
    }
}
