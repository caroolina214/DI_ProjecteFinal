
using MilAventures.Model.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MilAventures.Model.Repositories
{
    public class GenericRepository<T> where T : class
    {
        public readonly MilAventuresContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(MilAventuresContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>Retorna tots els registres.</summary>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>Cerca un registre per clau primària.</summary>
        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>Insereix un nou registre.</summary>
        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>Actualitza un registre existent.</summary>
        public virtual void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>Elimina un registre per clau primària.</summary>
        public virtual void Delete(int id)
        {
            T entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}