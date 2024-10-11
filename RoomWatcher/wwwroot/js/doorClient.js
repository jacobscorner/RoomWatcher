"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/doorHub").build();

let listenerMapForOpenButton = {};
let listenerMapForCloseButton = {};
let listenerMapForLockButton = {};
let listenerMapForUnlockButton = {};
let listenerMapForRemoveButton = {};

connection.on("HandleError", function (message) {
    addErrorMessage(message);        
});

connection.on("HandleWarning", function (message) {
    addWarningMessage(message);
});

function addSuccessMessage(message) {
    clearMessages();
    addMessage(message, 'green');
}

function addErrorMessage(message) {
    clearMessages();
    addMessage(message, 'red');
}

function addWarningMessage(message) {
    clearMessages();
    addMessage(message, 'orange');
}

function addMessage(message, color) {
    var messageElement = document.getElementById("messages");
    messageElement.innerHTML = message;
    messageElement.style.fontWeight = 'bold';
    messageElement.style.color = color;
}

function clearMessages() {
    document.getElementById("messages").innerHTML = "";
}

connection.on("SynchronizeClient", function (doorListData) {
    const doorList = JSON.parse(doorListData);
    doorList.forEach(presentDoor);
});
connection.on("DoorAdded", function(doorData) {    
    const door = JSON.parse(doorData);
    presentDoor(door);
    addSuccessMessage("A new door has been added.");
});

function presentDoor(door)
{
    var table = document.getElementById("doorTable");
    var row = table.insertRow();
    var doorIdCell = row.insertCell(0);
    var doorLabelCell = row.insertCell(1);
    var doorIsClosedCell = row.insertCell(2);
    var actionOpenCell = row.insertCell(3);
    var actionCloseCell = row.insertCell(4);
    var doorIsLockedCell = row.insertCell(5);
    var actionLockCell = row.insertCell(6);
    var actionUnlockCell = row.insertCell(7);
    var actionRemoveCell = row.insertCell(8);


    doorIdCell.innerHTML = `${ door.Id}`;
    doorLabelCell.innerHTML = `${ door.Label}`;
    doorIsClosedCell.innerHTML = `${ door.IsClosed}`;
    actionOpenCell.innerHTML = "<button class='openButton'>Open</button>";
    actionCloseCell.innerHTML = "<button class='closeButton'>Close</button>";
    doorIsLockedCell.innerHTML = `${ door.IsLocked}`;
    actionLockCell.innerHTML = "<button class='lockButton'>Lock</button>";
    actionUnlockCell.innerHTML = "<button class='unlockButton'>Unlock</button>";
    actionRemoveCell.innerHTML = "<button class='removeButton'>Remove</button>";

    registerClickEventForAction('.openButton', listenerMapForOpenButton, "OpenDoor");   
    registerClickEventForAction('.closeButton', listenerMapForCloseButton, "CloseDoor");   
    registerClickEventForAction('.lockButton', listenerMapForLockButton, "LockDoor");   
    registerClickEventForAction('.unlockButton', listenerMapForUnlockButton, "UnlockDoor");    
    registerClickEventForAction('.removeButton', listenerMapForRemoveButton, "RemoveDoor");    
}

function registerClickEventForAction(queryIdentifier, listenerMap, serverReceiverMethodName) {
    var buttons = document.querySelectorAll(queryIdentifier);
    Array.prototype.forEach.call(buttons, function addClickListener(btn) {
        var doorId = $(btn).closest("tr").find('td:first').text();
        if (!(doorId in listenerMap)) {
            listenerMap[doorId] = true;
            btn.addEventListener("click", function (event) {
                var $row = $(this).closest("tr");
                connection.invoke(serverReceiverMethodName, $row.find('td:first').text()).catch(function (err) {
                    return console.error(err.toString());
                });
                event.preventDefault();
            });
        }
    });
}

connection.on("DoorRemoved", function(doorId) {
    var table = document.getElementById("doorTable");
    var indexToRemove = 0;
    for (var i = 1, row; row = table.rows[i]; i++)
    {
        if (row.cells[0].innerHTML == doorId)
        {
            row.remove();
            break;
        }
    }
    addSuccessMessage("A door has been removed.");
});

connection.on("DoorUpdated", function(doorData) {
    const door = JSON.parse(doorData);
    var table = document.getElementById("doorTable");
    var doorId = `${ door.Id}`;

    for (var i = 1, row; row = table.rows[i]; i++)
    {
        if (row.cells[0].innerHTML == doorId)
        {
            row.cells[2].innerHTML = `${ door.IsClosed}`;
            row.cells[5].innerHTML = `${ door.IsLocked}`;
            break;
        }
    }
    addSuccessMessage("Door's status has been updated");
});

connection.start().then(function() {
    document.getElementById("doorTable").disabled = false;
    connection.invoke("InitSynchronizeClient").catch(function(err) {
        return console.error(err.toString());
    });
}).catch(function(err) {
    return console.error(err.toString());
});

document.getElementById("addDoor").addEventListener("click", function(event) {
    var doorLabel = document.getElementById("doorLabel").value;
    connection.invoke("AddDoor", doorLabel).catch(function(err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});