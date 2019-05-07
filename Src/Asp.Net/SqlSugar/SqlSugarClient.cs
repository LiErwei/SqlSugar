﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlSugar
{
    public partial class SqlSugarClient : ISqlSugarClient, ITenant
    {
        #region Gobal Property
        private ISqlSugarClient _Context = null;
        private string _ThreadId;
        private ConnectionConfig _CurrentConnectionConfig;
        private List<SugarTenant> _AllClients;
        private bool _IsAllTran = false;
        private bool _IsOpen = false;
        private MappingTableList _MappingTables;
        private MappingColumnList _MappingColumns;
        private IgnoreColumnList _IgnoreColumns;
        private IgnoreColumnList _IgnoreInsertColumns;

        #endregion

        #region Constructor
        public SqlSugarClient(ConnectionConfig config)
        {
            Check.Exception(config == null, "ConnectionConfig config is null");
            InitContext(config);
        }

        public SqlSugarClient(List<ConnectionConfig> configs)
        {
            Check.Exception(configs.IsNullOrEmpty(), "List<ConnectionConfig> configs is null");
            InitConfigs(configs);
            var config = configs.First();
            InitContext(config);
            _AllClients = configs.Select(it => new SugarTenant() { ConnectionConfig = it }).ToList(); ;
            _AllClients.First(it => it.ConnectionConfig.ConfigId == config.ConfigId).Context = this.Context;
        }

        #endregion

        #region Global variable
        public ISqlSugarClient Context { get => GetContext(); set => _Context = value; }
        public bool IsSystemTablesConfig => this.Context.IsSystemTablesConfig;
        public ConnectionConfig CurrentConnectionConfig { get => _CurrentConnectionConfig; set => _CurrentConnectionConfig = value; }
        public Guid ContextID { get => this.Context.ContextID; set => this.Context.ContextID = value; }


        public MappingTableList MappingTables { get => _MappingTables; set => _MappingTables = value; }
        public MappingColumnList MappingColumns { get => _MappingColumns; set => _MappingColumns = value; }
        public IgnoreColumnList IgnoreColumns { get => _IgnoreColumns; set => _IgnoreColumns = value; }
        public IgnoreColumnList IgnoreInsertColumns { get => _IgnoreInsertColumns; set => _IgnoreInsertColumns = value; }
        public Dictionary<string, object> TempItems { get => this.Context.TempItems; set => this.Context.TempItems = value; }
        #endregion

        #region SimpleClient
        public SimpleClient GetSimpleClient()
        {
            return this.Context.GetSimpleClient();
        }

        public SimpleClient<T> GetSimpleClient<T>() where T : class, new()
        {
            return this.Context.GetSimpleClient<T>();
        }
        #endregion

        #region Insertable
        public IInsertable<T> Insertable<T>(Dictionary<string, object> columnDictionary) where T : class, new()
        {
            return this.Context.Insertable<T>(columnDictionary);
        }

        public IInsertable<T> Insertable<T>(dynamic insertDynamicObject) where T : class, new()
        {
            return this.Context.Insertable<T>(insertDynamicObject);
        }

        public IInsertable<T> Insertable<T>(List<T> insertObjs) where T : class, new()
        {
            return this.Context.Insertable<T>(insertObjs);
        }

        public IInsertable<T> Insertable<T>(T insertObj) where T : class, new()
        {
            return this.Context.Insertable<T>(insertObj);
        }

        public IInsertable<T> Insertable<T>(T[] insertObjs) where T : class, new()
        {
            return this.Context.Insertable<T>(insertObjs);
        }

        #endregion

        #region Queryable

        #region Union
        public ISugarQueryable<T> Union<T>(List<ISugarQueryable<T>> queryables) where T : class, new()
        {
            return this.Context.Union(queryables);
        }

        public ISugarQueryable<T> Union<T>(params ISugarQueryable<T>[] queryables) where T : class, new()
        {
            return this.Context.Union(queryables);
        }

        public ISugarQueryable<T> UnionAll<T>(List<ISugarQueryable<T>> queryables) where T : class, new()
        {
            return this.Context.UnionAll(queryables);
        }

        public ISugarQueryable<T> UnionAll<T>(params ISugarQueryable<T>[] queryables) where T : class, new()
        {
            return this.Context.UnionAll(queryables);
        }
        #endregion

        public ISugarQueryable<T> SqlQueryable<T>(string sql) where T : class, new()
        {
            return this.Context.SqlQueryable<T>(sql);
        }
        public ISugarQueryable<ExpandoObject> Queryable(string tableName, string shortName)
        {
            return this.Context.Queryable(tableName, shortName);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8> Queryable<T, T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8> Queryable<T, T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7, T8> Queryable<T, T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7> Queryable<T, T2, T3, T4, T5, T6, T7>(Expression<Func<T, T2, T3, T4, T5, T6, T7, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7> Queryable<T, T2, T3, T4, T5, T6, T7>(Expression<Func<T, T2, T3, T4, T5, T6, T7, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6, T7> Queryable<T, T2, T3, T4, T5, T6, T7>(Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6> Queryable<T, T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6> Queryable<T, T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5, T6> Queryable<T, T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5> Queryable<T, T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5> Queryable<T, T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4, T5> Queryable<T, T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4> Queryable<T, T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4> Queryable<T, T2, T3, T4>(Expression<Func<T, T2, T3, T4, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3, T4> Queryable<T, T2, T3, T4>(Expression<Func<T, T2, T3, T4, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3> Queryable<T, T2, T3>(Expression<Func<T, T2, T3, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3> Queryable<T, T2, T3>(Expression<Func<T, T2, T3, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2, T3> Queryable<T, T2, T3>(Expression<Func<T, T2, T3, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2> Queryable<T, T2>(Expression<Func<T, T2, bool>> joinExpression) where T : class, new()
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2> Queryable<T, T2>(Expression<Func<T, T2, JoinQueryInfos>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2> Queryable<T, T2>(Expression<Func<T, T2, object[]>> joinExpression)
        {
            return this.Context.Queryable(joinExpression);
        }

        public ISugarQueryable<T, T2> Queryable<T, T2>(ISugarQueryable<T> joinQueryable1, ISugarQueryable<T2> joinQueryable2, Expression<Func<T, T2, bool>> joinExpression)
            where T : class, new()
            where T2 : class, new()
        {
            return this.Context.Queryable(joinQueryable1, joinQueryable2, joinExpression);
        }

        public ISugarQueryable<T, T2> Queryable<T, T2>(ISugarQueryable<T> joinQueryable1, ISugarQueryable<T2> joinQueryable2, JoinType joinType, Expression<Func<T, T2, bool>> joinExpression)
            where T : class, new()
            where T2 : class, new()
        {
            return this.Context.Queryable(joinQueryable1, joinQueryable2, joinType, joinExpression);
        }

        public ISugarQueryable<T> Queryable<T>()
        {
            return this.Context.Queryable<T>();
        }

        public ISugarQueryable<T> Queryable<T>(ISugarQueryable<T> queryable) where T : class, new()
        {
            return this.Context.Queryable<T>(queryable);
        }

        public ISugarQueryable<T> Queryable<T>(string shortName)
        {
            return this.Context.Queryable<T>(shortName);
        }

        #endregion

        #region Saveable
        public ISaveable<T> Saveable<T>(List<T> saveObjects) where T : class, new()
        {
            return this.Context.Saveable<T>(saveObjects);
        }

        public ISaveable<T> Saveable<T>(T saveObject) where T : class, new()
        {
            return this.Context.Saveable(saveObject);
        }
        #endregion

        #region Queue
        public QueueList Queues { get => this.Context.Queues; set => this.Context.Queues = value; }
        public void AddQueue(string sql, object parsmeters = null)
        {
            this.Context.AddQueue(sql, parsmeters);
        }

        public void AddQueue(string sql, List<SugarParameter> parsmeters)
        {
            this.Context.AddQueue(sql, parsmeters);
        }

        public void AddQueue(string sql, SugarParameter parsmeter)
        {
            this.Context.AddQueue(sql, parsmeter);
        }
        public int SaveQueues(bool isTran = true)
        {
            return this.Context.SaveQueues(isTran);
        }

        public Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> SaveQueues<T, T2, T3, T4, T5, T6, T7>(bool isTran = true)
        {
            return this.Context.SaveQueues<T, T2, T3, T4, T5, T6, T7>(isTran);
        }

        public Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> SaveQueues<T, T2, T3, T4, T5, T6>(bool isTran = true)
        {
            return this.Context.SaveQueues<T, T2, T3, T4, T5, T6>(isTran);
        }

        public Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>> SaveQueues<T, T2, T3, T4, T5>(bool isTran = true)
        {
            return this.Context.SaveQueues<T, T2, T3, T4, T5>(isTran);
        }

        public Tuple<List<T>, List<T2>, List<T3>, List<T4>> SaveQueues<T, T2, T3, T4>(bool isTran = true)
        {
            return this.Context.SaveQueues<T, T2, T3, T4>(isTran);
        }

        public Tuple<List<T>, List<T2>, List<T3>> SaveQueues<T, T2, T3>(bool isTran = true)
        {
            return this.Context.SaveQueues<T, T2, T3>(isTran);
        }

        public Tuple<List<T>, List<T2>> SaveQueues<T, T2>(bool isTran = true)
        {
            return this.Context.SaveQueues<T, T2>(isTran);
        }

        public List<T> SaveQueues<T>(bool isTran = true)
        {
            return this.Context.SaveQueues<T>(isTran);
        }

        public Task<int> SaveQueuesAsync(bool isTran = true)
        {
            return this.Context.SaveQueuesAsync(isTran);
        }

        public Task<Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>>> SaveQueuesAsync<T, T2, T3, T4, T5, T6, T7>(bool isTran = true)
        {
            return this.Context.SaveQueuesAsync<T, T2, T3, T4, T5, T6, T7>(isTran);
        }

        public Task<Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>>> SaveQueuesAsync<T, T2, T3, T4, T5, T6>(bool isTran = true)
        {
            return this.Context.SaveQueuesAsync<T, T2, T3, T4, T5, T6>(isTran);
        }

        public Task<Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>>> SaveQueuesAsync<T, T2, T3, T4, T5>(bool isTran = true)
        {
            return this.Context.SaveQueuesAsync<T, T2, T3, T4, T5>(isTran);
        }

        public Task<Tuple<List<T>, List<T2>, List<T3>, List<T4>>> SaveQueuesAsync<T, T2, T3, T4>(bool isTran = true)
        {
            return this.Context.SaveQueuesAsync<T, T2, T3, T4>(isTran);
        }

        public Task<Tuple<List<T>, List<T2>, List<T3>>> SaveQueuesAsync<T, T2, T3>(bool isTran = true)
        {
            return this.Context.SaveQueuesAsync<T, T2, T3>(isTran);
        }

        public Task<Tuple<List<T>, List<T2>>> SaveQueuesAsync<T, T2>(bool isTran = true)
        {
            return this.Context.SaveQueuesAsync<T, T2>(isTran);
        }

        public Task<List<T>> SaveQueuesAsync<T>(bool isTran = true)
        {
            return this.Context.SaveQueuesAsync<T>(isTran);
        }
        #endregion

        #region Updateable
        public IUpdateable<T> Updateable<T>() where T : class, new()
        {
            return this.Context.Updateable<T>();
        }

        public IUpdateable<T> Updateable<T>(Dictionary<string, object> columnDictionary) where T : class, new()
        {
            return this.Context.Updateable<T>(columnDictionary);
        }

        public IUpdateable<T> Updateable<T>(dynamic updateDynamicObject) where T : class, new()
        {
            return this.Context.Updateable<T>(updateDynamicObject);
        }

        public IUpdateable<T> Updateable<T>(Expression<Func<T, bool>> columns) where T : class, new()
        {
            return this.Context.Updateable<T>(columns);
        }

        public IUpdateable<T> Updateable<T>(Expression<Func<T, T>> columns) where T : class, new()
        {
            return this.Context.Updateable<T>(columns);
        }

        public IUpdateable<T> Updateable<T>(List<T> UpdateObjs) where T : class, new()
        {
            return this.Context.Updateable<T>(UpdateObjs);
        }

        public IUpdateable<T> Updateable<T>(T UpdateObj) where T : class, new()
        {
            return this.Context.Updateable<T>(UpdateObj);
        }

        public IUpdateable<T> Updateable<T>(T[] UpdateObjs) where T : class, new()
        {
            return this.Context.Updateable<T>(UpdateObjs);
        }

        #endregion

        #region Ado
        public IAdo Ado => this.Context.Ado;

        #endregion

        #region Deleteable
        public IDeleteable<T> Deleteable<T>() where T : class, new()
        {
            return this.Context.Deleteable<T>();
        }

        public IDeleteable<T> Deleteable<T>(dynamic primaryKeyValue) where T : class, new()
        {
            return this.Context.Deleteable<T>(primaryKeyValue);
        }

        public IDeleteable<T> Deleteable<T>(dynamic[] primaryKeyValues) where T : class, new()
        {
            return this.Context.Deleteable<T>(primaryKeyValues);
        }

        public IDeleteable<T> Deleteable<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return this.Context.Deleteable(expression);
        }

        public IDeleteable<T> Deleteable<T>(List<dynamic> pkValue) where T : class, new()
        {
            return this.Context.Deleteable<T>(pkValue);
        }

        public IDeleteable<T> Deleteable<T>(List<T> deleteObjs) where T : class, new()
        {
            return this.Context.Deleteable<T>(deleteObjs);
        }

        public IDeleteable<T> Deleteable<T>(T deleteObj) where T : class, new()
        {
            return this.Context.Deleteable<T>(deleteObj);
        }


        #endregion

        #region More api
        public IContextMethods Utilities { get => this.Context.Utilities; set => this.Context.Utilities = value; }
        public AopProvider Aop => this.Context.Aop;
        public ICodeFirst CodeFirst => this.Context.CodeFirst;
        public IDbFirst DbFirst => this.Context.DbFirst;
        public IDbMaintenance DbMaintenance => this.Context.DbMaintenance;
        public EntityMaintenance EntityMaintenance { get => this.Context.EntityMaintenance; set => this.Context.EntityMaintenance = value; }
        public QueryFilterProvider QueryFilter { get => this.Context.QueryFilter; set => this.Context.QueryFilter = value; }
        #endregion

        #region TenantManager
        public void ChangeDatabase(string configId)
        {
            Check.Exception(!_AllClients.Any(it => it.ConnectionConfig.ConfigId == configId), "ConfigId was not found {0}", configId);
            InitTenant(_AllClients.First(it => it.ConnectionConfig.ConfigId == configId));
            if (this._IsAllTran)
                this.Ado.BeginTran();
            if (this._IsOpen) 
                this.Open();
        }
        public void ChangeDatabase(Func<ConnectionConfig, bool> changeExpression)
        {
            var allConfigs = _AllClients.Select(it => it.ConnectionConfig);
            Check.Exception(!allConfigs.Any(changeExpression), "changeExpression was not found {0}", changeExpression.ToString());
            InitTenant(_AllClients.First(it => it.ConnectionConfig == allConfigs.First(changeExpression)));
            if (this._IsAllTran)
                this.Ado.BeginTran();
            if (this._IsOpen)
                this.Open();
        }
        public void BeginTran()
        {
            _IsAllTran = true;
            this.Context.Ado.BeginTran();
        }
        public void CommitTran()
        {
            this.Context.Ado.CommitTran();
            AllClientEach(it => it.Ado.CommitTran());
            _IsAllTran = false;
        }
        public DbResult<bool> UseTran(Action action, Action<Exception> errorCallBack = null)
        {
            var result = new DbResult<bool>();
            try
            {
                this.BeginTran();
                if (action != null)
                    action();
                this.CommitTran();
                result.Data = result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.ErrorException = ex;
                result.ErrorMessage = ex.Message;
                result.IsSuccess = false;
                this.RollbackTran();
                if (errorCallBack != null)
                {
                    errorCallBack(ex);
                }
            }
            return result;
        }

        public Task<DbResult<bool>> UseTranAsync(Action action, Action<Exception> errorCallBack = null)
        {
            Task<DbResult<bool>> result = new Task<DbResult<bool>>(() =>
            {
                return UseTran(action, errorCallBack);
            });
            TaskStart(result);
            return result;
        }

        public DbResult<T> UseTran<T>(Func<T> action, Action<Exception> errorCallBack = null)
        {
            var result = new DbResult<T>();
            try
            {
                this.BeginTran();
                if (action != null)
                    result.Data = action();
                this.CommitTran();
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.ErrorException = ex;
                result.ErrorMessage = ex.Message;
                result.IsSuccess = false;
                this.RollbackTran();
                if (errorCallBack != null)
                {
                    errorCallBack(ex);
                }
            }
            return result;
        }

        public Task<DbResult<T>> UseTranAsync<T>(Func<T> action, Action<Exception> errorCallBack = null)
        {
            Task<DbResult<T>> result = new Task<DbResult<T>>(() =>
            {
                return UseTran(action, errorCallBack);
            });
            TaskStart(result);
            return result;
        }

        public void RollbackTran()
        {
            this.Context.Ado.RollbackTran();
            AllClientEach(it=>it.Ado.RollbackTran());
            _IsAllTran = false;
        }
        public void Close()
        {
            this.Context.Close();
            AllClientEach(it => it.Close());
            _IsOpen = false;
        }
        public void Open()
        {
            this.Context.Open();
            _IsOpen = true;
        }

        #endregion

        #region IDispose
        public void Dispose()
        {
            this.Context.Dispose();
            AllClientEach(it => it.Ado.RollbackTran());
        }

        #endregion

        #region Other method
        public DateTime GetDate()
        {
            return this.Context.GetDate();
        }
        public void InitMppingInfo(Type type)
        {
            this.Context.InitMppingInfo(type);
        }
        public void InitMppingInfo<T>()
        {
            this.Context.InitMppingInfo(typeof(T));
        }
        #endregion

        #region Helper
        private ISqlSugarClient GetContext()
        {
            if (CurrentConnectionConfig.IsShardSameThread)
            {
                ISqlSugarClient result = _Context;
                if (CallContext.ContextList.Value.IsNullOrEmpty())
                {

                    CallContext.ContextList.Value = new List<ISqlSugarClient>();
                    CallContext.ContextList.Value.Add(_Context);
                }
                else
                {
                    ISqlSugarClient cacheContext = GetCallContext();
                    if (cacheContext != null)
                    {
                        result = cacheContext;
                    }
                    else
                    {
                        result = CopyClient();
                        CallContext.ContextList.Value.Add(result);
                    }
                }
                return result;
            }
            else if (_ThreadId == Thread.CurrentThread.ManagedThreadId.ToString())
            {
                _Context.MappingColumns = _MappingColumns;
                _Context.MappingTables = _MappingTables;
                _Context.IgnoreColumns = _IgnoreColumns;
                _Context.IgnoreInsertColumns = _IgnoreInsertColumns;
                return _Context;
            }
            else
            {
                if (CallContext.ContextList.Value == null)
                {
                    CallContext.ContextList.Value = new List<ISqlSugarClient>();
                }
                if (CallContext.ContextList.Value.IsNullOrEmpty() || GetCallContext() == null)
                {
                    var context = CopyClient();
                    CallContext.ContextList.Value.Add(context);
                    return context;
                }
                else
                {
                    return GetCallContext();
                }
            }
        }

        private SqlSugarClient CopyClient()
        {
            var result = new SqlSugarClient(this.CurrentConnectionConfig);
            result.MappingColumns = _MappingColumns;
            result.MappingTables = _MappingTables;
            result.IgnoreColumns = _IgnoreColumns;
            result.IgnoreInsertColumns = _IgnoreInsertColumns;
 
            return result;
        }

        private ISqlSugarClient GetCallContext()
        {
            return CallContext.ContextList.Value.FirstOrDefault(it => 
                it.CurrentConnectionConfig.DbType == _Context.CurrentConnectionConfig.DbType&&
                it.CurrentConnectionConfig.ConnectionString == _Context.CurrentConnectionConfig.ConnectionString&&
                it.CurrentConnectionConfig.InitKeyType==_Context.CurrentConnectionConfig.InitKeyType
            );
        }

        private void InitContext(ConnectionConfig config)
        {
            var aopIsNull = config.AopEvents == null;
            if (aopIsNull)
            {
                config.AopEvents = new AopEvents();
            }
            _Context = new SqlSugarEngine(config);
            if (!aopIsNull)
                _Context.Ado.IsEnableLogEvent = true;
            this.CurrentConnectionConfig = config;
            _ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            if (_MappingColumns == null)
                this.MappingTables = new MappingTableList();
            if (this.MappingColumns == null)
                this.MappingColumns = new MappingColumnList();
            if (this.IgnoreColumns == null)
                this.IgnoreColumns = new IgnoreColumnList();
            if (this.IgnoreInsertColumns == null)
                this.IgnoreInsertColumns = new IgnoreColumnList();
        }

        private void InitConfigs(List<ConnectionConfig> configs)
        {
            foreach (var item in configs)
            {
                if (item.ConfigId == null)
                {
                    item.ConfigId = Guid.NewGuid().ToString();
                }
            }
        }
        private void AllClientEach(Action<ISqlSugarClient> action)
        {
            if (_AllClients.HasValue())
            {
                foreach (var item in _AllClients.Where(it => it.Context.HasValue()))
                {
                    action(item.Context);
                }
            }
        }

        private void InitTenant(SugarTenant Tenant)
        {
            if (Tenant.Context == null)
            {
                Tenant.Context = new SqlSugarClient(Tenant.ConnectionConfig);
            }
            _Context = Tenant.Context;
            this.CurrentConnectionConfig = Tenant.ConnectionConfig;
        }
        private void TaskStart<Type>(Task<Type> result)
        {
            if (this.Context.CurrentConnectionConfig.IsShardSameThread)
            {
                Check.Exception(true, "IsShardSameThread=true can't be used async method");
            }
            result.Start();
        }
        #endregion

        #region Obsolete
        [Obsolete]
        public EntityMaintenance EntityProvider { get => this.Context.EntityProvider; set => this.Context.EntityProvider = value; }
        [Obsolete]
        public IContextMethods RewritableMethods { get => this.Context.RewritableMethods; set => this.Context.RewritableMethods = value; }
        [Obsolete]
        public SimpleClient SimpleClient => this.Context.SimpleClient;
        #endregion

    }
}
