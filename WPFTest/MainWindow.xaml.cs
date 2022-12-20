using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Todo> TodoItems = new ObservableCollection<Todo>(Todo.LoadMockData());
        public MainWindow()
        {
            InitializeComponent();
            TodoGrid.Items.Clear();
            TodoGrid.ItemsSource = TodoItems;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (deadlineTime.Value == null) return;
            Todo todo = new Todo(titleTextBox.Text, deadlineTime.Value ?? DateTime.Now, descriptionTextBox.Text);
            TodoItems.Add(todo);
            titleTextBox.Text = "";
            deadlineTime.Value = null;
            descriptionTextBox.Text = "";

        }

        private void todoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TodoGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TodoGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridPopup.IsOpen = true;
        }

        private void DataGridRow_MouseEnter(object sender, MouseEventArgs e)
        {

            ToolTip tooltip = new ToolTip { Content = "Test" };
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Todo todo = ((FrameworkElement)sender).DataContext as Todo;
            TodoItems.Remove(todo);
        }
        private void TodoGrid_AutoGenerate(object sender, EventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            foreach (var item in grid.Columns)
            {
                switch (item.Header.ToString())
                {
                    case "Remove":
                        item.DisplayIndex = grid.Columns.Count - 1;
                        break;
                    case "Completed":
                        item.Header = "";
                        item.MaxWidth = 20;
                        break;
                }
            }
        }
    }
}
