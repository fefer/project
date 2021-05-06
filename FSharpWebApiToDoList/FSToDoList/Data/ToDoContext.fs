namespace FSToDoList

open FSToDoList.Models
open Microsoft.EntityFrameworkCore
open System.Linq

module DataContext =

    type ToDoContext(options : DbContextOptions<ToDoContext>) = 
        inherit DbContext(options)
    
        [<DefaultValue>]
        val mutable ToDoItems : DbSet<ToDoItem>
        member public this._ToDoItems      with    get()   = this.ToDoItems 
                                           and     set value  = this.ToDoItems <- value 
 
        member this.ToDoItemExist (id:int) = this.ToDoItems.Any(fun x -> x.Id = id) // verify if the item exist

        member this.GetToDoItem (id:int) = this.ToDoItems.Find(id) // check the id to return the item

    let Initialize (context : ToDoContext) =
        context.Database.EnsureCreated() |> ignore // verify if the database exists, if it does not, then creates the database
        
        let tdItems : ToDoItem[] = 
            [|
                { Id = 0; Name = "GBPUSD"; Value = 1.2988 }
                { Id = 0; Name = "GBPBRL"; Value = 3.7398 }
                { Id = 0; Name = "GBPNIS"; Value = 1.1843 }
           |]


        if not(context.ToDoItems.Any()) then
                context.ToDoItems.AddRange(tdItems) |> ignore
                context.SaveChanges() |> ignore  
