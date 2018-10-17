using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class IQueryablePageListExtensions
    {
        /// <summary>
        /// 分页拓展方法
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="indexFrom">页码索引起始值</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 1, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((pageIndex - indexFrom) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }
    }
}
