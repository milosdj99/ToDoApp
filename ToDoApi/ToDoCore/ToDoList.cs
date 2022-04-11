namespace ToDoCore
{
    public class ToDoList
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public List<ToDoItem> Items { get; set; } = new();
        public DateTime? ReminderDate { get; set; }
        public int Position { get; set; }
        public bool Notified { get; set; }
        public string Owner { get; set; } = "";
        public string Email { get; set; } = "";


        public void Update(ToDoList list)
        {
            Title = list.Title;
            ReminderDate = list.ReminderDate;
        }

        public void UpdatePosition(int pos)
        {
            Position = pos;
        }
        
       
    }
}
