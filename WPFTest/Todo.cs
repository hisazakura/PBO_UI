using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WPFTest
{
    internal class Todo
    {
        public bool Checkbox { get; set; } = false;
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }

        public Todo(string Title, DateTime Deadline, string Description)
        {
            this.Title = Title;
            this.Deadline = Deadline;
            this.Description = Description;
        } 
        public static List<Todo> GetAllMission()
        {
            string conn = "https://localhost:7043/api/Mission";

            var client = new RestClient(conn);
            var request = new RestRequest(conn, Method.Get);
            var response = client.Execute(request);

            List<Todo> data = JsonConvert.DeserializeObject<List<Todo>>(response.Content);


            return data;
        }
        public static void Postdata(Todo Mission)
        {
            string conn = "https://localhost:7043/api/Mission";

            var client = new RestClient(conn);
            var request = new RestRequest(conn, Method.Post);
            var response = client.Execute(request);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/xml");
            request.AddJsonBody(Mission);

            client.Post(request);
        }
        public static void Deletedata(int id)
        {
            string conn = $"https://localhost:7043/api/Mission/{id}";

            var client = new RestClient(conn);
            var request = new RestRequest(conn, Method.Delete);
            var response = client.Execute(request);

            request.AddHeader("Content-Type", "application/json");

        }
        public static void  Savedata(Todo Mission)
        {
            string conn = $"https://localhost:7043/api/Mission/{Mission.Id}";

            var client = new RestClient(conn);
            var request = new RestRequest(conn, Method.Put);
     

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/xml");
            request.AddJsonBody(Mission);
            var response = client.Execute(request);

        }
        override public string ToString()
        {
            return this.Id.ToString() + "\n" + this.Title.ToString() + "\n" + this.Deadline.ToString() + "\n" + this.Description.ToString() + "\n" + this.Checkbox.ToString();
        }
    }

}
