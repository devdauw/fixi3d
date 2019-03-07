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
        i = 0,
        length = jsonWallsList.Items.length;
    for(a in wallsList.options){
        wallsList.options.remove(0);
    }
    for (; i < length; i++) {
        option = document.createElement('option');
        option.setAttribute('value', jsonWallsList.Items.modelName);
        option.appendChild(document.createTextNode(jsonWallsList.Items[i]['modelName']));
        wallsList.appendChild(option);
    }
}

function sendWallSelected(){
    var wallsList = document.getElementById("wallsSelect");
    gameInstance.SendMessage('WallCreator', 'SelectedWall', wallsList.options[wallsList.selectedIndex].text);
}

//Fonction permettant de s'assurer que le CANVAS est selectionne afin de permettre l'interaction avec le clavier
window.addEventListener('keydown', function(event) {
    var gameContainer = "BODY";
    var isFocused = (document.activeElement.nodeName == gameContainer);
    console.log(isFocused);
    if (isFocused) {
        //Si notre canvas est focus on regarde quelle touche du clavier est utilisee
        switch (event.key) {
            //Tranfert d'une camera a l'autre
            case "c":
            case "C":
                gameInstance.SendMessage('Main Camera', 'SwitchCamera');
                break;
            default:
                return;
        }

        event.preventDefault();
    }
}, true);






