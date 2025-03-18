using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PreferenceSurvey;

public partial class MainWindow : Window
{
    private bool _isRequirementMet = false;
    private string? _name = "";
    private string? _category = "";
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CheckInputs(object? sender, RoutedEventArgs e)
    {
        NameErrorTextBlock.Text = NameTextBox.Text != "" ? "" : "Enter your name";
        CategoryErrorTextBlock.Text = CategoryComboBox.SelectedItem is not null ? "" : "Choose a category";
        _isRequirementMet = NameTextBox.Text != "" && CategoryComboBox.SelectedItem is not null;
        
        _name = NameTextBox.Text;
        _category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
    }

    private void SummarizeAll(object? sender, RoutedEventArgs e)
    {
        if (!_isRequirementMet)
        {
            SummaryTextBlock.Text = "Not all requirements are met";
            return;
        }
        
        var yesCount = 0;
        var maxYes = CheckboxesGrid.RowDefinitions.Count;
        
        for (int i = 0; i < maxYes; i++)
        {
            var checkBox = CheckboxesGrid.Children.OfType<CheckBox>().FirstOrDefault(checkbox => Grid.GetRow(checkbox) == i);
            yesCount += checkBox?.IsChecked == true ? 1 : 0;
        }

        SummaryTextBlock.Text = $"Name: {_name}\nInterest category: {_category}\n\"Yes\" count: {yesCount}/{maxYes}";
    }
}