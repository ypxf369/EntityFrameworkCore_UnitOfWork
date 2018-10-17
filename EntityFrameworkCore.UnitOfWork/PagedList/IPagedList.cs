using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 分页接口
    /// </summary>
    /// <typeparam name="T">分页类型</typeparam>
    public interface IPagedList<T>
    {
        /// <summary>
        /// 页码索引起始值
        /// </summary>
        int IndexFrom { get; }

        /// <summary>
        /// 当前页码
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 每页条数
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 总数量 <typeparamref name="T"/>
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// 当前页数据项
        /// </summary>
        IList<T> Items { get; }

        /// <summary>
        /// 前一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
