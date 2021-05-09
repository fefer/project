# project
Brick abode project

To run:

cd ./FSharpWebApiToDoList/FSToDoList
dotnet run

Using CURL

List all:
curl http://localhost:5000/api/todoitems

Remove:

Removes the id 1:
curl -X DELETE -v http://localhost:5000/api/todoitems/1

Using Postman

Insert (Id : 0, Name : "EURUSD", Value : 1.4):

URL: http://localhost:5000/api/todoitems/

Key: Content-Type

Value: application/json

Body: { Id : 0, Name : "EURUSD", Value : 1.4 }


Search id 4 with name "EURUSD":
URL: http://localhost:5000/api/ToDoItems/search

Key: Content-Type

Value: application/json

Body (ex): [{ Id : 4, Name : "EURUSD"}]
