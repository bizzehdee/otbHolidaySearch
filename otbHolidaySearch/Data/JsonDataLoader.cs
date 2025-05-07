namespace otbHolidaySearch.Data;

public class JsonDataLoader<T>
    where T : class
{
    public IEnumerable<T> LoadData(string filePath)
    {
        var data = new List<T>();

        using (var reader = new StreamReader(filePath))
        {
            var json = reader.ReadToEnd();
            data = JsonConvert.DeserializeObject<List<T>>(json);
        }

        return data;
    }
}