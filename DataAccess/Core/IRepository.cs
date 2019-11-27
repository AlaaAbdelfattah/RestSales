using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RestAPI.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Implemented Methods

        #region Get

        /// <summary>
        /// Get Single Entity 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        TEntity GetSingle(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Get All  [TEntity]
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Get All [ TEntity] Filters with Linq Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Get All [TEntity] For Paging Idea  with Filter Expression
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="isAscendingOrder"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAllPaged<TResult>(
            Expression<Func<TEntity, bool>> expression,
            int pageNum, int pageSize,
            Expression<Func<TEntity, TResult>> orderByProperty,
            bool isAscendingOrder,
            out int rowsCount);

        /// <summary>
        /// Get All [TEntity] For Paging Idea Without Filter Expression
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="isAscendingOrder"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAllPaged<TResult>(
            int pageNum,
            int pageSize,
            Expression<Func<TEntity, TResult>> orderByProperty,
            bool isAscendingOrder,
            out int rowsCount);


        #endregion

        #region Count

        /// <summary>
        /// Count
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// Count with Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> expression);

        #endregion

        #region Others

        /// <summary>
        /// Attach 
        /// </summary>
        /// <param name="entity"></param>
        void Attach(TEntity entity);

        /// <summary>
        /// Detach
        /// </summary>
        /// <param name="entity"></param>
        void Detach(TEntity entity);

        #endregion

        #region Add 

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int Add(TEntity entity);


        #endregion

        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int Update(TEntity entity);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="errors"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int Update(TEntity entity, out List<string> errors);

        #endregion

        #region Delete 

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int Delete(TEntity entity);

        #endregion

        int SaveChanges();

        #endregion



    }
}
