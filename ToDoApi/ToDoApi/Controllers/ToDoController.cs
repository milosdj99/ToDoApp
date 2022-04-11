using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Serilog;
using ToDoApi.Exceptions;
using ToDoApi.ToDoServices;
using ToDoCore;
using ToDoApi.ExtensionMethods;

namespace ToDoApi.Controllers
{
    [Route("api/to-do-lists")]
    [ApiController]
    public class ToDoController : ControllerBase
    {

        private readonly ToDoService _service;


        public ToDoController(ToDoService service)
        {
            _service = service;
        }

   

        [Authorize(Policy = "get:lists")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllLists()
        {
            Log.Debug("ToDoApi.GetAllLists() executed!");
            return Ok(_service.GetAllLists());
        }

        [Authorize(Policy = "get:list")]
        [HttpGet("lists/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetToDoListById([FromRoute] Guid id)
        {
            Log.Debug("ToDoApi.GetToDoListById() executed!");
            return Ok(_service.GetToDoListById(id));
        }

        [Authorize(Policy = "search:lists")]
        [HttpGet("search/{criteria}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SearchLists([FromRoute] string criteria)
        {
            Log.Debug("ToDoApi.SearchLists() executed!");
            return Ok(_service.GetToDoListsBySearchCriteria(criteria));
        }


        [Authorize(Policy = "add:list")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddList([FromBody] ToDoList newList)
        {
            try
            {
                string email = this.getUserEmail();
                string owner = this.getUserName();
                var list = _service.AddToDoList(newList, email, owner);
                Log.Debug("ToDoApi.AddList() executed!");


                return CreatedAtAction(nameof(GetToDoListById), new { id = newList.Id }, list);
            }
            catch (SqlException ex)
            {
                Log.Error($"Sql exception occured : {ex.StackTrace}!");
                return BadRequest();
            }
        }

        [Authorize(Policy = "modify:list")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ModifyList(ToDoList newList)
        {
            if (_service.ModifyToDoList(newList))
            {
                Log.Debug("ToDoApi.ModifyList() executed!");
                return Ok();
            }

            return NotFound();
        }


        [Authorize(Policy = "delete:list")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteList([FromRoute] Guid id)
        {
            if (_service.DeleteToDoList(id))
            {
                Log.Debug("ToDoApi.DeleteList() executed!");
                return Ok();
            }

            Log.Debug("ToDoApi.DeleteList() failed!");
            return NotFound();
        }

        [Authorize(Policy = "get:item")]
        [HttpGet("{listId}/to-do-items/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetItemById([FromRoute] Guid listId, [FromRoute] Guid id)
        {
            var item = _service.GetToDoItemById(listId, id);
            Log.Debug("ToDoApi.GetItemById() executed!");

            return item == null ? NotFound() : Ok(item);
        }


        [Authorize(Policy = "add:item")]
        [HttpPost("{listId}/add-item")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddItem([FromRoute] Guid listId, [FromBody] ToDoItem newItem)
        {
            try
            {
                Log.Debug("ToDoApi.AddItem() executed!");

                string owner = this.getUserName();
                return _service.AddToDoItem(newItem, owner)
                    ? CreatedAtAction(nameof(GetItemById), new { listId, id = newItem.Id }, newItem)
                    : NotFound();
            }
            catch (SqlException ex)
            {
                Log.Error($"Sql exception occured: {ex.StackTrace}!");
                return BadRequest();
            }
        }

        [Authorize(Policy = "modify:item")]
        [HttpPut("{listId}/to-do-items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ModifyItem([FromRoute] Guid listId, [FromBody] ToDoItem newItem)
        {
            if (_service.ModifyToDoItem(newItem))
            {
                Log.Debug("ToDoApi.ModifyItem() executed!");
                return Ok();
            }

            return NotFound();
        }

        [Authorize(Policy = "delete:item")]
        [HttpDelete("{listId}/to-do-items/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteItem([FromRoute] Guid listId, [FromRoute] Guid id)
        {
            if (_service.DeleteToDoItem(listId, id))
            {
                Log.Debug("ToDoApi.DeleteItem() executed!");
                return Ok();
            }

            return NotFound();
        }

        [Authorize(Policy = "modify:list-position")]
        [HttpPut("{listId}/position/{newPosition}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateToDoListPosition([FromRoute] Guid listId, [FromRoute] int newPosition)
        {
            try
            {
                _service.UpdateToDoListPosition(listId, newPosition);
            }
            catch (UpdateException e)
            {
                if (e.Message == "NotFound")
                {

                    return NotFound();

                }
                else if (e.Message == "BadRequest")
                {

                    return BadRequest();

                }
            }
            return Ok();
        }

        [Authorize(Policy = "modify:item-position")]
        [HttpPut("{listId}/item-position/{itemId}/{newPosition}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateToDoItemPosition([FromRoute] Guid listId, [FromRoute] Guid itemId, [FromRoute] int newPosition)
        {
            try
            {
                _service.UpdateToDoItemPosition(listId, itemId, newPosition);
            }
            catch (UpdateException e)
            {
                if (e.Message == "NotFound")
                {

                    return NotFound();

                }
                else if (e.Message == "BadRequest")
                {

                    return BadRequest();

                }
            }
            return Ok();
        }

        [HttpPost("share/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddSharedToDoList([FromRoute] Guid id)
        {
            Log.Debug("ToDoApi.AddSharedToDoList() executed!");

            try
            {
                var list = _service.AddSharedList(id);

                return Ok(list.Id);
            }
            catch (SqlException ex)
            {
                Log.Error($"Sql exception occured : {ex.StackTrace}!");
                return BadRequest();
            }
        }

        [HttpGet("share/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ViewSharedToDoList([FromRoute] Guid id)
        {
            Log.Debug("ToDoApi.ViewSharedToDoList() executed!");
            return _service.GetSharedList(id) != null ? Ok(_service.GetSharedList(id)) : NotFound();
        }

    }
}




