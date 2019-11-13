using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace HKLite
{
    internal class Dao<T>: IDao<T>
    {
        private DALayer dalayer;
        private TransacBuilder tranBuilder;
        internal Dao(DALayer dalayer)
        {
            this.dalayer = dalayer;
        }

        internal Dao(TransacBuilder tranBuilder)
        {
            this.tranBuilder = tranBuilder;
        }

        #region Builder
        public IQueryBuilder<T> QueryBuilder()
        {
            if (dalayer != null)
                return dalayer.QueryBuilder<T>();
            else if (tranBuilder != null)
                return tranBuilder.QueryBuilder<T>();
            return null;
        }

        public IInsertBuilder<T> InsertBuilder()
        {
            if (dalayer != null)
                return dalayer.InsertBuilder<T>();
            else if (tranBuilder != null)
                return tranBuilder.InsertBuilder<T>();
            return null;
        }

        public IUpdateBuilder<T> UpdateBuilder()
        {
            if (dalayer != null)
                return dalayer.UpdateBuilder<T>();
            else if (tranBuilder != null)
                return tranBuilder.UpdateBuilder<T>();
            return null;
        }

        public IDeleteBuilder<T> DeleteBuilder()
        {
            if (dalayer != null)
                return dalayer.DeleteBuilder<T>();
            else if (tranBuilder != null)
                return tranBuilder.DeleteBuilder<T>();
            return null;
        }

        #endregion

        public T QueryByKey(object keyValue)
        {
            var qBuilder = QueryBuilder();
            qBuilder.QueryByKey(keyValue);
            return qBuilder.QuerySingle();
        }

        public List<T> QueryAll()
        {
            var qBuilder = QueryBuilder();
            return qBuilder.QueryAll();
        }

        public int Insert(T entity)
        {
            var iBuilder = InsertBuilder();
            iBuilder.Insert(entity);
            if (tranBuilder == null)
                return iBuilder.Run();
            return 0;
        }

        public int UpdateByKey(T entity)
        {
            var uBuilder = UpdateBuilder();
            uBuilder.UpdateByKey(entity);
            if (tranBuilder == null)
                return uBuilder.Run();
            return 0;
        }

        public int DeleteByKey(object keyValue)
        {
            var dBuilder = DeleteBuilder();
            dBuilder.DeleteByKey(keyValue);
            if (tranBuilder == null)
                return dBuilder.Run();
            return 0;
        }

        public int DeleteAll()
        {
            var dBuilder = DeleteBuilder();
            dBuilder.Delete();
            if (tranBuilder == null)
                return dBuilder.Run();
            return 0;
        }

    }
}
