using System;
using System.Collections.Generic;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.Repositories;

namespace AvaloniaApplication1.Services;

// класс для работы с задачами
public class TodoService
{
    private List<TodoModel> _tasks = new List<TodoModel>();
    private ITaskRepository _repository;

    public TodoService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public List<TodoModel> GetTasks()
    {
        return _tasks;
    }

    public void AddTask(string name, string description, DateTime dueDate, TaskPriority priority)
    {
        TodoModel task = new TodoModel();
        task.Name = name;
        task.Description = description;
        task.CreatedAt = DateTime.Now;
        task.DueDate = dueDate;
        task.Priority = priority;
        task.Status = TaskStatus.NotStarted;
        _tasks.Add(task);
    }

    public void EditTask(TodoModel task, string name, string description, DateTime dueDate, TaskPriority priority)
    {
        // проверка названия сработает в свойстве Name
        task.Name = name;
        task.Description = description;
        task.DueDate = dueDate;
        task.Priority = priority;
    }

    public void DeleteTask(TodoModel task)
    {
        _tasks.Remove(task);
    }

    public void ChangeStatus(TodoModel task, TaskStatus status)
    {
        task.Status = status;
    }

    public void SaveTasks()
    {
        _repository.Save(_tasks);
    }

    public int LoadTasks()
    {
        _tasks = _repository.Load();
        return _tasks.Count;
    }
}
