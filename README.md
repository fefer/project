# Brick abode project

Connect psql with F# app

- Start dotnet:

```
cd ./FSharpWebApiToDoList/FSToDoList
dotnet run
```

### Using CURL

- List all:

```
curl http://localhost:5000/api/todoitems
```

- Removes the id 1:
```
curl -X DELETE -v http://localhost:5000/api/todoitems/1
```

### Using Postman

- Using post, insert (Id : 0, Name : "EURUSD", Value : 1.4):

```
URL: http://localhost:5000/api/todoitems/
Key: Content-Type
Value: application/json
Body: { Id : 0, Name : "EURUSD", Value : 1.4 }
```


- Using post search id 4 with name "EURUSD":

```
URL: http://localhost:5000/api/ToDoItems/search
Key: Content-Type
Value: application/json
Body: [{ Id : 4, Name : "EURUSD"}]
```
