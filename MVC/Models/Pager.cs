namespace MVC.Models;

public class Pager
{
    private const int DefaultPageSize = 5;
    public int TotalItems { get; }
    public int CurrentPage { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int StartPage { get; }
    public int EndPage { get; }

    public int DisplayStartItem { get; private set; }
    public int DisplayEndItem { get; private set; }

    public Pager()
    {
    }

    public Pager(int totalItems, int currentPage, int pageSize = DefaultPageSize)
    {
        TotalItems = totalItems;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling((decimal)TotalItems / PageSize);
        CurrentPage = currentPage;

        StartPage = CurrentPage - 3;
        EndPage = CurrentPage + 3;

        if (StartPage <= 0)
        {
            EndPage -= (StartPage - 1);
            StartPage = 1;
        }

        if (EndPage > TotalPages)
        {
            EndPage = TotalPages;
            if (EndPage > 10)
            {
                StartPage = EndPage - 6;
            }
        }

        DisplayStartItem = (currentPage - 1) * pageSize + 1;
        DisplayEndItem = Math.Min(currentPage * pageSize, TotalItems);

    }
}