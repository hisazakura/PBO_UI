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
using System.Windows.Controls.Primitives;
using System.Security.Cryptography;
using System.Text.Json.Nodes;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Todo> TodoItems = new ObservableCollection<Todo>(Todo.GetAllMission());
        public MainWindow()
        {
            InitializeComponent();
            TodoGrid.Items.Clear();
            TodoGrid.ItemsSource = TodoItems;
            ResetInput();
        }

        private void ResetInput()
        {
            titleTextBox.Text = "\x00a0" + "Title";
            titleTextBox.Foreground = SystemColors.GrayTextBrush;
            descriptionTextBox.Text = "\x00a0" + "Description";
            descriptionTextBox.Foreground = SystemColors.GrayTextBrush;
            deadlineDate.SelectedDate = null;
            deadlineDate.Foreground = SystemColors.GrayTextBrush;
            timeComboBox.SelectedIndex = -1;
            timeTextBox.Text = "\x00a0" + "Select a time";
            timeTextBox.Foreground = SystemColors.GrayTextBrush;
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
            Todo.Postdata(todo);
            TodoItems.Add(todo);
            ResetInput();

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
            Todo.Deletedata(todo.Id);
            TodoItems.Remove(todo);
        }
        private void TodoGrid_AutoGenerate(object sender, EventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            foreach (var item in grid.Columns.ToList())
            {
                item.HeaderStyle = new Style(typeof(DataGridColumnHeader));
                item.HeaderStyle.Setters.Add(new Setter(DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
                item.HeaderStyle.Setters.Add(new Setter(DataGridColumnHeader.BackgroundProperty, new SolidColorBrush(Color.FromRgb(255, 185, 0))));
                item.HeaderStyle.Setters.Add(new Setter(DataGridColumnHeader.ForegroundProperty, new SolidColorBrush(Color.FromRgb(0, 53, 102))));
                item.CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0, 53, 102))));
                item.CellStyle.Setters.Add(new Setter(DataGridCell.BorderThicknessProperty, new Thickness(0,0,0,0)));

                switch (item.Header.ToString())
                {
                    case "Checkbox":
                        item.Header = "";
                        break;
                    case "Title":
                        item.MinWidth = 170;
                        item.MaxWidth = 170;
                        break;
                    case "Deadline":
                        item.MinWidth = 155;
                        break;
                    case "Description":
                        item.MinWidth = 300;
                        item.MaxWidth = 300;
                        break;
                    case "Remove":
                        item.DisplayIndex = grid.Columns.Count - 1;
                        item.MinWidth = 80;
                        break;
                    case "Completed":
                        item.Header = "";
                        item.MaxWidth = 20;
                        break;
                    case "Id":
                        TodoGrid.Columns.Remove(item);
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
                titleTextBox.Foreground = SystemColors.ControlLightLightBrush;
                titleTextBox.Text = "";
            }
        }

        private void descriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            string hint = "\x00a0" + "Description";

            if (descriptionTextBox.Text == hint)
            {
                descriptionTextBox.Foreground = SystemColors.ControlLightLightBrush;
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
            if (e.AddedItems.Count == 0)
            {
                timeTextBox.Text = "";
                return;
            }
            timeTextBox.Text = (e.AddedItems[0] as ComboBoxItem).Content as string;
            timeTextBox.Foreground = SystemColors.ControlLightLightBrush;
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

        private void deadlineDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TodoGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            TodoGrid.SelectedItem = null; 
        }

        private void timeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (timeTextBox.Text != "") return;
            timeTextBox.Text = "\x00a0" + "Select a time";
            timeTextBox.Foreground = SystemColors.GrayTextBrush;
        }

        private void timeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (timeTextBox.Text != "\x00a0" + "Select a time") return;
            timeTextBox.Text = "";
            timeTextBox.Foreground = SystemColors.ControlLightLightBrush;
        }

        private void deadlineDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (deadlineDate.Text != "Select a date") return;
            deadlineDate.Foreground = SystemColors.GrayTextBrush;
        }

        private void deadlineDate_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deadlineDate.Foreground != SystemColors.GrayTextBrush) return;
            deadlineDate.Foreground = SystemColors.ControlLightLightBrush;
        }

        private void TodoGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Todo item = (Todo)e.Row.Item;
            string column = e.Column.Header.ToString();
            if (column == "")
            {
                bool modified = (e.EditingElement as CheckBox).IsChecked?? false;
                item = EditField(item, "Checkbox", modified);
            }
            else
            {
                string modified = (e.EditingElement as TextBox).Text;
                item = EditField(item, e.Column.Header.ToString(), modified);
            }
            Todo.Savedata(item);


        }

        private Todo EditField(Todo todo, string field, object value)
        {
            switch (field)
            {
                case "Title":
                    todo.Title = (string)value;
                    break;
                case "Description":
                    todo.Description = (string)value;
                    break;
                case "Checkbox":
                    todo.Checkbox = (bool)value; 
                    break;
            }
            return todo;
        }
    }
}
