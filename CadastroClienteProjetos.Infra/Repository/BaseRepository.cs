using CadastroClienteProjetos.Domain.Interfaces;
using CadastroClienteProjetos.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CadastroClienteProjetos.Infra.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        readonly DbSet<T> dbSet;
        readonly SQLServerContext _dbContext;
        public BaseRepository(SQLServerContext dbContext)
        {
            _dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public IEnumerable<T> FindSingleBy(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
                return dbSet.Where(predicate).ToList();
            else
                throw new ArgumentNullException("Predicate value must be passed to FindSingleBy<T>.");
        }

        public T GetById(object id)
        {
            return dbSet.Find(id);
        }

        public T Insert(T obj)
        {
            dbSet.Add(obj);
            return obj;
        }

        public List<T> Insert(List<T> obj)
        {
            dbSet.AddRange(obj);
            return obj;
        }

        public void Delete(object id)
        {
            var entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(T entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
               dbSet.Attach(entityToDelete);

            dbSet.Remove(entityToDelete);
        }

        public T Update(T obj)
        {
            dbSet.Attach(obj);
            _dbContext.Entry(obj).State = EntityState.Modified;
            return obj;
        }

        public List<T> Update(List<T> obj)
        {
            dbSet.UpdateRange(obj);
            return obj;
        }

        public int Save()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (Exception dbEx)
            {
                throw new Exception(dbEx.Message, dbEx.InnerException);
            }
        }

        public int Save(bool changes)
        {
            return _dbContext.SaveChanges(changes);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                    _dbContext.Dispose();
            }
        }

    }
}