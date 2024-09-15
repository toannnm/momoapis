namespace Application.Extensions
{
    public class Pagination<TEntity>
    {
        public int TotalItemsCount { get; set; }
        public int TotalPageCount
        {
            get
            {
                var temp = TotalItemsCount / PageSize;
                if (TotalItemsCount % PageSize == 0)
                {
                    return temp;
                }
                return temp + 1;
            }
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public ICollection<TEntity> Items { get; set; } = null!;
    }
}
