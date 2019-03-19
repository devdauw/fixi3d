var gameInstance;
var jsonWallsList;
const webglDiv = document.getElementById("webgl");
var isFocused;

document.addEventListener("DOMContentLoaded", function () {
    gameInstance = UnityLoader.instantiate("gameContainer",
        UNITY_CONSTANTS.UNITY_WEBGL_BUILD_URL, {
            onProgress: UnityProgress
        });
    return gameInstance;
}, false);

function getLengthFromPage() {
    var size = parseFloat(document.getElementById("input_length").value);
    return size;
}

function getHeightFromPage() {
    var size = parseFloat(document.getElementById("input_height").value);
    return size;
}

function getWidthFromPage() {
    var size = parseFloat(document.getElementById("input_width").value);
    return size;
}

function createWall() {
    gameInstance.SendMessage("WallCreator", "CreateWall");
}

function editWall() {
    var selectedWall = getCurrentSelectedWall();
    var jsonString = JSON.stringify(selectedWall);
    gameInstance.SendMessage("WallCreator", "EditWall", jsonString);
}

function removeWall(){
    var selectedWall = getCurrentSelectedWall();
    var jsonString = JSON.stringify(selectedWall);
    gameInstance.SendMessage("WallCreator", "RemoveWall", jsonString);
}

function copyWall(){
    var selectedWall = getCurrentSelectedWall();
    var jsonString = JSON.stringify(selectedWall);
    gameInstance.SendMessage("WallCreator", "CopyWall", jsonString);
}

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
}

function mouseSelectAction(wallObject) {
    wallObject = JSON.parse(wallObject);
    console.log(wallObject);
}

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
}

//Function to handlekey press, to use with an eventHandler
function handleKeyPress() {
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
        switch (event.key) {
            case "c":
            case "C":
                gameInstance.SendMessage('MainCamera', 'SwitchCamera');
                break;
            case "ArrowLeft":
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
                break;
            case "ArrowUp":
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Top');
                break;
            case "ArrowRight":
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
                break;
            case "ArrowDown":
                gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
                break;
            default:
                break;
        }
    }
}

function handleWheel() {
    const delta = Math.sign(event.deltaY);
    gameInstance.SendMessage("MainCamera", "ZoomCamera", delta);
}

document.addEventListener("click", function(event) {
    if (event.target.id == "#canvas") {
        console.log("Clicked on our div with WebGL content");
        document.addEventListener("keydown", handleKeyPress);
        document.addEventListener("wheel", handleWheel);
    } else {
        console.log("Clicked on another div, removing event and restoring normal keys function");
        document.removeEventListener("keydown", handleKeyPress);
        document.removeEventListener("wheel", handleWheel);
    }
});

document.getElementById("wallsSelect").onchange = function(event) {
    var length = document.getElementById("input_length");
    var height = document.getElementById("input_height");
    var width = document.getElementById("input_width");
    var select = document.getElementById("wallsSelect");
    var selectValue = select[select.selectedIndex].value;
    for(var items in jsonWallsList) {
        jsonWallsList[items].forEach(element => {
            if (element.modelName == selectValue) {
            var ob = element;
            length.value = ob.modelSize.x;
            height.value = ob.modelSize.y;
            width.value = ob.modelSize.z;
        }
    });
    }
}