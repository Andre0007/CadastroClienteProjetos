using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CadastroClienteProjetos.Domain.Interfaces
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> GetAll();       

        IEnumerable<T> FindSingleBy(Expression<Func<T, bool>> predicate);

        T GetById(object Id);

        T Insert(T obj);

        List<T> Insert(List<T> obj);

        void Delete(object Id);

        T Update(T obj);

        List<T> Update(List<T> obj);

        int Save();
    }
}