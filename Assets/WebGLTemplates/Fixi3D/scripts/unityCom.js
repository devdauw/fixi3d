var gameInstance;
var jsonWallsList;
var firstTimeL = false;
var firstTimeT = false;
var firstTimeR = false;
var firstTimeB = false;
key = [];

document.addEventListener(
	'DOMContentLoaded',
	function() {
		gameInstance = UnityLoader.instantiate('gameContainer', UNITY_CONSTANTS.UNITY_WEBGL_BUILD_URL, {
			onProgress: UnityProgress
		});
		return gameInstance;
	},
	false
);

function getLengthFromPage() {
	var size = parseFloat(document.getElementById('input_length').value);
	return size;
}

function getHeightFromPage() {
	var size = parseFloat(document.getElementById('input_height').value);
	return size;
}

function getWidthFromPage() {
	var size = parseFloat(document.getElementById('input_width').value);
	return size;
}

function createWall() {
	gameInstance.SendMessage('WallCreator', 'CreateWall');
}

function editWall() {
	var selectedWall = getCurrentSelectedWall();
	var jsonString = JSON.stringify(selectedWall);
	gameInstance.SendMessage('WallCreator', 'EditWall', jsonString);
}

function removeWall() {
	var selectedWall = getCurrentSelectedWall();
	var jsonString = JSON.stringify(selectedWall);
	gameInstance.SendMessage('WallCreator', 'RemoveWall', jsonString);
}

function copyWall() {
	var selectedWall = getCurrentSelectedWall();
	var jsonString = JSON.stringify(selectedWall);
	gameInstance.SendMessage('WallCreator', 'CopyWall', jsonString);
}

function getCSharpModelsList(cSharpList) {
	jsonWallsList = JSON.parse(cSharpList);
	var wallsList = document.getElementById('wallsSelect'),
		option,
		i = 0,
		length = jsonWallsList.Items.length;
	for (a in wallsList.options) {
		wallsList.options.remove(0);
	}
	for (; i < length; i++) {
		option = document.createElement('option');
		option.setAttribute('value', jsonWallsList.Items[i]['modelName']);
		option.appendChild(document.createTextNode(jsonWallsList.Items[i]['modelName']));
		wallsList.appendChild(option);
	}
	return jsonWallsList;
}

function getCurrentSelectedWall() {
	var ob = new Object();
	var select = document.getElementById('wallsSelect');
	var selectValue = select[select.selectedIndex].value;
	for (var items in jsonWallsList) {
		jsonWallsList[items].forEach((element) => {
			if (element.modelName == selectValue) {
				ob = element;
			}
		});
	}
	return ob;
}

function createFix() {
	var selectedWall = getCurrentSelectedWall();
	var jsonString = JSON.stringify(selectedWall);
	gameInstance.SendMessage('WallCreator', 'PlaceFixation', jsonString);
}

onkeydown = onkeyup = function(e) {
	e = e || event; // to deal with IE
	key[e.keyCode] = e.type == 'keydown';
	var l = key.length,
		i;
	var gameContainer = 'BODY';
	var isFocused = document.activeElement.nodeName == gameContainer;
	if (isFocused) {
		for (i = 0; i < l; i++) {
			if (key[8] && key[16] && key[17]) {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'ResetRot');
			} else if (key[17] && key[37]) {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'CtrlLeft');
			} else if (key[17] && key[38]) {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'CtrlTop');
			} else if (key[17] && key[39]) {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'CtrlRight');
			} else if (key[17] && key[40]) {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'CtrlBottom');
			}
			if (key[i]) {
				switch (i) {
					case 67: // "C" key
						gameInstance.SendMessage('Main Camera', 'SwitchCamera');
						break;
					case 37: // Left Arrow
						gameInstance.SendMessage('Main Camera', 'MoveCamera', 'Left');
						firstTimeL = true;
						break;
					case 38: // Top Arrow
						gameInstance.SendMessage('Main Camera', 'MoveCamera', 'Top');
						firstTimeT = true;
						break;
					case 39: // Right Arrow
						gameInstance.SendMessage('Main Camera', 'MoveCamera', 'Right');
						firstTimeR = true;
						break;
					case 40: // Bottom Arrow
						gameInstance.SendMessage('Main Camera', 'MoveCamera', 'Bottom');
						firstTimeB = true;
						break;
					default:
						return;
				}
			}
			if (key[37] == false && firstTimeL) {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'LeftDisable');
				firstTimeL = false;
			} else if (key[38] == false && firstTimeT) {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'TopDisable');
				firstTimeT = false;
			} else if (key[39] == false && firstTimeR) {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'RightDisable');
				firstTimeR = false;
			} else if (key[40] == false && firstTimeB) {
				gameInstance.SendMessage('Main Camera', 'MoveCamera', 'BottomDisable');
				firstTimeB = false;
			}
		}
	}
};

window.addEventListener('wheel', (event) => {
	const delta = Math.sign(event.deltaY);
	gameInstance.SendMessage('Main Camera', 'ZoomCamera', delta);
});

document.getElementById('wallsSelect').onchange = function(event) {
	var length = document.getElementById('input_length');
	var height = document.getElementById('input_height');
	var width = document.getElementById('input_width');
	var select = document.getElementById('wallsSelect');
	var selectValue = select[select.selectedIndex].value;
	for (var items in jsonWallsList) {
		jsonWallsList[items].forEach((element) => {
			if (element.modelName == selectValue) {
				var ob = element;
				length.value = ob.modelSize.x;
				height.value = ob.modelSize.y;
				width.value = ob.modelSize.z;
			}
		});
	}
};
