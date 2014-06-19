using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Data;
using System.Linq.Expressions;

namespace RnD.TVLSec.Models
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private AppDbEntities _appDbEntities = new AppDbEntities();
        private ObjectContext _context;

        private readonly IObjectSet<T> _iObjectSet;

        protected RepositoryBase()
        {

            try
            {
                _iObjectSet = DataContext.CreateObjectSet<T>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected ObjectContext DataContext
        {
            get { return _context ?? (_context = new ObjectContext(_appDbEntities.Connection.ConnectionString)); }
        }

        #region Get, Insert, Update, Delete

        public virtual void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _iObjectSet.AddObject(entity);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            EntityKey key = GenerateKey(entity);
            var originalEntity = (T)_context.GetObjectByKey(key);
            string qualifiedEntitySetName = this.GetEntitySetName(typeof(T));
            Type parentType = entity.GetType();
            this.SetInsertAuditInfo(entity, originalEntity, parentType);
            _context.ApplyCurrentValues(qualifiedEntitySetName, entity);
        }

        private void SetInsertAuditInfo(T updatedEntity, T originalEntity, Type type)
        {
            try
            {
                System.Reflection.PropertyInfo pIUser = type.GetProperty("IUser");
                System.Reflection.PropertyInfo pIDate = type.GetProperty("IDate");

                string IUser = string.Empty;
                DateTime IDate = DateTime.Now;

                if (pIUser != null)
                {
                    string user = Convert.ToString(pIUser.GetValue(originalEntity, null));
                    pIUser.SetValue(updatedEntity, user, null);
                }

                if (pIDate != null)
                {
                    IDate = Convert.ToDateTime(pIDate.GetValue(originalEntity, null));
                    pIDate.SetValue(updatedEntity, IDate, null);
                }

            }
            catch (Exception ex)
            {
                //if property does not exist, just suppress it
            }
        }

        public EntityKey GenerateKey(T t)
        {
            //Int32 value = 0;
            Type type = t.GetType();
            System.Reflection.PropertyInfo ID = type.GetProperty("Id");
            var value = Convert.ToInt32(ID.GetValue(t, null));


            IEnumerable<KeyValuePair<string, object>> entityKeyValues = new KeyValuePair<string, object>[] { 
                new KeyValuePair<string, object>("Id", value) };

            string qualifiedEntitySetName = this.GetEntitySetName(typeof(T));
            EntityKey key = new EntityKey(qualifiedEntitySetName, entityKeyValues);

            return key;
        }

        public EntityKey GenerateKey(T t, string keyName)
        {
            //Int32 value = 0;
            object value = 0;
            Type type = t.GetType();
            System.Reflection.PropertyInfo ID = type.GetProperty(keyName);
            //value = Convert.ToInt32(ID.GetValue(t, null));
            value = ID.GetValue(t, null);

            IEnumerable<KeyValuePair<string, object>> entityKeyValues = new KeyValuePair<string, object>[] { 
                new KeyValuePair<string, object>(keyName, value) };

            string qualifiedEntitySetName = this.GetEntitySetName(typeof(T));
            EntityKey key = new EntityKey(qualifiedEntitySetName, entityKeyValues);

            return key;
        }

        private string GetEntitySetName(Type t)
        {
            string qualifiedEntitySetName = _context.DefaultContainerName + "." + t.Name;
            return qualifiedEntitySetName;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _iObjectSet.DeleteObject(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _iObjectSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException("obj");
                }
                _iObjectSet.DeleteObject(obj);
            }
        }

        public virtual T GetById(long id)
        {
            // Define the entity key values.
            IEnumerable<KeyValuePair<string, object>> entityKeyValues =
                new KeyValuePair<string, object>[] { 
                new KeyValuePair<string, object>("Id", id) };

            string qualifiedEntitySetName = _context.DefaultContainerName + "." + typeof(T).Name;
            EntityKey key = new EntityKey(qualifiedEntitySetName, entityKeyValues);

            try
            {
                return (T)_context.GetObjectByKey(key);
            }
            catch
            {
                return null;
            }
        }

        public virtual List<T> GetAll()
        {
            return _iObjectSet.ToList();
        }

        public virtual List<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _iObjectSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _iObjectSet.Where(where).FirstOrDefault<T>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}