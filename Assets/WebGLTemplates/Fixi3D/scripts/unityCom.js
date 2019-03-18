var gameInstance;
var jsonWallsList;

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

window.addEventListener("keydown", function(event) {
	var gameContainer = "BODY";
	var isFocused = (document.activeElement.nodeName == gameContainer);
	if (isFocused) {
		var x = event.key;
		if (event.ctrlKey) {
			if (x == "ArrowLeft") {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'CtrlLeft');
			} else if (x == "ArrowUp") {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'CtrlTop');
			} else if (x == "ArrowRight") {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'CtrlRight');
			} else if (x == "ArrowDown") {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'CtrlBottom');
			}
		} else {
			switch (x) {
				case "c":
				case "C":
					gameInstance.SendMessage('Main Camera', 'SwitchCamera');
					break;
				case "ArrowLeft":
					gameInstance.SendMessage('Main Camera', 'MoveCamera', 'Left');
					break;
				case "ArrowUp":
					gameInstance.SendMessage('Main Camera', 'MoveCamera', 'Top');
					break;
				case "ArrowRight":
					gameInstance.SendMessage('Main Camera', 'MoveCamera', 'Right');
					break;
				case "ArrowDown":
					gameInstance.SendMessage('Main Camera', 'MoveCamera', 'Bottom');
					break;
				default:
					break;
			}
		}
	}
	event.preventDefault();
});

window.addEventListener("wheel", event => {
    const delta = Math.sign(event.deltaY);
    gameInstance.SendMessage("Main Camera", "ZoomCamera", delta);
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