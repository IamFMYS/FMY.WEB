﻿/*
 * 作者： JT
 * 
 * 时间：
 * 
 * 介绍：Ibatis MapperInstance , Get Dynamic Sql
 */
using System;
using System.Text;
using System.IO;
using System.Reflection;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;

namespace FMY.WEB.IbatisDao
{
    public class Mapper
    {

        public static string MapperDomain = "FMY.WEB.IbatisDao";

        private static volatile ISqlMapper _mapper;

        public static ISqlMapper Instance
        {
            get {
                if (_mapper == null)
                {
                    lock (typeof(ISqlMapper))
                    {
                        if (_mapper == null)
                            _mapper = InitMapper();
                    }
                }
                return _mapper;
            }
        }


        /// <summary>
        /// 从资源文件加载SqlMap.config 创建ISqlMapper
        /// </summary>
        /// <returns></returns>
        private static ISqlMapper InitMapper()
        {
            //SqlMapSession sesstion=
            DomSqlMapBuilder builder = new DomSqlMapBuilder();
            Assembly assembly = Assembly.GetAssembly(typeof(Mapper));
            string resouce = string.Format("{0}.{1}", MapperDomain, "SqlMap.config");
            using (Stream stream = assembly.GetManifestResourceStream(resouce))
            {
                try
                {
                    return builder.Configure(stream);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// 得到运行时的IbatisNet动态生成的SQL语句
        /// </summary>
        /// <param name="sqlMapper">获取关联SqlMapper对象</param>
        /// <param name="statementName">xml中节点名称</param>
        /// <param name="paramObject">xml中节点的参数</param>
        /// <returns>生成的Sql语句或者错误</returns>
        public static string GetSql(string statementName, object paramObject)
        {
            try
            {
                IBatisNet.DataMapper.MappedStatements.IMappedStatement statement = Instance.GetMappedStatement(statementName);
                if (!Instance.IsSessionStarted)
                {
                    Instance.OpenConnection();
                }
                IBatisNet.DataMapper.Scope.RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, Instance.LocalSession);
                statement.PreparedCommand.Create(scope, Instance.LocalSession, statement.Statement, paramObject);
                StringBuilder sbSql = new StringBuilder();

                Func<string, string> fn = (type) =>
                {
                    switch (type)
                    {
                        case "Int32":
                            return "Int";
                        case "String":
                        default:
                            return "Varchar(100)";
                    }
                };

                foreach (System.Data.IDataParameter pa in scope.IDbCommand.Parameters)
                {
                    if (pa.DbType == System.Data.DbType.Int32)
                    {
                        sbSql.AppendFormat("DECLARE {0} {1}  SET {0}={2} ", pa.ParameterName, fn(pa.DbType.ToString()), pa.Value);
                    }
                    else
                    {
                        sbSql.AppendFormat("DECLARE {0} {1}  SET {0}='{2}' ", pa.ParameterName, fn(pa.DbType.ToString()), pa.Value);
                    }
                }
                sbSql.Append(scope.PreparedStatement.PreparedSql);
                return sbSql.ToString();
            }
            catch (Exception ex)
            {
                return "获取SQL语句出现异常：" + ex.Message;
            }
        }
    }
}
