namespace otbHolidaySearch.Data;

public class JsonDataLoader<T> : IDataLoader<T>
    where T : class
{
    public IEnumerable<T> LoadData(string filePath)
    {
        List<T> data;

        using (var reader = new StreamReader(filePath))
        {
            var json = reader.ReadToEnd();
            data = System.Text.Json.JsonSerializer.Deserialize<List<T>>(json);
        }

        return data;
    }
}