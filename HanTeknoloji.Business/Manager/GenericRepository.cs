using HanTeknoloji.Data.Models.Orm.Context;
using HanTeknoloji.Data.Models.Orm.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Business.Manager
{
    public class GenericRepository<T> : IDisposable where T : BaseEntity
    {
        public HanTeknolojiContext db;
        public DbSet<T> _dbcontext;
        public GenericRepository()
        {
            db = new HanTeknolojiContext();
            _dbcontext = db.Set<T>();
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void Add(T entity)
        {
            if (entity != null)
            {
                entity.AddDate = DateTime.Now;
                entity.IsDeleted = false;
                _dbcontext.Add(entity);
                SaveChanges();
            }
        }

        public List<T> GetAll()
        {
            return _dbcontext.Where(x => x.IsDeleted == false).ToList();
        }

        public List<T> GetListWithQuery(Expression<Func<T, bool>> lambda)
        {
            return _dbcontext.Where(lambda).Where(x => x.IsDeleted == false).ToList();
        }
        
        public void Delete(int id)
        {
            var entity = db.Set<T>().Find(id);

            if (entity != null)
            {
                //entity.DeleteDate = DateTime.Now;
                entity.IsDeleted = true;
                db.SaveChanges();
            }
        }

        public T Find(int id)
        {
            var entity = db.Set<T>().Find(id);
            return entity;
        }

        public T First()
        {
            var entity = db.Set<T>().First();
            return entity;
        }

        public T FirstOrDefault(Expression<Func<T, bool>> lambda)
        {
            return _dbcontext.FirstOrDefault(lambda);
        }

        public bool Any(Expression<Func<T, bool>> lambda)
        {
            return _dbcontext.Where(x => x.IsDeleted == false).Any(lambda);
        }

        public void Dispose()
        {
            db.Dispose();
            GC.SuppressFinalize(db);
        }
    }
}
