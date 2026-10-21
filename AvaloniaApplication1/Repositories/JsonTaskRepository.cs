using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using AvaloniaApplication1.Models;

namespace AvaloniaApplication1.Repositories;

// сохранение и загрузка задач в json файл
public class JsonTaskRepository : ITaskRepository
{
    private string _filePath;

    public JsonTaskRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void Save(List<TodoModel> tasks)
    {
        // создаем папку, если ее еще нет
        string folder = Path.GetDirectoryName(_filePath);
        if (folder != "" && !Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        var options = new JsonSerializerOptions();
        options.WriteIndented = true;
        // чтобы статус и приоритет писались словами, а не числами
        options.Converters.Add(new JsonStringEnumConverter());

        string json = JsonSerializer.Serialize(tasks, options);
        File.WriteAllText(_filePath, json);
    }

    public List<TodoModel> Load()
    {
        if (!File.Exists(_filePath))
        {
            return new List<TodoModel>();
        }

        string json = File.ReadAllText(_filePath);

        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter());

        return JsonSerializer.Deserialize<List<TodoModel>>(json, options);
    }
}
