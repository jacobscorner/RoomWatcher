# Room watcher

I picked up challenge 3 with the requirements written in challenge 1. My main idea was to stick to expectations of challenge 3.
- implement async notification to the clients based on actions from peer clients.
- express the logic in c#

## Why I chose challenge 3?
- Though I'm ready and eager to learn new stuffs, I didnot want to fall out on timeline.
- Yet, I had to ramp-up on 
    - aspnet webapp
    - javascript
    - signalr framework.

## Summary
Using signalr client<>server communication, clients communicate with each other mediated by a 
server. This is an asp.net web app including javascript client.

## Features
* I went with a simple ui which includes door configuration and monitoring since I didnt want to spend too much time on UI.
* A new client can see a start screen with all existing doors. This is paramount as a new client can be spun at a later point in time.
* Clients can add doors(with a label), remove doors, perform below actions.
* There are 4 actions.
    - On IsClosed property, we have two actions 'Open' and 'Close'.
    - On IsLocked property, we have two actions 'Lock' and 'Unlock'.
* warnings and errors are only returned to caller who triggered the event.
* success messages are broadcasted to all clients as changes are relevant to them. This can also act as a notification about new changes.
* If any action, which triggers no state change, its a warning. eg, Going from 'open' to 'open' state.
* An open door can be closed.
* A closed door can be opened unless its locked.
* Locked door can be unlocked.
* A door can be locked only if its closed.
* Clients see the changes concurrently.

## Design

![A](ComponentDiagram.jpg?raw=true "Component Diagram")

![B](EventFlowDiagram.jpg?raw=true "Eventflow diagram")

* js client is built with html and raw js.
* signarlr-hub(server) has been implemented with components top down as listed below. Service
    - Hub/ controller(DoorHub.cs)
        * receives events and delegates to Service layer
        * returns messages and serialized data objects back to clients asynchronously.
    - Service(DoorService.cs)
        * carries out business logic including validations
        * delegate CRUD to db provider and returns to controller
    - DB provider(DoorStubRepositoryProvider.cs)
        * works with DB for CRUD operations and returns to service layer
        * I have used a static dictionary to mimic db.
    - Model(Door.cs)
        * core definition of door entity. For the ease of expression and since its not
        production, I have also assumed it as the DTO.

## Doors table format
  * ID(door Id)
  * Label(door name)
  * Is Closed(When the value is 'true', the door is closed)
  * Open(action button to open the door)
  * Close(action button to close the door)
  * Is Locked(When the value is 'true', the door is locked)
  * Lock(action button to lock the door)
  * Unlock(action button to unlock the door)
  * Remove(action button to remove the respective door)
      
## Questions to Customer
- Do we need created-at, updated-at for doors.
- Do we need user support ? client need to login with user credentials.
    * If yes, does door need created-by, last updated-by info.
    * if yes, should we add role based access and authorization level for doors, door groups, enclosures. Eg below:
        - door can be added and removed by admin.
        - door can be locked/unlocked by security.
        - door can be opened/closed by employees.
- Do we have query requirements for doors ?  It can be used for filtering from UI.
    * If yes,  should we support tagging the doors? eg add 'important' tag to a door so that we can use it in filter queries
- Should door carry a state ? eg. 'Under maintenance'/'Temporarily down' such that we can handle door properties accordingly.
- Should doors have compositions?
    * An Enclosure can have multiple doors. Enclosure can be comprised when one of the door is compromised.
    * Set of doors can be grouped to door-groups. i.e those belongs to a particular floor or area etc.
- Alerting to target group incase of certain events, actions (sms, call etc).
- Analytics of certain events, state changes. eg. How many times a specific door has been opened, such stuffs.
- UX improvements.

## Things to expand
- I've added areas of feature expansion under previous 'Questions to Customer' section.
- Add dto and mapper( to map between dto and model in both directions) such that external model is disjoint with internal model.
- In enterprise form, I would like to switch the static classes to singleton classes(implemented on interfaces) such that I can dictate the structure and make it unit testing friendly.
- In enterprise form, I would like to add logging.
- In enterprise form, I would like an improved error handling setup with exception classes and its specializations along with more validations.
- more thin data transfer(objectref, properties changed, operation) between client and server. Currently im passing the complete object.
- Move to binding-based UI client(MVC approach) so that I dont want to restitch UI manually on data change.
- Add actual db implementation. Reduce the data-fetch (getDoor()) comparing to current implementation.
- docker integration for transporting. I have left this as Im yet to ramp up on it.

## Steps to run the app
- Install VS 2022.
- Clone this repo.
- Build the RoomWatcher project.
- Run the app.
- You will get the client-app popped-up in https://localhost:7067/
- Open this in more tabs to spin more clients.

## Testcase
- Add Door
    * Provide a 'Door Label' value & Click on 'Add Door' button. _You should see the new door added in the table below._
    * Click on 'Add Door' without providing a door label. _You should see an error message._
- Perform Actions(Open, Close, Lock, Unlock, Remove) 
    * Add few doors
    * Try combinations of Actions in the table.
      - Verify : An open door can be closed.
      - Verify : A closed door can be opened unless its locked.
      - Verify : Locked door can be unlocked.
      - Verify : A door can be locked only if its closed.
    * verfiy the concurrent changes in all clients along with the success message alerts.
- Spin up a new client after adding few doors from another client. _You should see existing doors listed in new client._
- After adding 2 clients(A and B),
    * From client A, perform 'open' action  when the door is open(IsClosed = false). _You should observe warning in  client A only._
    * From client A, perform 'Add Door' with no label. _You should observe error in  client A only._

