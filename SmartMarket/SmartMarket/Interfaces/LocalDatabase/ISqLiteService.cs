using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SmartMarket.Models;

namespace SmartMarket.Interfaces.LocalDatabase
{
    public interface ISqLiteService
    {
        #region Get Path
        //IDatabaseConnection DbConnection { get; }
        #endregion

        #region Gets

        T GetWithChildren<T>(string primaryKey, bool isRecursive = false) where T : class, new();

        IEnumerable<T> GetAllWithChildren<T>(Expression<Func<T, bool>> predicate, bool recursively = true, bool includeInactiveObjects = false)
            where T : class, new();
         T Get<T>(string primarykey) where T : class, new();
        T Get<T>(Expression<Func<T, bool>> predicate) where T : class, new();
        IEnumerable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class, new();

        #endregion

        #region Inserts

        // Insert new object
        int Insert<T>(T obj);

        int InsertAll<T>(IEnumerable<T> list);

        void InsertWithChildren<T>(T obj, bool isRecursive = false);

        #endregion

        #region Updates

        int Update<T>(T obj);

        void UpdateWithChildren<T>(T obj);

        int UpdateAll<T>(IEnumerable<T> list);

        #endregion

        #region Deletes

        int Delete<T>(string id);

        void Delete<T>(T obj, bool isRecursive = false);

        int DeleteAll<T>();

        void DeleteAll<T>(IEnumerable<T> obj);

        #endregion


        #region App Settings

        AppSettings GetSettings();

        #endregion

    }
}
