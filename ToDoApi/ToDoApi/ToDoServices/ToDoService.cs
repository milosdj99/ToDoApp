using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using ToDoApi.Exceptions;
using ToDoCore;
using ToDoInfrastructure;

namespace ToDoApi.ToDoServices
{
    public class ToDoService
    {

        private readonly ToDoDbContext _context;


        public ToDoService(ToDoDbContext context)
        {
            _context = context;
        }

        
        public List<ToDoList> GetAllLists()
        {

            List<ToDoList> returnList = new List<ToDoList>();

            returnList = _context.ToDoLists.Include(x => x.Items).OrderByDescending(x => x.Position).ToList();
            return returnList;
        }

        public ToDoList GetToDoListById(Guid id) =>
            _context.ToDoLists.Include(x => x.Items).FirstOrDefault(x => x.Id == id);


        public List<ToDoList> GetToDoListsBySearchCriteria(string criteria)
        {
            return _context.ToDoLists
                .Where(x => x.Title.Contains(criteria) || x.Items.Any(item => item.Content.Contains(criteria))).Include(x => x.Items).ToList();
        }

        public ToDoList AddToDoList(ToDoList newList, string email, string owner)
        {
            newList.Email = email;
            newList.Owner = owner;
            newList.Notified = false;

            newList.UpdatePosition(_context.ToDoLists.Count());

            _context.ToDoLists.Add(newList);
            _context.SaveChanges();
            return newList;
        }

        public bool ModifyToDoList(ToDoList newList)
        {
            var existingList = _context.ToDoLists.FirstOrDefault(x => x.Id == newList.Id);

            if (existingList != null)
            {
                existingList.Update(newList);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteToDoList(Guid id)
        {
            var existingList = _context.ToDoLists.FirstOrDefault(x => x.Id == id);

            if (existingList != null)
            {
                _context.ToDoLists.Remove(existingList);

                _context.ToDoLists.Where(x => x.Position > existingList.Position).ToList().ForEach(x => x.UpdatePosition(x.Position - 1));

                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public ToDoItem? GetToDoItemById(Guid listId, Guid id)
        {
            var list = _context.ToDoLists.FirstOrDefault(x => x.Id == listId);
            if (list == null)
            {
                return null;
            }
            return _context.ToDoItems.FirstOrDefault(x => x.Id == id);
        }

        public bool AddToDoItem(ToDoItem newItem, string owner)
        {
            var list = _context.ToDoLists.Include(x => x.Items).FirstOrDefault(x => x.Id == newItem.ToDoListId);

            if (list == null)
            {
                return false;
            }

            newItem.UpdatePosition(list.Items.Count());
            newItem.Owner = owner;
            
            list.Items.Add(newItem);
            _context.SaveChanges();
            return true;
        }

        public bool ModifyToDoItem(ToDoItem newItem)
        {
            var existingItem = _context.ToDoItems.FirstOrDefault(x => x.Id == newItem.Id && x.ToDoListId == newItem.ToDoListId);

            if (existingItem != null)
            {
                existingItem.Update(newItem);
                _context.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }

        public bool DeleteToDoItem(Guid listId, Guid id)
        {
            var list = _context.ToDoLists.Include(x=>x.Items).FirstOrDefault(x => x.Id == listId);

            if(list == null)
            {
                return false;
            }

            var existingItem = list.Items.Where(x => x.Id == id).FirstOrDefault();

            if (existingItem != null)
            {
                _context.ToDoItems.Remove(existingItem);

                _context.ToDoItems.Where(x => x.Position > existingItem.Position && x.ToDoListId == listId).ToList().ForEach(x => x.UpdatePosition(x.Position - 1));

                _context.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }


        public void UpdateToDoListPosition(Guid listId, int newPosition)
        {
            var list = _context.ToDoLists.Find(listId);

            if (newPosition < 0 || newPosition >= _context.ToDoLists.Count())
            {
                throw new UpdateException("BadRequest");
            }

            if(list == null)
            {
                throw new UpdateException("NotFound");
            }

            int currentPosition = list.Position;          

            _context.ToDoLists.Where(x => x.Position > currentPosition).ToList().ForEach(x => x.UpdatePosition(x.Position - 1));

            list.UpdatePosition(newPosition);

            _context.SaveChanges();

            _context.ToDoLists.Where(x => x.Position >= newPosition && x.Id != listId).ToList().ForEach(x => x.UpdatePosition(x.Position + 1));

            _context.SaveChanges();

            

        }

        public void UpdateToDoItemPosition(Guid listId, Guid itemId, int newPosition)
        {
            var item = _context.ToDoItems.Find(itemId);

            if (_context.ToDoLists.Find(listId) == null || item == null)
            {
                throw new UpdateException("NotFound");
            }

            if (newPosition < 0 || newPosition >= _context.ToDoItems.Where(x => x.ToDoListId == listId).Count())
            {
                throw new UpdateException("BadRequest");
            }

            int currentPosition = item.Position;           

            _context.ToDoItems
                .Where(x => x.Position > currentPosition && x.ToDoListId == listId)
                .ToList()
                .ForEach(x => x.UpdatePosition(x.Position - 1));

            item.UpdatePosition(newPosition);

            _context.SaveChanges();


            _context.ToDoItems
                .Where(x => x.Position >= newPosition && x.Id != itemId && x.ToDoListId == listId)
                .ToList()
                .ForEach(x => x.UpdatePosition(x.Position + 1));

            _context.SaveChanges();  
        }

        public ToDoShareList AddSharedList(Guid listId)
        {
            ToDoShareList newList = new ToDoShareList()
            {
                ListId = listId,
                ExpirationTime = DateTime.Now.AddHours(2)
            };

            _context.ToDoSharedLists.Add(newList);
            _context.SaveChanges();
            return newList;
        }



        public ToDoList GetSharedList(Guid sharedId)
        {
            DateTime expTime = _context.ToDoSharedLists.FirstOrDefault(x => x.Id == sharedId)!.ExpirationTime;

            if (expTime < DateTime.Now)
            {
                return null;
            }

            Guid originalId = _context.ToDoSharedLists.FirstOrDefault(x => x.Id == sharedId)!.ListId;

            return _context.ToDoLists.FirstOrDefault(x => x.Id == originalId);
        }

    }
}
