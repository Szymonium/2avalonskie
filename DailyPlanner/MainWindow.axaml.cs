using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Avalonia.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;

namespace DailyPlanner
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<TaskItem> Tasks { get; } = new();
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Tasks.CollectionChanged += (_, _) => UpdateTaskGrid();
        }   
        public class TaskItem
        {
             public required string Name { get; set; }
             public required string Category { get; set; }
             public bool IsCompleted { get; set; }
        }
        private void AddTask(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskName.Text))
            {
                var newTask = new TaskItem { Name = TaskName.Text, Category = CategorySelection.SelectionBoxItem.ToString() };
                Tasks.Add(newTask);
                TaskName.Text = "";
                UpdateSummary();
            }
        }

        private void UpdateTaskGrid()
        {
            TaskGrid.RowDefinitions.Clear();
            TaskGrid.Children.Clear();
            TaskGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            AddHeaderToGrid();

            int rowIndex = 1;
            foreach (var task in Tasks)
            {
                TaskGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                AddTaskToGrid(task, rowIndex);
                rowIndex++;
            }
        }

        private void AddHeaderToGrid()
        {
            var taskHeader = new TextBlock
            {
                Text = "Task",
                Margin = new Avalonia.Thickness(5)
            };
            Grid.SetRow(taskHeader, 0);
            Grid.SetColumn(taskHeader, 0);
            TaskGrid.Children.Add(taskHeader);
            
            var categoryHeader = new TextBlock
            {
                Text = "Category",
                Margin = new Avalonia.Thickness(5)
            };
            Grid.SetRow(categoryHeader, 0);
            Grid.SetColumn(categoryHeader, 1);
            TaskGrid.Children.Add(categoryHeader);
            
            var completionHeader = new TextBlock
            {
                Text = "Done",
                Margin = new Avalonia.Thickness(5)
            };
            Grid.SetRow(completionHeader, 0);
            Grid.SetColumn(completionHeader, 2);
            TaskGrid.Children.Add(completionHeader);
            
            var actionHeader = new TextBlock
            {
                Text = "Deletion",
                Margin = new Avalonia.Thickness(5)
            };
            Grid.SetRow(actionHeader, 0);
            Grid.SetColumn(actionHeader, 3);
            TaskGrid.Children.Add(actionHeader);
        }

        private void AddTaskToGrid(TaskItem task, int rowIndex)
        {
            var taskNameTextBlock = new TextBlock
            {
                Text = task.Name,
                Margin = new Avalonia.Thickness(5)
            };
            Grid.SetRow(taskNameTextBlock, rowIndex);
            Grid.SetColumn(taskNameTextBlock, 0);
            TaskGrid.Children.Add(taskNameTextBlock);
            
            var categoryComboBox = new ComboBox
            {
                Margin = new Avalonia.Thickness(5)
            };
            for (int i = 0; i < CategorySelection.Items.Count; i++)
            {
                categoryComboBox.Items.Add((CategorySelection.Items[i] as ComboBoxItem)?.Content?.ToString());
            }

            categoryComboBox.SelectedItem = task.Category;
            categoryComboBox.SelectionChanged +=
                (box, _) => task.Category = (box as ComboBox).SelectionBoxItem.ToString();
            
            Grid.SetRow(categoryComboBox, rowIndex);
            Grid.SetColumn(categoryComboBox, 1);
            TaskGrid.Children.Add(categoryComboBox);
            
            var checkBox = new CheckBox
            {
                IsChecked = task.IsCompleted,
                Margin = new Avalonia.Thickness(5)
            };
            checkBox.IsCheckedChanged += (_, _) => UpdateCompletionStatus(task);
            Grid.SetRow(checkBox, rowIndex);
            Grid.SetColumn(checkBox, 2);
            TaskGrid.Children.Add(checkBox);

            var deleteButton = new Button
            {
                Content = "Delete",
                Margin = new Avalonia.Thickness(5),
                Tag = task
            };
            deleteButton.Click += DeleteTask;
            Grid.SetRow(deleteButton, rowIndex);
            Grid.SetColumn(deleteButton, 3);
            TaskGrid.Children.Add(deleteButton);
        }

        private void UpdateCompletionStatus(TaskItem task)
        {
            task.IsCompleted = !task.IsCompleted;
            UpdateSummary();
        }

        private void DeleteTask(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TaskItem task)
            {
                Tasks.Remove(task);
                UpdateSummary();
            }
        }

        private void UpdateSummary()
        {
            SummaryText.Text = $"Tasks: {Tasks.Count}, Finished: {Tasks.Count(t => t.IsCompleted)}";
        }
    }


}
