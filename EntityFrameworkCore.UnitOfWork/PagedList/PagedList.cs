using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 分页对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 页码索引起始值
        /// </summary>
        public int IndexFrom { get; set; }

        /// <summary>
        /// 当前页数据项
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// 前一页
        /// </summary>
        public bool HasPreviousPage => PageIndex - IndexFrom > 0;

        /// <summary>
        /// 下一页
        /// </summary>
        public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="indexFrom">页码索引起始值</param>
        internal PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int indexFrom)
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            PageIndex = pageIndex;
            PageSize = pageSize;
            IndexFrom = indexFrom;
            if (source is IQueryable<T> querable)
            {
                TotalCount = querable.Count();
                Items = querable.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                TotalCount = source.Count();
                Items = source.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize).ToList();
            }
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal PagedList() => Items = new T[0];
    }


    /// <summary>
    /// PagedList converter.
    /// </summary>
    /// <typeparam name="TSource">原类型</typeparam>
    /// <typeparam name="TResult">目标类型</typeparam>
    internal class PagedList<TSource, TResult> : IPagedList<TResult>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// 页码索引起始值
        /// </summary>
        public int IndexFrom { get; }

        /// <summary>
        /// 当前页数据项
        /// </summary>
        public IList<TResult> Items { get; }

        /// <summary>
        /// 上一页
        /// </summary>
        public bool HasPreviousPage => PageIndex - IndexFrom > 0;

        /// <summary>
        /// 下一页
        /// </summary>
        public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="converter">转换器</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="indexFrom">页码索引起始值</param>
        public PagedList(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int pageIndex, int pageSize, int indexFrom)
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            PageIndex = pageIndex;
            PageSize = pageSize;
            IndexFrom = indexFrom;
            if (source is IQueryable<TSource> querable)
            {
                TotalCount = querable.Count();
                var items = querable.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize).ToList();

                Items = new List<TResult>(converter(items));
            }
            else
            {
                TotalCount = source.Count();
                var items = source.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize).ToList();

                Items = new List<TResult>(converter(items));
            }
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="converter">转换器</param>
        public PagedList(IPagedList<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
        {
            PageIndex = source.PageIndex;
            PageSize = source.PageSize;
            IndexFrom = source.IndexFrom;
            TotalCount = source.TotalCount;
            TotalPages = source.TotalPages;

            Items = new List<TResult>(converter(source.Items));
        }
    }

    /// <summary>
    /// IPagedList 帮助方法
    /// </summary>
    public static class PagedList
    {
        /// <summary>
        /// 获取一个空的IPagedList对象
        /// </summary>
        /// <typeparam name="T">type </typeparam>
        /// <returns>IPagedList 空对象</returns>
        public static IPagedList<T> Empty<T>() => new PagedList<T>();

        /// <summary>
        /// 获取一个由<see cref="IPagedList{TSource}"/>类型转换为 <see cref="IPagedList{TResult}"/> 类型的对象
        /// </summary>
        /// <typeparam name="TResult">原类型</typeparam>
        /// <typeparam name="TSource">目标类型</typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <returns><see cref="IPagedList{TResult}"/>对象</returns>
        public static IPagedList<TResult> From<TResult, TSource>(IPagedList<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter) => new PagedList<TSource, TResult>(source, converter);
    }
}
