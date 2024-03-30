namespace Intercon.Domain.Pagination;

public abstract class QueryStringParameters
{
    public const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }
}

public class ReviewParameters : QueryStringParameters
{

}

public class BusinessParameters : QueryStringParameters
{

}
