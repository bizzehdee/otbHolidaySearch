namespace otbHolidaySearch.Data;

public interface IDataLoader<out T>
    where T : class
{
    IEnumerable<T> LoadData(string filePath);
}