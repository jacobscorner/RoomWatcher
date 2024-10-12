# Room watcher

I picked up challenge 3 with the requirements written in challenge 1. My main idea was to stick to expectations of challenge 3.
- implement async notification to the clients based on actions from peer clients.
- express the logic in c#

## Why I chose challenge 3?
- Though I'm ready and eager to learn to new stuffs which are react, wpf, wcf, I didnot want to fall out on timeline.
- Yet, I had to ramp-up on 
    - aspnet webapp
    - javascript
    - signalr framework.

## Summary
Using signalr client<>server communication, I provide clients to talk to each other with a
server catalyzation. This is aspnet web app with a javascript client.

## Features
- UI
    * I went with simple ui which includes door configuration as well as I didnt want to spend too much time in making it flamboyant.
    * A new client can see an start screen with all existing doors. This is paramount as a client can be spun at a later point in time.
    * Clients can add(with a label) and remove doors.
    * There are 4 actions.
        - On IsClosed property, we have two actions 'Open' and 'Close'.
        - On IsLocked property, we have two actions 'Lock' and 'Unlock'.
    * warnings and errors are event prompt driven, hence its already returned to caller who triggered the event
    * success messages are broadcasted to all clients as the event-affected changes are reflected at presentation layers of all clients. This can also act as a notification to clients telling there are new changes.
    * If any action, which triggers no state change, its a warning. like going from open to open state.
    * An open door can be closed and vice versa unless its locked.
    * Locked door can be unlocked and vice versa only if its closed.
    * Clients see the changes concurrently.

## Design

![A](ComponentDiagram.jpg?raw=true "Component Diagram")

![B](EventFlowDiagram.jpg?raw=true "Eventflow diagram")

* js client is built with html and raw js.
* signarlr-hub(server) has been implemented with components top down as listed below
    - Hub/ controller(DoorHub.cs)
        * receives events and delegates to Service layer
        * returns messages and serialized data objects back to clients asynchronously.
    - Service(DoorService.cs)
        * carries out business logic including validations
        * delegate CRUD to db provider and returns to controller
    - DB provider(DoorStubRepositoryProvider)
        * works with DB for CRUD operations and returns to service layer
        * I have used a stub dictionary to mimic db.
    - Model(Door.cs)
        * core definition of door entity. For the ease of expression and since its not
        production, I have also assumed it as DTO.

## Questions to Customer
- Should door carry a state ? (in maintenance) such that we can handle its update and delete operations accordingly.
- Should doors have an association to enclosure since doors can have shared enclosures.
    * Enclosure can be comprised when one of the door is compromised
- Synchronous alerting to target group incase of certain events (sms, call, etc)
- Analytics of certain events. How many times a specific door has been opened, such stuffs.
- Role based access and authorization level for doors.
    * door can be added and removed by admin.
    * door can be locked/unlocked by security.
    * door can be opened/closed by employees.
- UX improvements.

## Things to expand
- Im not talking about additional features since i mentioned those in 'Questions to customer' section
- add dto and mapper to map b/n dto and model in both directions
- improved error handling with exception class and its specializations along with more validations
- more thin data transfer(objectref, properties changed, operation) between client and server. Currently im passing the complete object
- Move it mvc binding based UI client so that I dont want to restitch UI manually on data change.
- db implementation. Reduce the data(getDoor()) comparing to current implementation.
- docker integration for transporting. I have left this as Im yet to ramp up on it.