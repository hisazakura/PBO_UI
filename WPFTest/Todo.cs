using System;
using System.Collections.Generic;
using System.Text;

namespace WPFTest
{
    internal class Todo
    {
        public bool Completed { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }
        
        public Todo(string title, DateTime deadline, string desc)
        {
            this.Title = title;
            this.Deadline = deadline;
            this.Description = desc;
            this.Completed = false;
        }
        public static List<Todo> LoadMockData()
        {
            List<Todo> MockData = new List<Todo>();
            MockData.Add(new Todo("Tugas PBO", new DateTime(2022, 12, 23), "Menyelesaikan Tugas PBO"));
            MockData.Add(new Todo("Natalan", new DateTime(2022, 12, 25), "Menikmati Natalan dan diskon Steam"));
            MockData.Add(new Todo(
                "Tes Deskripsi Panjang",
                DateTime.Now,
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));
            return MockData;
        }

    }

}
