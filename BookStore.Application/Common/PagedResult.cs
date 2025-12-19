using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Common
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; }
        public PaginationMetadata Pagination { get; set; }
        public PagedResult(List<T> data,int totalCount, int Page,int page)
        {
            Data = data;
            Pagination = new PaginationMetadata(totalCount, Page, page);
        }

        public static PagedResult<T> Empty(int page,int pageSize)
        {
            return new PagedResult<T>(new List<T>(), 0, page, pageSize);
        }
    }
}
