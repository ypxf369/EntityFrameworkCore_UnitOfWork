using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 为IEnumerable集合提供分页功能
    /// </summary>
    public static class IEnumerablePagedListExtensions
    {
        /// <summary>
        /// 分页拓展方法
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="indexFrom">页码索引起始值</param>
        /// <returns></returns>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int indexFrom = 1) => new PagedList<T>(source, pageIndex, pageSize, indexFrom);

        /// <summary>
        /// 将数据源转换为目标类型的分页拓展方法
        /// </summary>
        /// <typeparam name="TSource">原类型</typeparam>
        /// <typeparam name="TResult">目标类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="converter">转换器</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="indexFrom">页码索引起始值</param>
        /// <returns></returns>
        public static IPagedList<TResult> ToPagedList<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int pageIndex, int pageSize, int indexFrom = 0) => new PagedList<TSource, TResult>(source, converter, pageIndex, pageSize, indexFrom);
    }
}
