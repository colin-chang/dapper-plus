﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Dapper
{
    public class DapperHelper<TConnection>
        where TConnection : IDbConnection, new()
    {
        private readonly string _connStr;

        private IDbConnection Cnn => new TConnection {ConnectionString = _connStr};

        public DapperHelper(string connectionString) => _connStr = connectionString;

        /// <summary>
        /// Execute a non-query command.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="param">The parameters to use for this command.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute(string sql, object param = null, CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.Execute(sql, param, commandType: commandType);
        }

        /// <summary>
        /// Execute a non-query command asynchronously.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="param">The parameters to use for this command.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> ExecuteAsync(string sql, object param = null, CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.ExecuteAsync(sql, param, commandType: commandType);
        }

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="param">The parameters to use for this command.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The first cell returned, as System.Object.</returns>
        public object QueryScalar(string sql, object param = null, CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.ExecuteScalar(sql, param, commandType: commandType);
        }

        /// <summary>
        /// Execute parameterized SQL asynchronously that selects a single value.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="param">The parameters to use for this command.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The first cell returned, as System.Object.</returns>
        public async Task<object> QueryScalarAsync(string sql, object param = null,
            CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.ExecuteScalarAsync(sql, param, commandType: commandType);
        }

        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <returns>A sequence of data of T; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is created per row,and a direct column-name===member-name mapping is assumed (case insensitive).</returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.Query<T>(sql, param, commandType: commandType);
        }

        /// <summary>
        /// Execute a query asynchronously.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <returns>A sequence of data of T; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is created per row,and a direct column-name===member-name mapping is assumed (case insensitive).</returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null,
            CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryAsync<T>(sql, param, commandType: commandType);
        }

        /// <summary>
        /// Perform a asynchronous multi-mapping query with 2 input types. This returns a single type, combined from the raw types via map.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <typeparam name="T1">The first type in the record set.</typeparam>
        /// <typeparam name="T2">The second type in the record set.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <returns>An enumerable of TReturn.</returns>
        public IEnumerable<TReturn> Query<T1, T2, TReturn>(string sql,
            Func<T1, T2, TReturn> map, object param = null, CommandType? commandType = null,
            bool buffered = true)
        {
            using var cnn = Cnn;
            return cnn.Query(sql, map, param, commandType: commandType,
                buffered: buffered);
        }

        /// <summary>
        /// Perform a asynchronous multi-mapping query with 2 input types. This returns a single type, combined from the raw types via map.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <typeparam name="T1">The first type in the record set.</typeparam>
        /// <typeparam name="T2">The second type in the record set.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <returns>An enumerable of TReturn.</returns>
        public async Task<IEnumerable<TReturn>> QueryAsync<T1, T2, TReturn>(string sql,
            Func<T1, T2, TReturn> map, object param = null, CommandType? commandType = null,
            bool buffered = true)
        {
            using var cnn = Cnn;
            return await cnn.QueryAsync(sql, map, param, commandType: commandType,
                buffered: buffered);
        }

        /// <summary>
        /// Perform a asynchronous multi-mapping query with 3 input types. This returns a single type, combined from the raw types via map.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <typeparam name="T1">The first type in the record set.</typeparam>
        /// <typeparam name="T2">The second type in the record set.</typeparam>
        /// <typeparam name="T3">The third type in the record set.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <returns>An enumerable of TReturn.</returns>
        public IEnumerable<TReturn> Query<T1, T2, T3, TReturn>(string sql,
            Func<T1, T2, T3, TReturn> map, object param = null, CommandType? commandType = null,
            bool buffered = true)
        {
            using var cnn = Cnn;
            return cnn.Query(sql, map, param, commandType: commandType,
                buffered: buffered);
        }

        /// <summary>
        /// Perform a asynchronous multi-mapping query with 3 input types. This returns a single type, combined from the raw types via map.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <typeparam name="T1">The first type in the record set.</typeparam>
        /// <typeparam name="T2">The second type in the record set.</typeparam>
        /// <typeparam name="T3">The third type in the record set.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <returns>An enumerable of TReturn.</returns>
        public async Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, TReturn>(string sql,
            Func<T1, T2, T3, TReturn> map, object param = null, CommandType? commandType = null,
            bool buffered = true)
        {
            using var cnn = Cnn;
            return await cnn.QueryAsync(sql, map, param, commandType: commandType,
                buffered: buffered);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>Multiple result sets</returns>
        public IEnumerable<IEnumerable<object>> QueryMultiple(IEnumerable<string> sqls, object param = null,
            CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>Multiple result sets</returns>
        public async Task<IEnumerable<IEnumerable<object>>> QueryMultipleAsync(IEnumerable<string> sqls,
            object param = null, CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public (IEnumerable<T1> Result1, IEnumerable<T2> Result2) QueryMultiple<T1, T2>(
            IEnumerable<string> sqls, object param = null, CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple<T1, T2>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public async Task<(IEnumerable<T1> Result1, IEnumerable<T2> Result2)>
            QueryMultipleAsync<T1, T2>(IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync<T1, T2>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public (IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3)
            QueryMultiple<T1, T2, T3>(IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple<T1, T2, T3>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public async
            Task<(IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3)>
            QueryMultipleAsync<T1, T2, T3>(IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync<T1, T2, T3>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public (IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
            IEnumerable<T4> Result4)
            QueryMultiple<T1, T2, T3, T4>(IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple<T1, T2, T3, T4>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public async
            Task<(IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
                IEnumerable<T4> Result4)>
            QueryMultipleAsync<T1, T2, T3, T4>(IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync<T1, T2, T3, T4>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public (IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
            IEnumerable<T4> Result4, IEnumerable<T5> Result5)
            QueryMultiple<T1, T2, T3, T4, T5>(IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple<T1, T2, T3, T4, T5>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam> 
        /// <returns>Multiple result sets</returns>
        public async
            Task<(IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
                IEnumerable<T4> Result4, IEnumerable<T5> Result5)>
            QueryMultipleAsync<T1, T2, T3, T4, T5>(IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync<T1, T2, T3, T4, T5>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam>
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public (IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
            IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6)
            QueryMultiple<T1, T2, T3, T4, T5, T6>(IEnumerable<string> sqls,
                object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple<T1, T2, T3, T4, T5, T6>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam> 
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public async
            Task<(IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
                IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6)>
            QueryMultipleAsync<T1, T2, T3, T4, T5, T6>(
                IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync<T1, T2, T3, T4, T5, T6>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam>
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <typeparam name="T7">The type of the seventh sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public (IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
            IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6, IEnumerable<T7> Result7)
            QueryMultiple<T1, T2, T3, T4, T5, T6, T7>(IEnumerable<string> sqls,
                object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple<T1, T2, T3, T4, T5, T6, T7>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam> 
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <typeparam name="T7">The type of the seventh sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public async
            Task<(IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
                IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6, IEnumerable<T7> Result7)>
            QueryMultipleAsync<T1, T2, T3, T4, T5, T6, T7>(
                IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync<T1, T2, T3, T4, T5, T6, T7>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam>
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <typeparam name="T7">The type of the seventh sql record set.</typeparam>
        /// <typeparam name="T8">The type of the eighth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public (IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
            IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6, IEnumerable<T7> Result7,
            IEnumerable<T8> Result8)
            QueryMultiple<T1, T2, T3, T4, T5, T6, T7, T8>(IEnumerable<string> sqls,
                object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple<T1, T2, T3, T4, T5, T6, T7, T8>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam> 
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <typeparam name="T7">The type of the seventh sql record set.</typeparam>
        /// <typeparam name="T8">The type of the eighth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public async
            Task<(IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
                IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6, IEnumerable<T7> Result7,
                IEnumerable<T8> Result8)>
            QueryMultipleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
                IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam>
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <typeparam name="T7">The type of the seventh sql record set.</typeparam>
        /// <typeparam name="T8">The type of the eighth sql record set.</typeparam>
        /// <typeparam name="T9">The type of the ninth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public (IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
            IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6, IEnumerable<T7> Result7,
            IEnumerable<T8> Result8, IEnumerable<T9> Result9)
            QueryMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IEnumerable<string> sqls,
                object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam> 
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <typeparam name="T7">The type of the seventh sql record set.</typeparam>
        /// <typeparam name="T8">The type of the eighth sql record set.</typeparam>
        /// <typeparam name="T9">The type of the ninth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public async
            Task<(IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
                IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6, IEnumerable<T7> Result7,
                IEnumerable<T8> Result8, IEnumerable<T9> Result9)>
            QueryMultipleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
                IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam>
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <typeparam name="T7">The type of the seventh sql record set.</typeparam>
        /// <typeparam name="T8">The type of the eighth sql record set.</typeparam>
        /// <typeparam name="T9">The type of the ninth sql record set.</typeparam>
        /// <typeparam name="T10">The type of the tenth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public (IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
            IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6, IEnumerable<T7> Result7,
            IEnumerable<T8> Result8, IEnumerable<T9> Result9, IEnumerable<T10> Result10)
            QueryMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IEnumerable<string> sqls,
                object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return cnn.QueryMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a command with multiple result sets and get its reader.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>Multiple result sets reader</returns>
        public SqlMapper.GridReader QueryMultipleReader(IEnumerable<string> sqls,
            object param = null,
            CommandType? commandType = null)
        {
            var cnn = Cnn;
            try
            {
                return cnn.QueryMultiple(sqls.Concat(), param, commandType: commandType);
            }
            catch
            {
                cnn.Close();
                cnn.Dispose();
                throw;
            }
        }


        /// <summary>
        /// Execute a command with multiple result sets and get its reader.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>Multiple result sets reader</returns>
        public async Task<SqlMapper.GridReader> QueryMultipleReaderAsync(IEnumerable<string> sqls,
            object param = null,
            CommandType? commandType = null)
        {
            var cnn = Cnn;
            try
            {
                return await cnn.QueryMultipleAsync(sqls.Concat(), param, commandType: commandType);
            }
            catch
            {
                cnn.Close();
                cnn.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sqls">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <typeparam name="T1">The type of the first sql record set.</typeparam>
        /// <typeparam name="T2">The type of the second sql record set.</typeparam>
        /// <typeparam name="T3">The type of the third sql record set.</typeparam>
        /// <typeparam name="T4">The type of the fourth sql record set.</typeparam>
        /// <typeparam name="T5">The type of the fifth sql record set.</typeparam> 
        /// <typeparam name="T6">The type of the sixth sql record set.</typeparam>
        /// <typeparam name="T7">The type of the seventh sql record set.</typeparam>
        /// <typeparam name="T8">The type of the eighth sql record set.</typeparam>
        /// <typeparam name="T9">The type of the ninth sql record set.</typeparam>
        /// <typeparam name="T10">The type of the tenth sql record set.</typeparam>
        /// <returns>Multiple result sets</returns>
        public async
            Task<(IEnumerable<T1> Result1, IEnumerable<T2> Result2, IEnumerable<T3> Result3,
                IEnumerable<T4> Result4, IEnumerable<T5> Result5, IEnumerable<T6> Result6, IEnumerable<T7> Result7,
                IEnumerable<T8> Result8, IEnumerable<T9> Result9, IEnumerable<T10> Result10)>
            QueryMultipleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                IEnumerable<string> sqls, object param = null,
                CommandType? commandType = null)
        {
            using var cnn = Cnn;
            return await cnn.QueryMultipleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(sqls, param, commandType);
        }

        /// <summary>
        /// Execute a non-query transaction.
        /// </summary>
        /// <param name="scripts">The 'SqlScript' set of this transaction to execute.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteTransaction(IEnumerable<SqlScript> scripts)
        {
            var cnn = Cnn;
            var count = 0;
            IDbTransaction tran = null;
            try
            {
                cnn.Open();
                tran = cnn.BeginTransaction();
                count += scripts.Sum(script =>
                    cnn.Execute(script.Sql, script.Param, tran, commandType: script.CommandType));

                tran.Commit();
                return count;
            }
            catch
            {
                tran?.Rollback();
                return 0;
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
        }

        /// <summary>
        /// Execute a non-query transaction asynchronously.
        /// </summary>
        /// <param name="scripts">The 'SqlScript' set of this transaction to execute.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> ExecuteTransactionAsync(IEnumerable<SqlScript> scripts)
        {
            var cnn = Cnn;
            var count = 0;
            IDbTransaction tran = null;
            try
            {
                cnn.Open();
                tran = cnn.BeginTransaction();
                foreach (var script in scripts)
                    count += await cnn.ExecuteAsync(script.Sql, script.Param, tran,
                        commandType: script.CommandType);

                tran.Commit();
                return count;
            }
            catch
            {
                tran?.Rollback();
                return 0;
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
        }

        /// <summary>
        /// Execute a transaction with the specify operation inside
        /// </summary>
        /// <param name="transaction">transaction operation</param>
        public void ExecuteTransaction(Action<IDbConnection> transaction)
        {
            var cnn = Cnn;
            IDbTransaction tran = null;
            try
            {
                cnn.Open();
                tran = cnn.BeginTransaction();
                if (transaction.Method.IsDefined(typeof(AsyncStateMachineAttribute), false))
                    throw new NotSupportedException(
                        "asynchronous Action<IDbConnection> is not awaitable nor supported, please use Func<IDbConnection,Task> instead");
                transaction(cnn);
                tran.Commit();
            }
            catch
            {
                tran?.Rollback();
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
        }

        /// <summary>
        /// Execute a transaction with the specify operation inside
        /// </summary>
        /// <param name="transaction">transaction operation</param>
        /// <typeparam name="TResult">return value type of the transaction operation</typeparam>
        /// <returns>return value of the transaction operation</returns>
        public TResult ExecuteTransaction<TResult>(Func<IDbConnection, TResult> transaction)
        {
            var cnn = Cnn;
            IDbTransaction tran = null;
            try
            {
                cnn.Open();
                tran = cnn.BeginTransaction();
                var result = transaction(cnn);

                if (transaction.Method.IsDefined(typeof(AsyncStateMachineAttribute), false))
                    (result as Task)?.Wait();

                tran.Commit();
                return result;
            }
            catch
            {
                tran?.Rollback();
                return default;
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
        }
    }
}