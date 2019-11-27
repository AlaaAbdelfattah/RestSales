using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace RestAPI.DataAccess
{

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        #region Fields and Constructors 

        /// <summary>
        /// Db Context 
        /// </summary>
        public SalesEntities _context;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="context"></param>
        public Repository()
        {
            _context = new SalesEntities();
            Dispose(false);
        }

        /// <summary>
        /// _dispose .
        /// </summary>
        private bool _disposed;

        #endregion

        #region Implemented Methods 

        #region Get

        /// <summary>
        /// Get single TEntity 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> expression)
        {
            using (var dbContextTransactions = _context.Database.BeginTransaction())
            {
                try
                {
                    TEntity result = _context.Set<TEntity>().Where(expression).FirstOrDefault();
                    //Commit transactions
                    dbContextTransactions.Commit();
                    return result;
                }
                catch (DbEntityValidationException ex)
                {
                    //Rollback all changes .
                    dbContextTransactions.Rollback();
                    string errorMessages = string.Join("; ",
                        ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                    throw new DbEntityValidationException(errorMessages);
                }
            }
        }

        /// <summary>
        /// Return List  Of [TEntity]
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            using (var dbContextTransactions = _context.Database.BeginTransaction())
            {
                try
                {
                    IEnumerable<TEntity> list = _context.Set<TEntity>().ToList();
                    //Commit transactions
                    dbContextTransactions.Commit();
                    return list;
                }
                catch (DbEntityValidationException ex)
                {
                    //Rollback all changes .
                    dbContextTransactions.Rollback();
                    string errorMessages = string.Join("; ",
                        ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                    throw new DbEntityValidationException(errorMessages);
                }
            }
        }

        /// <summary>
        /// Return List  Of [TEntity] with [Expression]
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            using (var dbContextTransactions = _context.Database.BeginTransaction())
            {
                try
                {
                    IEnumerable<TEntity> list = _context.Set<TEntity>().Where(expression).ToList();
                    //Commit transactions
                    dbContextTransactions.Commit();
                    return list;
                }
                catch (DbEntityValidationException ex)
                {
                    //Rollback all changes .
                    dbContextTransactions.Rollback();
                    string errorMessages = string.Join("; ",
                        ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                    throw new DbEntityValidationException(errorMessages);
                }
            }
        }

        /// <summary>
        /// Get All  [TEntity] with Expression and Pagging 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="isAscendingOrder"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAllPaged<TResult>(Expression<Func<TEntity, bool>> expression,
            int pageNum,
            int pageSize, Expression<Func<TEntity, TResult>> orderByProperty,
            bool isAscendingOrder, out int rowsCount)
        {
            using (var dbContextTransactions = _context.Database.BeginTransaction())
            {
                try
                {
                    IQueryable<TEntity> query = _context.Set<TEntity>().Where(expression);
                    if (pageSize <= 0)
                    {
                        pageSize = 10;
                    }

                    rowsCount = query.Count();
                    if (rowsCount <= pageSize || pageNum <= 0)
                    {
                        pageNum = 1;
                    }

                    int excludedRows = (pageNum - 1) * pageSize;
                    query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);
                    //Commit transactions
                    dbContextTransactions.Commit();
                    return query.Skip(excludedRows).Take(pageSize);
                }
                catch (DbEntityValidationException ex)
                {
                    //Rollback all changes .
                    dbContextTransactions.Rollback();
                    string errorMessages = string.Join("; ",
                        ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                    throw new DbEntityValidationException(errorMessages);
                }
            }
        }

        /// <summary>
        /// Get All  [TEntity] and Pagging without Expression  
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="isAscendingOrder"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAllPaged<TResult>(int pageNum, int pageSize,
            Expression<Func<TEntity, TResult>> orderByProperty,
            bool isAscendingOrder, out int rowsCount)
        {
            using (var dbContextTransactions = _context.Database.BeginTransaction())
            {
                try
                {
                    IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
                    if (pageSize <= 0)
                    {
                        pageSize = 20;
                    }
                    rowsCount = query.Count();
                    if (rowsCount <= pageSize || pageNum <= 0)
                    {
                        pageNum = 1;
                    }
                    int excludedRows = (pageNum - 1) * pageSize;
                    query = isAscendingOrder ? query.OrderBy(orderByProperty)
                        : query.OrderByDescending(orderByProperty);
                    //Commit transactions
                    dbContextTransactions.Commit();
                    return query.Skip(excludedRows).Take(pageSize);
                }
                catch (DbEntityValidationException ex)
                {
                    //Rollback all changes .
                    dbContextTransactions.Rollback();
                    string errorMessages = string.Join("; ",
                        ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                    throw new DbEntityValidationException(errorMessages);
                }
            }
        }

        #endregion

        #region Count

        /// <summary>
        /// return count of TEntity
        /// </summary>
        /// <returns></returns>
        long IRepository<TEntity>.Count()
        {
            using (var dbContextTransactions = _context.Database.BeginTransaction())
            {
                try
                {
                    int count = _context.Set<TEntity>().Count();
                    //Commit transactions
                    dbContextTransactions.Commit();
                    return count;
                }
                catch (DbEntityValidationException ex)
                {
                    //Rollback all changes .
                    dbContextTransactions.Rollback();
                    string errorMessages = string.Join("; ",
                        ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                    throw new DbEntityValidationException(errorMessages);
                }
            }


        }

        /// <summary>
        /// return count of TEntity with Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        long IRepository<TEntity>.Count(Expression<Func<TEntity, bool>> expression)
        {
            using (var dbContextTransactions = _context.Database.BeginTransaction())
            {
                try
                {
                    int count = _context.Set<TEntity>().Where(expression).Count();
                    //Commit transactions
                    dbContextTransactions.Commit();
                    return count;
                }
                catch (DbEntityValidationException ex)
                {
                    //Rollback all changes .
                    dbContextTransactions.Rollback();
                    string errorMessages = string.Join("; ",
                        ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                    throw new DbEntityValidationException(errorMessages);
                }
            }
        }

        #endregion

        #region Others 

        /// <summary>
        /// Attach  TEntity
        /// </summary>
        /// <param name="entity"></param>
        void IRepository<TEntity>.Attach(TEntity entity)
        {
            using (var dbContextTransactions = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<TEntity>().Attach(entity);
                    _context.SaveChanges();
                    //Commit transactions
                    dbContextTransactions.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    //Rollback all changes .
                    dbContextTransactions.Rollback();
                    string errorMessages = string.Join("; ",
                        ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                    throw new DbEntityValidationException(errorMessages);
                }
            }
        }

        /// <summary>
        /// Detach TEntity
        /// </summary>
        /// <param name="entity"></param>
        void IRepository<TEntity>.Detach(TEntity entity)
        {
            using (var dbContextTransactions = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<TEntity>().Remove(entity);
                    _context.SaveChanges();
                    //Commit transactions
                    dbContextTransactions.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    //Rollback all changes .
                    dbContextTransactions.Rollback();
                    string errorMessages = string.Join("; ",
                        ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                    throw new DbEntityValidationException(errorMessages);
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> to release both managed and unmanaged resources; 
        ///   <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion 

        #region Add

        /// <summary>
        ///  Add Test Method 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                _context.Entry(entity).State = EntityState.Added;
                return 1;
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Add Or Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().AddOrUpdate(entity);
                //Change Entity State 
                _context.Entry(entity).State = EntityState.Modified;
                return 1;
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Int32.</returns>
        public int Update(TEntity entity, out List<string> errors)
        {
            try
            {
                errors = null;
                _context.Set<TEntity>().AddOrUpdate(entity);
                //Change Entity State 
                _context.Entry(entity).State = EntityState.Modified;
                return 1;
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Remove 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                //Change Entity State 
                _context.Entry(entity).State = EntityState.Deleted;
                return 1;
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        #endregion

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

    }
}
