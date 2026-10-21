using System;

namespace AvaloniaApplication1.Models;

public enum TaskStatus
{
    NotStarted,
    InProgress,
    Done
}

public enum TaskPriority
{
    Low,
    Medium,
    High
}

public class TodoModel
{
    private string _name = "";

    public string Name
    {
        get 
        { 
            return _name; 
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Название не может быть пустым");
            }
            _name = value;
        }
    }

    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public TaskStatus Status { get; set; }

    public string GetStatusText()
    {
        if (Status == TaskStatus.NotStarted) return "Не начата";
        if (Status == TaskStatus.InProgress) return "В процессе";
        return "Выполнена";
    }

    public string GetPriorityText()
    {
        if (Priority == TaskPriority.Low) return "Низкий";
        if (Priority == TaskPriority.Medium) return "Средний";
        return "Высокий";
    }

    // так задача будет выглядеть в списке
    public override string ToString()
    {
        return Name + " [" + GetStatusText() + ", " + GetPriorityText() + "] срок: " + DueDate.ToShortDateString();
    }
}
