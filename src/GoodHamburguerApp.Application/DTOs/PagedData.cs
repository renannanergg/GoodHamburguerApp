namespace GoodHamburguerApp.Application.DTOs
{
    public class PagedData<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }

        public PagedData(IReadOnlyList<T> items, int totalCount, int offset, int limit)
        {
            Items = items;
            TotalCount = totalCount;
            Offset = offset;
            Limit = limit;
        }
    }
}
