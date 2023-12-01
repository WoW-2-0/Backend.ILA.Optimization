namespace Caching.SimpleInfra.Domain.Common.Query;

public class FilterPagination
{
    public FilterPagination()
    {
    }

    public FilterPagination(uint pageSize, uint pageToken)
    {
        PageSize = pageSize;
        PageToken = pageToken;
    }

    public uint PageSize { get; }

    public uint PageToken { get; }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        hashCode.Add(PageSize);
        hashCode.Add(PageToken);

        return hashCode.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is FilterPagination filterPagination && filterPagination.GetHashCode() == GetHashCode();
    }
}