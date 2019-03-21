var gameInstance;
var jsonWallsList;
var keysStatus = {};

document.addEventListener("DOMContentLoaded", function () {
    var sidebarColl = document.getElementsByClassName("clickableCollapse");
    var i;

    gameInstance = UnityLoader.instantiate("gameContainer",
        UNITY_CONSTANTS.UNITY_WEBGL_BUILD_URL, {
            onProgress: UnityProgress
        });

    for (i = 0; i < sidebarColl.length; i++) {
        sidebarColl[i].addEventListener("click", function() {
            this.classList.toggle("active");
            var content = this.nextElementSibling;
            if (content.style.maxHeight){
                content.style.maxHeight = null;
            } else {
                content.style.maxHeight = content.scrollHeight + "px";
            }
        });
    }

    return gameInstance;
}, false);

function getFloatValueFromInput(input_name) {
    var size = document.getElementById(input_name).value;
    //Check if our float value has a comma in it, if so transform it to a dot
    if (size.indexOf(",")) {
        size = size.replace(/,/g, '.');
    }
    var sizeFloat = parseFloat(size);
    return sizeFloat;
}

function createWall() {
    gameInstance.SendMessage("WallCreator", "CreateWall");
};

function editWall() {
    var selectedWall = getCurrentSelectedWall();
    var jsonString = JSON.stringify(selectedWall);
    gameInstance.SendMessage("WallCreator", "EditWall", jsonString);
};

function removeWall(){
    var selectedWall = getCurrentSelectedWall();
    var jsonString = JSON.stringify(selectedWall);
    gameInstance.SendMessage("WallCreator", "RemoveWall", jsonString);
};

function copyWall(){
    var selectedWall = getCurrentSelectedWall();
    var jsonString = JSON.stringify(selectedWall);
    gameInstance.SendMessage("WallCreator", "CopyWall", jsonString);
};

function getCSharpModelsList(cSharpList) {
    jsonWallsList = JSON.parse(cSharpList);
    var wallsList = document.getElementById("wallsSelect"),
        option,
        i = 0,
        length = jsonWallsList.Items.length;
    for(a in wallsList.options){
        wallsList.options.remove(0);
    }
    for (; i < length; i++) {
        option = document.createElement("option");
        option.setAttribute("value", jsonWallsList.Items[i]["modelName"]);
        option.appendChild(document.createTextNode(jsonWallsList.Items[i]["modelName"]));
        wallsList.appendChild(option);
    }
    return jsonWallsList;
};

function mouseSelectAction(wallObject) {
    wallObject = JSON.parse(wallObject);
    console.log(wallObject);
};

function getCurrentSelectedWall() {
    var ob = new Object;
    var select = document.getElementById("wallsSelect");
    var selectValue = select[select.selectedIndex].value;
    for(var items in jsonWallsList) {
        jsonWallsList[items].forEach(element => {
            if (element.modelName == selectValue) {
            ob = element;
        }
    });
    }
    return ob;
};

//Function to handlekey press, to use with an eventHandler
function handleKeyDown() {
    event.preventDefault();
    if (event.ctrlKey) {
        var arrow = event.key;
        switch (arrow) {
            case "ArrowLeft":
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlLeft');
                break;
            case "ArrowUp":
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlTop');
                break;
            case "ArrowRight":
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlRight');
                break;
            case "ArrowDown":
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlBottom');
                break;
            default:
                break;
        }
    } else {
        if (event.code == "KeyC") {
            gameInstance.SendMessage('MainCamera', 'SwitchCamera');
            return;
        } else {
            keysStatus[event.code] = true;
            if (keysStatus["ArrowUp"] && keysStatus["ArrowRight"]) {
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Top');
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
            } else if (keysStatus["ArrowUp"] && keysStatus["ArrowLeft"]) {
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Top');
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
            } else if (keysStatus["ArrowDown"] && keysStatus["ArrowRight"]) {
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
            } else if (keysStatus["ArrowDown"] && keysStatus["ArrowLeft"]) {
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
            }  else if (keysStatus["ArrowUp"]) {
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Top');
            } else if (keysStatus["ArrowDown"]) {
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
            } else if (keysStatus["ArrowRight"]) {
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
            } else if (keysStatus["ArrowLeft"]) {
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
            }
        }
    }
};

function handleKeyUp() {
    keysStatus[event.code] = false;
}

function handleWheel() {
    const delta = Math.sign(event.deltaY);
    gameInstance.SendMessage("MainCamera", "ZoomCamera", delta);
};

document.addEventListener("click", function(event) {
    if (event.target.id == "#canvas") {
        //console.log("Clicked on our div with WebGL content, starting eventHandler to capture keypresses and mousewheel");
        document.addEventListener("keydown", handleKeyDown);
        document.addEventListener("keyup", handleKeyUp);
        document.addEventListener("wheel", handleWheel);
    } else {
        //console.log("Clicked on another div, removing event and restoring normal keys function");
        document.removeEventListener("keydown", handleKeyDown);
        document.removeEventListener("keyup", handleKeyUp);
        document.removeEventListener("wheel", handleWheel);
    }
});

document.getElementById("wallsSelect").onchange = function(event) {
    var length = document.getElementById("input_edit_length");
    var height = document.getElementById("input_edit_height");
    var width = document.getElementById("input_edit_width");
    var select = document.getElementById("wallsSelect");
    var selectValue = select[select.selectedIndex].value;
    for(var items in jsonWallsList) {
        jsonWallsList[items].forEach(element => {
            if (element.modelName == selectValue) {
            var ob = element;
            length.value = ob.modelSize.x.toFixed(3);
            height.value = ob.modelSize.y.toFixed(3);
            width.value = ob.modelSize.z.toFixed(3);
        }
    });
    }
};