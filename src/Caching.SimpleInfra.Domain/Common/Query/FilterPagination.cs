namespace Caching.SimpleInfra.Domain.Common.Query;

public class FilterPagination
{
    public FilterPagination()
    {
        
    }
    
    public FilterPagination(int pageSize, int pageToken)
    {
        PageSize = pageSize;
        PageToken = pageToken;
    }

    public int PageSize { get; set; }

    public int PageToken { get; set; }
}