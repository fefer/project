namespace FSToDoList

open FSToDoList.DataContext
open FSToDoList.Models
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open System.Collections.Generic
open System.Linq;

[<Route("api/ToDoItems")>]
[<ApiController>]
type ToDoItemsController private () = 
    inherit ControllerBase()
    new (context : ToDoContext) as this =
        ToDoItemsController () then
        this._Context <- context
    
    [<HttpGet>]
    member this.Get() =
        ActionResult<IEnumerable<ToDoItem>>(this._Context.ToDoItems)

    //GET: api/ToDoItems/search
    [<Route("search")>]
    //[<HttpGet>]
    [<HttpPost>]
    //member this.Get([<FromBody>] _Values : ToDoItem[] ) =
    member this.Post([<FromBody>] _Values : ToDoItem[] ) =
        if base.ModelState.IsValid then //check if the entry is valid
            
            let ToDoItems : List<ToDoItem> = new List<ToDoItem>() // starts the list
            
            for value in _Values do // look for all the items either by name or id
                if (value.Id = 0) then
                    ToDoItems.AddRange(this._Context.ToDoItems.Where(fun x -> x.Name = value.Name).ToList())
                else if(this._Context.ToDoItemExist(value.Id)) then
                    ToDoItems.Add(this._Context.ToDoItems.Find(value.Id))

            if (ToDoItems.Count = 0) then
                ActionResult<IEnumerable<ToDoItem>>(base.NotFound("ITEM NOT PRESENT"))
            else
                ActionResult<IEnumerable<ToDoItem>>(base.Ok(ToDoItems))
        else 
            ActionResult<IEnumerable<ToDoItem>>(base.BadRequest(base.ModelState))

    [<HttpGet("{id}")>]
    member this.Get(id:int) = 
        if base.ModelState.IsValid then  // check the if the entry is valid
            if not ( this._Context.ToDoItemExist(id) ) then // check if ToDoItem exist
                ActionResult<IActionResult>(base.NotFound("ToDoItem NOT PRESENT"))
            else
                ActionResult<IActionResult>(base.Ok(this._Context.GetToDoItem(id)))
        else
            ActionResult<IActionResult>(base.BadRequest(base.ModelState))

    [<HttpPost>]
    member this.Post([<FromBody>] _ToDoItem : ToDoItem) =
        if (base.ModelState.IsValid) then // check if the entry is valid
            if not( isNull _ToDoItem.Name ) then
                if ( _ToDoItem.Id <> 0 ) then // check if the ID 0
                    ActionResult<IEnumerable<ToDoItem>>(base.BadRequest("WRONG ID"))
                else 
                        this._Context.ToDoItems.Add(_ToDoItem) |> ignore
                        this._Context.SaveChanges() |> ignore
                        //ActionResult<JsonResult>(base.Ok(this._Context.ToDoItems.Last()))
                        ActionResult<IEnumerable<ToDoItem>>(this._Context.ToDoItems)
            else
                ActionResult<IEnumerable<ToDoItem>>(base.BadRequest("NULL INITIAL FIELD"))                    
        else
            ActionResult<IEnumerable<ToDoItem>>(base.BadRequest(base.ModelState))

    [<HttpPut("{id}")>]
     member this.Put( id:int, [<FromBody>] _ToDoItem : ToDoItem) =
        if (base.ModelState.IsValid) then // check if the entry is valid
            if not( isNull _ToDoItem.Name ) then
                if (_ToDoItem.Id <> id) then 
                    ActionResult<IActionResult>(base.BadRequest())
                else
                        try //error handler
                            this._Context.Entry(_ToDoItem).State = EntityState.Modified |> ignore
                            this._Context.SaveChanges() |> ignore
                            ActionResult<IActionResult>(base.Ok(_ToDoItem))
                        with ex ->
                            if not( this._Context.ToDoItemExist(id) ) then
                                ActionResult<IActionResult>(base.NotFound())
                            else 
                                ActionResult<IActionResult>(base.BadRequest())
            else
                ActionResult<IActionResult>(base.BadRequest())                                
        else    
            ActionResult<IActionResult>(base.BadRequest(base.ModelState))

    [<HttpDelete("{id}")>]
    member this.Delete(id:int) =
        if (base.ModelState.IsValid) then // check if entry is valid 
            if not( this._Context.ToDoItemExist(id) ) then 
                ActionResult<IActionResult>(base.NotFound())
            else (
                    this._Context.ToDoItems.Remove(this._Context.GetToDoItem(id)) |> ignore
                    this._Context.SaveChanges() |> ignore
                    ActionResult<IActionResult>(base.Ok(this._Context.ToDoItems.Last()))
            )
        else
            ActionResult<IActionResult>(base.BadRequest(base.ModelState))

    [<DefaultValue>]
    val mutable _Context : ToDoContext
