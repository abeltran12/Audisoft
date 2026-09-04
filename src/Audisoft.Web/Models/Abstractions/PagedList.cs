namespace Audisoft.Web.Models.Abstractions;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }

    public PagedList(List<T> items, MetaData metaData)
    {
        MetaData = metaData;
        AddRange(items);
    }
}
