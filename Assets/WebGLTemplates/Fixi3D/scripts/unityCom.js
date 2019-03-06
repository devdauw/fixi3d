var gameInstance;

document.addEventListener('DOMContentLoaded', function () { 
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
    gameInstance.SendMessage('WallCreator', 'CreateWall');
    gameInstance.SendMessage('WallCreator', 'GetWallsList');
}

function editWall() {
    gameInstance.SendMessage('WallCreator', 'EditWall');
}

function destroyWall(){
    gameInstance.SendMessage('WallCreator', 'DestroyWall');
}

function getCSharpModelsList(cSharpList) {
    var jsonWallsList = JSON.parse(cSharpList);
    var wallsList = document.getElementById("wallsSelect"),
        option,
        i = jsonWallsList.Items.length - 1,
        length = jsonWallsList.Items.length;

    for (; i < length; i++) {
        option = document.createElement('option');
        option.setAttribute('value', jsonWallsList.Items.modelName);
        option.appendChild(document.createTextNode(jsonWallsList.Items[i]['modelName']));
        wallsList.appendChild(option);
    }
}

window.addEventListener('keydown', function(event) {
    switch (event.key) {
        case "c":
            gameInstance.SendMessage('Main Camera', 'SwitchCamera');
            break;
        default:
            return;
    }

    event.preventDefault();
}, true);






