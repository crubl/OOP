using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.Repositories;
using AvaloniaApplication1.Services;

namespace AvaloniaApplication1.Views;

public partial class MainWindow : Window
{
    private TodoService _service = new TodoService(new JsonTaskRepository("Data/tasks.json"));
    private TodoModel _selected = null;

    public MainWindow()
    {
        InitializeComponent();

        // заполняем выпадающие списки
        PriorityBox.ItemsSource = new List<string> { "Низкий", "Средний", "Высокий" };
        StatusBox.ItemsSource = new List<string> { "Не начата", "В процессе", "Выполнена" };
        ClearForm();

        // при запуске пробуем загрузить старые задачи из файла
        if (File.Exists("Data/tasks.json"))
        {
            LoadFromFileButton_Click(null, null);
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _service.AddTask(NameBox.Text, DescriptionBox.Text, GetDueDate(), GetPriority());
            RefreshList();
            StatusText.Text = "Задача добавлена";
            ClearForm();
        }
        catch (Exception ex)
        {
            StatusText.Text = ex.Message;
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selected == null)
        {
            StatusText.Text = "Сначала выберите задачу в списке";
            return;
        }
        try
        {
            _service.EditTask(_selected, NameBox.Text, DescriptionBox.Text, GetDueDate(), GetPriority());
            _service.ChangeStatus(_selected, GetStatus());
            RefreshList();
            StatusText.Text = "Изменения сохранены";
        }
        catch (Exception ex)
        {
            StatusText.Text = ex.Message;
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selected == null)
        {
            StatusText.Text = "Сначала выберите задачу в списке";
            return;
        }
        _service.DeleteTask(_selected);
        _selected = null;
        ClearForm();
        RefreshList();
        StatusText.Text = "Задача удалена";
    }

    private void SaveToFileButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _service.SaveTasks();
            StatusText.Text = "Задачи сохранены в файл Data/tasks.json";
        }
        catch (Exception ex)
        {
            StatusText.Text = "Ошибка сохранения: " + ex.Message;
        }
    }

    private void LoadFromFileButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int count = _service.LoadTasks();
            RefreshList();
            StatusText.Text = "Загружено задач: " + count;
        }
        catch (Exception ex)
        {
            StatusText.Text = "Ошибка загрузки: " + ex.Message;
        }
    }

    // при клике на задачу подставляем её данные в форму
    private void TasksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TasksList.SelectedItem is TodoModel task)
        {
            _selected = task;
            NameBox.Text = task.Name;
            DescriptionBox.Text = task.Description;
            DueDatePicker.SelectedDate = task.DueDate;
            PriorityBox.SelectedIndex = (int)task.Priority;
            StatusBox.SelectedIndex = (int)task.Status;
        }
    }

    // вспомогательные методы

    private DateTime GetDueDate()
    {
        if (DueDatePicker.SelectedDate == null)
        {
            return DateTime.Now;
        }
        return DueDatePicker.SelectedDate.Value.DateTime;
    }

    // индекс в списке совпадает со значением enum
    private TaskPriority GetPriority()
    {
        if (PriorityBox.SelectedIndex == 0) return TaskPriority.Low;
        if (PriorityBox.SelectedIndex == 1) return TaskPriority.Medium;
        return TaskPriority.High;
    }

    private TaskStatus GetStatus()
    {
        if (StatusBox.SelectedIndex == 0) return TaskStatus.NotStarted;
        if (StatusBox.SelectedIndex == 1) return TaskStatus.InProgress;
        return TaskStatus.Done;
    }

    // перечитываем список, чтобы текст задач обновился
    private void RefreshList()
    {
        TasksList.ItemsSource = null;
        TasksList.ItemsSource = _service.GetTasks();
    }

    private void ClearForm()
    {
        NameBox.Text = "";
        DescriptionBox.Text = "";
        DueDatePicker.SelectedDate = DateTimeOffset.Now;
        PriorityBox.SelectedIndex = 1;
        StatusBox.SelectedIndex = 0;
    }
}
