using System.Collections.Generic;
using AvaloniaApplication1.Models;

namespace AvaloniaApplication1.Repositories;

// интерфейс для сохранения и загрузки задач
public interface ITaskRepository
{
    void Save(List<TodoModel> tasks);

    List<TodoModel> Load();
}
