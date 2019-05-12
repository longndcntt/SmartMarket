using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace SmartMarket.Services.SQLiteService
{
    public class SqLiteService : ISqLiteService
    {
        #region Properties

        protected const string DatabaseName = "SmartMarket.db";

        private SQLiteConnection _database;

        protected static readonly object Locker = new object();

        public IDatabaseConnection DbConnection { get; private set; }

        #endregion

        #region Constructors

        public SqLiteService(IDatabaseConnection dbConnection)
        {
            DbConnection = dbConnection;
            Init();
        }

        #endregion

        #region Inits

        private void Init()
        {
            if (_database != null) return;

            // Connect database
            _database = DbConnection.SqliteConnection(DatabaseName);

            // Create database
            var listTable = new List<Type>
            {
                typeof(Order),
                typeof(OrderDetails),
                typeof(UserModel),
                typeof(ReviewProduct),
                typeof(ItemModel),
                typeof(Category),
                typeof(ItemDetails),
                typeof(ImageItemDetail),
                typeof(AppSettings),
            };

            foreach (var table in listTable)
            {
                CreateTable(table);
            }
        }

        #endregion

        #region Create Database

        private void CreateTable<T>(T table) where T : Type
        {
            lock (Locker)
            {
                try
                {
                    _database.CreateTable(table);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"CreateTable: {e}");
#endif
                }
            }
        }

        #endregion

        #region Gets

        /// <summary>
        /// Return data of multiple table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey"></param>
        /// <param name="isRecursive"></param>
        /// <returns></returns>
        public T GetWithChildren<T>(string primaryKey, bool isRecursive = false) where T : class, new()
        {
            lock (Locker)
            {
                try
                {
                    return _database.GetWithChildren<T>(primaryKey, isRecursive);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"GetWithChildren - primaryKey: {e}");
#endif
                    return null;
                }
            }
        }

        /// <summary>
        /// Retrieves all data from table <typeparamref>T</typeparamref>.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="recursively">Perform cascade operations specified in object?</param>
        /// <param name="includeInactiveObjects">Include object marked as inactive?</param>
        public IEnumerable<T> GetAllWithChildren<T>(Expression<Func<T, bool>> predicate, bool recursively = true, bool includeInactiveObjects = false) where T : class, new()
        {
            lock (Locker)
            {
                try
                {
                    var list = _database.GetAllWithChildren<T>(recursive: recursively);
                    return list;
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"GetWithChildren - primaryKey: {e}");
#endif
                    return null;
                }
            }
        }

        public T Get<T>(string primarykey) where T : class, new()
        {
            lock (Locker)
            {
                try
                {
                    return _database.Get<T>(primarykey);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"Get - primaryKey: {e}");
#endif
                    return null;
                }
            }
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            try
            {
                lock (Locker)
                {
                    return _database.Table<T>().Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine($"Get - Expression: {e}");
#endif
                return null;
            }
        }

        public IEnumerable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            lock (Locker)
            {
                try
                {
                    return _database.Table<T>().Where(predicate).ToList();
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"Get - Expression: {e}");
#endif
                    return null;
                }
            }
        }

        #endregion

        #region Inserts

        public int Insert<T>(T obj)
        {
            lock (Locker)
            {
                try
                {
                    return _database.InsertOrReplace(obj);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"SQLiteHelper - InsertOrReplace: {e}");
#endif
                    return -1;
                }
            }
        }

        public int InsertAll<T>(IEnumerable<T> list)
        {
            lock (Locker)
            {
                try
                {
                    return _database.InsertAll(list);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"SQLiteHelper - InsertAll: {e}");
#endif
                    return -1;
                }
            }
        }

        public void InsertWithChildren<T>(T obj, bool isRecursive = false)
        {
            lock (Locker)
            {
                try
                {
                    _database.InsertWithChildren(obj, isRecursive);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"SQLiteHelper - InsertWithChildren: {e}");
#endif
                }
            }
        }

        #endregion

        #region Updates

        public int Update<T>(T obj)
        {
            lock (Locker)
            {
                try
                {
                    return _database.Update(obj);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"SQLiteHelper - Update: {e}");
#endif
                    return -1;
                }
            }
        }

        public void UpdateWithChildren<T>(T obj)
        {
            lock (Locker)
            {
                try
                {
                    _database.UpdateWithChildren(obj);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"SQLiteHelper - UpdateWithChildren {e}");
#endif
                }
            }
        }

        public int UpdateAll<T>(IEnumerable<T> list)
        {
            lock (Locker)
            {
                try
                {
                    return _database.UpdateAll(list);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"SQLiteHelper - UpdateAll {e}");
#endif
                    return -1;
                }
            }
        }

        #endregion

        #region Deletes

        public int Delete<T>(string id)
        {
            lock (Locker)
            {
                try
                {
                    return _database.Delete<T>(id);
                }
                catch (Exception e)
                {
#if DEBUG              
                    Debug.WriteLine($"SQLiteHelper - Delete: {e}");
#endif
                    return -1;
                }
            }
        }

        public void Delete<T>(T obj, bool isRecursive = false)
        {
            lock (Locker)
            {
                try
                {
                    _database.Delete(obj, isRecursive);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"SQLiteHelper - void Delete: {e}");
#endif
                }
            }
        }

        public int DeleteAll<T>()
        {
            lock (Locker)
            {
                try
                {
                    return _database.DeleteAll<T>();
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"SQLiteHelper - DeleteAll: {e}");
#endif
                    return -1;
                }
            }
        }

        public void DeleteAll<T>(IEnumerable<T> obj)
        {
            lock (Locker)
            {
                try
                {
                    _database.DeleteAll(obj);
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"SQLiteHelper - DeleteAll: {e}");
#endif
                }
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose of everything
        /// </summary>
        public static void Dispose()
        {

        }

        #endregion


        #region App Settings

        public AppSettings GetSettings()
        {
            lock (Locker)
            {
                try
                {
                    var appSettings = _database.Get<AppSettings>(a => a.Id == 0);

                    if (appSettings == null)
                    {
                        appSettings = new AppSettings();

                        _database.Insert(appSettings);
                    }

                    return appSettings;
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine($"Get Setting Error: {e}");
#endif
                    var appSettings = new AppSettings();

                    _database.Insert(appSettings);

                    return appSettings;
                }
            }
        }

        #endregion

    }
}
