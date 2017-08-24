/*********************************************************
 * CopyRight: QIXIAO CODE BUILDER. 
 * Version:4.2.0
 * Author:qixiao(柒小)
 * Create:2017-08-24 17:48:50
 * E-mail: dong@qixiao.me | wd8622088@foxmail.com 
 * Personal WebSit: http://qixiao.me 
 * Technical WebSit: http://www.cnblogs.com/qixiaoyizhan/ 
 * Description:Bankinate Upgrade,No Object!,No OO Design!
 * Thx , Best Regards ~
 *********************************************************/
using QX_Frame.Bantina.Extends;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace QX_Frame.Bantina.BankinateAuto
{
    public interface IBankinateAuto { }
    public abstract class BankinateAuto : Sql_Helper_DG, IBankinateAuto, IDisposable
    {
        /// <summary>
        /// ConnectionString_READ
        /// </summary>
        public string ConnectionString_READ { get; set; }
        /// <summary>
        /// ConnectionString_WRITE
        /// </summary>
        public string ConnectionString_WRITE { get; set; }
        /// <summary>
        /// DataBaseName
        /// </summary>
        public string DataBaseName { get; set; }
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Execute SqlStatement
        /// </summary>
        public string SqlStatement { get; set; }
        /// <summary>
        /// Result Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// TableInfo
        /// </summary>
        private TableInfo TableInfo { get; set; }
        /// <summary>
        /// define execute result
        /// </summary>

        #region Constructor
        /// <summary>
        /// Bantina
        /// </summary>
        public BankinateAuto()
        {
            if (string.IsNullOrEmpty(ConnString_Default))
            {
                throw new Exception_DG("ConnString_Default Must Be Declared When Initiation ! -- QX_Frame.Bantina.Bankinate");
            }
            this.ConnectionString_READ = ConnString_Default;
            this.ConnectionString_WRITE = ConnString_Default;
            this.DataBaseName = GetDataBaseNameFromDBConnection();
        }

        /// <summary>
        /// Bantina
        /// </summary>
        /// <param name="connString"></param>
        public BankinateAuto(string connString)
        {
            ConnString_Default = connString;
            this.ConnectionString_READ = ConnString_Default;
            this.ConnectionString_WRITE = ConnString_Default;
            this.DataBaseName = GetDataBaseNameFromDBConnection();
        }

        /// <summary>
        /// Bantina
        /// </summary>
        /// <param name="connString_RW"></param>
        /// <param name="connString_R"></param>
        public BankinateAuto(string connString_RW, string connString_R)
        {
            ConnString_RW = connString_RW;
            ConnString_R = connString_R;
            this.ConnectionString_READ = ConnString_RW;
            this.ConnectionString_WRITE = ConnString_R;
            ConnString_Default = connString_R;
            this.DataBaseName = GetDataBaseNameFromDBConnection();
        }

        #endregion

        #region Insert

        public bool Insert(string tableName, IDictionary<string, object> keyValueDic)
        {
            return this.CommonFlow(tableName, () =>
             {
                 this.Message = "add success";
                 return true;
             });

            //Type entityType = typeof(TEntity);

            //string tableName = GetTablaName(entityType);

            //StringBuilder builder_front = new StringBuilder();
            //StringBuilder builder_behind = new StringBuilder();

            //List<SqlParameter> sqlParameterList = new List<SqlParameter>();

            //builder_front.Append("INSERT INTO ");
            //builder_front.Append(tableName);
            //builder_front.Append(" (");
            //builder_behind.Append(" VALUES (");

            //PropertyInfo[] propertyInfos = entityType.GetProperties();
            //foreach (PropertyInfo propertyInfo in propertyInfos)
            //{
            //    //AutoIncrease : if property is auto increase attribute skip this column.
            //    if (propertyInfo.GetCustomAttribute(typeof(AutoIncreaseAttribute), true) is AutoIncreaseAttribute autoIncreaseAttr)
            //    {
            //        continue;
            //    }

            //    //key :
            //    if (propertyInfo.GetCustomAttribute(typeof(KeyAttribute), true) is KeyAttribute keyAttr)
            //    {
            //        builder_front.Append(propertyInfo.Name);
            //        builder_front.Append(",");

            //        builder_behind.Append("@");
            //        builder_behind.Append(propertyInfo.Name);
            //        builder_behind.Append(",");

            //        sqlParameterList.Add(new SqlParameter("@" + propertyInfo.Name, propertyInfo.GetValue(entity)));
            //    }

            //    //Column :
            //    if (propertyInfo.GetCustomAttribute(typeof(ColumnAttribute), true) is ColumnAttribute columnAttr)
            //    {
            //        builder_front.Append(propertyInfo.Name);
            //        builder_front.Append(",");

            //        builder_behind.Append("@");
            //        builder_behind.Append(propertyInfo.Name);
            //        builder_behind.Append(",");

            //        sqlParameterList.Add(new SqlParameter("@" + propertyInfo.Name, propertyInfo.GetValue(entity)));
            //    }

            //    //the end
            //    if (propertyInfos.Last() == propertyInfo)
            //    {
            //        builder_front.Remove(builder_front.Length - 1, 1);
            //        builder_front.Append(")");

            //        builder_behind.Remove(builder_behind.Length - 1, 1);
            //        builder_behind.Append(")");
            //    }
            //}

            ////Generate SqlStatement
            //string sql = builder_front.Append(builder_behind.ToString()).ToString();

            //this.SqlStatement = sql;

            //bool result = false;

            ////Execute Task That Execute SqlStatement
            //await Task.Run(() =>
            //{
            //    result = ExecuteNonQuery(sql, System.Data.CommandType.Text, sqlParameterList.ToArray()) > 0;
            //});

            //// Cache Deal:if Table change,we should renew the cache value
            //HttpRuntimeCache_Helper_DG.Cache_Add(tableName, result);
        }

        #endregion

        #region Update



        #endregion

        #region Delete



        #endregion

        #region Select



        #endregion

        #region prepare method

        /// <summary>
        /// Execute Func Filter:deal Exception from BankinateAuto
        /// </summary>
        /// <param name="executeFunc"></param>
        /// <returns></returns>
        private bool CommonFlow(string tableName, Func<bool> executeFunc)
        {
            //try
            //{
            this.TableName = tableName.Trim();

            //query table feilds info
            string queryFeildSql = $"use [{this.DataBaseName}] SELECT a.[id] as 'TableId', a.colid as 'ColumnId', a.[name] as 'FeildName',a.length as 'Length',c.[name] as 'Type',a.isnullable as 'Nullable' FROM syscolumns a left join systypes c on a.xtype = c.xusertype inner join sysobjects d on a.id = d.id and d.xtype = 'U' where d.name = '{this.TableName}'";

            List<TableFeildsInfo> tableFeildsInfoList = TableConstructionCache($"{this.DataBaseName}{this.TableName}{queryFeildSql}", () =>
            {
                return Return_List_T_ByDataSet<TableFeildsInfo>(ExecuteDataSet(queryFeildSql));
            });

            TableInfo tableInfo = new TableInfo();
            tableInfo.TableId = tableFeildsInfoList.FirstOrDefault() == null ? 0 : tableFeildsInfoList.FirstOrDefault().TableId;
            tableInfo.TableName = this.TableName;
            tableInfo.TableFildsInfoList = tableFeildsInfoList;
            tableInfo.ForeignTableInfoList = GetForeignRelationTableFeildsInfo(tableInfo);

            this.TableInfo = tableInfo;
            return executeFunc();
            //}
            //catch (Exception ex)
            //{
            //    this.Message = ex.ToString();
            //    return false;
            //}
        }

        /// <summary>
        /// GetDataBaseNameFromDBConnection
        /// </summary>
        /// <returns></returns>
        private string GetDataBaseNameFromDBConnection()
        {
            string[] propertieArray = ConnString_Default.Split(';');
            foreach (var item in propertieArray)
            {
                if (item.ToLower().Contains("catalog"))
                {
                    return item.Split('=').Last().Trim();
                }
                if (item.ToLower().Contains("database"))
                {
                    return item.Split('=').Last().Trim();
                }
            }
            throw new KeyNotFoundException("the database name not found from connection string ! -- qixiao");
        }


        private List<TableInfo> GetForeignRelationTableFeildsInfo(TableInfo tableInfo)
        {
            //query foreign relation
            string queryForeignRelationSql = $"select a.constid as 'ForeignRelationId',a.fkeyid as 'ParentTableId',a.rkeyid as 'RelationTableId',a.fkey as 'ForeignKeyId',a.rkey as 'RelationKeyId' from sys.sysforeignkeys a";
            List<ForeignRelatioin> foreignRelationList = TableConstructionCache($"{this.DataBaseName}{this.TableName}{queryForeignRelationSql}", () =>
            {
                return Return_List_T_ByDataSet<ForeignRelatioin>(ExecuteDataSet(queryForeignRelationSql));
            }).Where(t => t.ParentTableId == tableInfo.TableId).ToList();

            List<TableInfo> foreignTableInfoList = new List<TableInfo>();

            foreach (var item in foreignRelationList)
            {
                tableInfo.HasForeignKey = tableInfo.HasForeignKey || true;

                TableInfo foreignTableInfo = new TableInfo();
                foreignTableInfo.TableId = item.RelationTableId;
                foreignTableInfo.TableName = GetTableNameByTableId(foreignTableInfo.TableId);
                foreignTableInfo.ForeignKeyId = item.ForeignKeyId;
                foreignTableInfo.RelationKeyId = item.RelationKeyId;
                foreignTableInfo.TableFildsInfoList = GetTableFeildsInfoListByTableId(item.RelationTableId);
                foreignTableInfo.ForeignTableInfoList = GetForeignRelationTableFeildsInfo(foreignTableInfo);
                foreignTableInfoList.Add(foreignTableInfo);
            }
            return foreignTableInfoList;
        }

        private string GetTableNameByTableId(int tableId)
        {
            string queryFeildSql = $"select a.id as 'TableId',a.name as 'TableName' from sys.sysobjects a where xType <> 'S' and xType <> 'IT'";

            return TableConstructionCache($"{this.DataBaseName}{this.TableName}{queryFeildSql}", () =>
            {
                return Return_List_T_ByDataSet<TableIdName>(ExecuteDataSet(queryFeildSql));
            }).Where(t => t.TableId == tableId).FirstOrDefault().TableName;
        }
        private List<TableFeildsInfo> GetTableFeildsInfoListByTableId(int tableId)
        {
            string queryFeildSql = $"use [{this.DataBaseName}] SELECT a.[id] as 'TableId', a.colid as 'ColumnId', a.[name] as 'FeildName',a.length as 'Length',c.[name] as 'Type',a.isnullable as 'Nullable' FROM syscolumns a left join systypes c on a.xtype = c.xusertype inner join sysobjects d on a.id = d.id and d.xtype = 'U' where d.id = '{tableId}'";

            return TableConstructionCache($"{this.DataBaseName}{this.TableName}{queryFeildSql}", () =>
            {
                return Return_List_T_ByDataSet<TableFeildsInfo>(ExecuteDataSet(queryFeildSql));
            });
        }

        #endregion

        #region Transaction Support
        /// <summary>
        /// Transacation
        /// </summary>
        /// <param name="action"></param>
        public void Transaction(Action action)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                action();
                trans.Complete();
            }
        }
        #endregion

        #region Cache Support

        /// <summary>
        /// TableConstructionCache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static T TableConstructionCache<T>(string cacheKey, Func<T> func) where T : class
        {
            string hashKey = cacheKey.GetHashCode().ToString();
            object cacheValue = HttpRuntimeCache_Helper_DG.Cache_Get(hashKey);
            if (cacheValue != null)
                return cacheValue as T;
            cacheValue = func();
            HttpRuntimeCache_Helper_DG.Cache_Add(hashKey, cacheValue);
            return cacheValue as T;
        }

        /// <summary>
        /// Cache Channel
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="cacheKey">cacheKey</param>
        /// <param name="func">Func<object></param>
        /// <returns></returns>
        private static object CacheChannel(string tableName, string cacheKey, Func<object> func)
        {
            /**
             * author:qixiao
             * create:2017-8-7 22:47:13
             * Howw to judge a cache lose efficacy when we update the table data ？
             * we add another cache name changeCache， let key = tableName，expire time default ，like 1.
             * once we gain data from cache ， validate changeCache has any value not null.if it is ,regard value changed,we should gain table data renew not from cache.
             * if it is null , regard as unchanged, if cacheCache expire ,dataCache expired too,we can gain cache relieved
             * */
            if (HttpRuntimeCache_Helper_DG.Cache_Get(tableName) == null)
            {
                object cacheValue = HttpRuntimeCache_Helper_DG.Cache_Get(cacheKey);
                if (cacheValue != null)
                    return cacheValue;
            }

            //Execute Action
            object result = func();

            HttpRuntimeCache_Helper_DG.Cache_Add(cacheKey, result);
            HttpRuntimeCache_Helper_DG.Cache_Delete(tableName);

            return result;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => GC.Collect();

        #endregion
    }

    /// <summary>
    /// TableInfo class
    /// </summary>
    internal class TableInfo
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public List<TableFeildsInfo> TableFildsInfoList { get; set; }
        public bool HasForeignKey { get; set; } = false;
        //parent foreignKeyId
        public int ForeignKeyId { get; set; }
        //this table relation parent relationKeyId
        public int RelationKeyId { get; set; }
        //foreignTableList
        public List<TableInfo> ForeignTableInfoList { get; set; }
    }

    /// <summary>
    /// Internal Class TableCOnstruction
    /// </summary>
    internal class TableFeildsInfo
    {
        public int TableId { get; set; }
        public int ColumnId { get; set; }
        public string FeildName { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string Nullable { get; set; }
    }

    internal class ForeignRelatioin
    {
        public int ForeignRelationId { get; set; }
        public int ParentTableId { get; set; }
        public int RelationTableId { get; set; }
        public int ForeignKeyId { get; set; }
        public int RelationKeyId { get; set; }
    }
    internal class TableIdName
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
    }
}
