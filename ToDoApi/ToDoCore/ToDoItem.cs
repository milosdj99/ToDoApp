namespace ToDoCore
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = "";
        public bool Checked { get; set; }
        public Guid ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; } = new();
        public int Position { get; set; }

        public string Owner { get; set; } = "";


        public void Update(ToDoItem item)
        {
            Content = item.Content;
            Checked = item.Checked;
        }

        public void UpdatePosition(int pos)
        {
            Position = pos;
        }


    }
}
