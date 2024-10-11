"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/doorHub").build();

connection.on("SyncAcked", function(doorListData) {
    const doorList = JSON.parse(doorListData);
    doorList.forEach(presentDoor);
});

connection.on("HandleError", function(errorMessage) {
    document.getElementById("errors").innerHTML = errorMessage;
});

connection.on("DoorAdded", function(doorData) {
    clearErrors();
    const door = JSON.parse(doorData);
    presentDoor(door);
});

function clearErrors()
{
    document.getElementById("errors").innerHTML = "";
}
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

    var openButtons = document.querySelectorAll('.openButton');
    Array.prototype.forEach.call(openButtons, function addClickListener(btn) {
        btn.addEventListener("click", function(event) {
            var $row = $(this).closest("tr");
            connection.invoke("OpenDoor", $row.find('td:first').text()).catch(function(err) {
                return console.error(err.toString());
            });
            event.preventDefault();
        });
    });

    var closeButtons = document.querySelectorAll('.closeButton');
    Array.prototype.forEach.call(closeButtons, function addClickListener(btn) {
        btn.addEventListener("click", function(event) {
            var $row = $(this).closest("tr");
            connection.invoke("CloseDoor", $row.find('td:first').text()).catch(function(err) {
                return console.error(err.toString());
            });
            event.preventDefault();
        });
    });

    var lockButtons = document.querySelectorAll('.lockButton');
    Array.prototype.forEach.call(lockButtons, function addClickListener(btn) {
        btn.addEventListener("click", function(event) {
            var $row = $(this).closest("tr");
            connection.invoke("LockDoor", $row.find('td:first').text()).catch(function(err) {
                return console.error(err.toString());
            });
            event.preventDefault();
        });
    });

    var unlockButtons = document.querySelectorAll('.unlockButton');
    Array.prototype.forEach.call(unlockButtons, function addClickListener(btn) {
        btn.addEventListener("click", function(event) {
            var $row = $(this).closest("tr");
            connection.invoke("UnlockDoor", $row.find('td:first').text()).catch(function(err) {
                return console.error(err.toString());
            });
            event.preventDefault();
        });
    });

    var removeButtons = document.querySelectorAll('.removeButton');
    Array.prototype.forEach.call(removeButtons, function addClickListener(btn) {
        btn.addEventListener("click", function(event) {
            var $row = $(this).closest("tr");
            connection.invoke("RemoveDoor", $row.find('td:first').text()).catch(function(err) {
                return console.error(err.toString());
            });
            event.preventDefault();
        });
    });
}

connection.on("DoorRemoved", function(doorId) {
    clearErrors();
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
});

connection.on("DoorUpdated", function(doorData) {
    clearErrors();
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
});

connection.start().then(function() {
    document.getElementById("doorTable").disabled = false;
    connection.invoke("InitSync").catch(function(err) {
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