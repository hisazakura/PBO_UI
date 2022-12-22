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
            titleTextBox.Text = "\x00a0" + "Title";
            titleTextBox.Foreground = SystemColors.GrayTextBrush;
            descriptionTextBox.Text = "\x00a0" + "Description";
            descriptionTextBox.Foreground = SystemColors.GrayTextBrush;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (deadlineDate.SelectedDate == null) return;
            if (titleTextBox.Text == "\x00a0" + "Title") return;
            if (descriptionTextBox.Text == "\x00a0" + "Description") return;
            DateTime date = deadlineDate.SelectedDate?? DateTime.Today;
            TimeSpan time;
            try
            {
                time = ConvertToTime(timeTextBox.Text);
            }
            catch (ArgumentException)
            {
                return;
            }
            DateTime deadline = date + time;

            Todo todo = new Todo(titleTextBox.Text, deadline, descriptionTextBox.Text);
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

        private void titleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string hint = "\x00a0" + "Title";
            if (titleTextBox.Text == "")
            {
                titleTextBox.Foreground = SystemColors.GrayTextBrush;
                titleTextBox.Text = hint;
            }
        }

        private void titleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            string hint = "\x00a0" + "Title";

            if (titleTextBox.Text == hint)
            {
                titleTextBox.Foreground = SystemColors.ActiveCaptionTextBrush;
                titleTextBox.Text = "";
            }
        }

        private void descriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            string hint = "\x00a0" + "Description";

            if (descriptionTextBox.Text == hint)
            {
                descriptionTextBox.Foreground = SystemColors.ActiveCaptionTextBrush;
                descriptionTextBox.Text = "";
            }
        }

        private void descriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string hint = "\x00a0" + "Description";
            if (descriptionTextBox.Text == "")
            {
                descriptionTextBox.Foreground = SystemColors.GrayTextBrush;
                descriptionTextBox.Text = hint;
            }
        }

        private void timeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timeTextBox.Text = (e.AddedItems[0] as ComboBoxItem).Content as string;
        }

        private TimeSpan ConvertToTime(string timeString)
        {
            char[] timeLetters = timeString.ToArray();
            if (timeLetters[2].Equals(":"))
            {
                throw new ArgumentException("Invalid Time-Formatted String");
            }
            if (timeString.Length != 5)
            {
                throw new ArgumentException("Invalid Time-Formatted String");
            }

            char[] hourChar = { timeLetters[0], timeLetters[1] };
            string hourString = new string(hourChar);
            int hour;
            bool isValid = int.TryParse(hourString, out hour);
            if (!isValid || hour > 24)
            {
                throw new ArgumentException("Invalid Time-Formatted String");
            }

            char[] minuteChar = { timeLetters[3], timeLetters[4] };
            string minuteString = new string(minuteChar);
            int minute;
            isValid = int.TryParse(minuteString, out minute);
            if (!isValid || minute > 60)
            {
                throw new ArgumentException("Invalid Time-Formatted String");
            }

            return new TimeSpan(hour, minute, 0);
        }
        private void closeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
                Trace.WriteLine("Error");
            }
        }
        private void minApp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void dragRegion_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
