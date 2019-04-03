var gameInstance;
var jsonWallsList;
var keysStatus = {};
var firstTimeL = false;
var firstTimeT = false;
var firstTimeR = false;
var firstTimeB = false;
var wall;

document.addEventListener(
	'DOMContentLoaded',
	function() {
		let sidebarColl = document.getElementsByClassName('clickableCollapse');
		let i;

		gameInstance = UnityLoader.instantiate('gameContainer', UNITY_CONSTANTS.UNITY_WEBGL_BUILD_URL, {
			onProgress: UnityProgress
		});

		for (i = 0; i < sidebarColl.length; i++) {
			sidebarColl[i].addEventListener('click', function() {
				this.classList.toggle('active');
				var content = this.nextElementSibling;
				if (content.style.maxHeight) {
					content.style.maxHeight = null;
				} else {
					content.style.maxHeight = content.scrollHeight + 'px';
				}
			});
		}

		return gameInstance;
	},
	false
);

function getFloatValueFromInput(input_name) {
	var size = document.getElementById(input_name).value;
	//Check if our float value has a comma in it, if so transform it to a dot
	if (size.indexOf(',')) {
		size = size.replace(/,/g, '.');
	}
	return parseFloat(size);
}

function createWall() {
	gameInstance.SendMessage('WallCreator', 'CreateWall');
}

function editWall() {
	if (wall == null) return;
	let jsonString = JSON.stringify(wall);
	gameInstance.SendMessage('WallCreator', 'EditWall', jsonString);
}

function removeWall() {
	if (wall == null) return;
	let jsonString = JSON.stringify(wall);
	gameInstance.SendMessage('WallCreator', 'RemoveWall', jsonString);
	let length = document.getElementById('input_edit_length');
	let height = document.getElementById('input_edit_height');
	let width = document.getElementById('input_edit_width');
	length.value = '';
	height.value = '';
	width.value = '';
	wall = null;
}

function copyWall() {
	if (wall == null) return;
	let jsonString = JSON.stringify(wall);
	gameInstance.SendMessage('WallCreator', 'CopyWall', jsonString);
}

function getCSharpModelsList(cSharpList) {
	jsonWallsList = JSON.parse(cSharpList);
	console.log(jsonWallsList);
}

function mouseSelectAction(wallObject) {
	if (wallObject == null) console.log('Null !');
	console.log(wallObject);
	wallObject = JSON.parse(wallObject);
	let length = document.getElementById('input_edit_length');
	let height = document.getElementById('input_edit_height');
	let width = document.getElementById('input_edit_width');
	length.value = wallObject.modelSize.x;
	height.value = wallObject.modelSize.y;
	width.value = wallObject.modelSize.z;
	wall = wallObject;
}

//Function to handlekey press, to use with an eventHandler
function handleKeyDown(event) {
	event.preventDefault();
	if (event.ctrlKey) {
		keysStatus[event.code] = true;
		if (keysStatus['ShiftLeft'] && keysStatus['Backspace']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'ResetRot');
		} else if (keysStatus['ArrowUp'] && keysStatus['ArrowRight']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlTop');
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlRight');
		} else if (keysStatus['ArrowUp'] && keysStatus['ArrowLeft']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlTop');
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlLeft');
		} else if (keysStatus['ArrowDown'] && keysStatus['ArrowRight']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlBottom');
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlRight');
		} else if (keysStatus['ArrowDown'] && keysStatus['ArrowLeft']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlBottom');
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlLeft');
		} else if (keysStatus['ArrowUp']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlTop');
		} else if (keysStatus['ArrowDown']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlBottom');
		} else if (keysStatus['ArrowRight']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlRight');
		} else if (keysStatus['ArrowLeft']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'CtrlLeft');
		}
	} else {
		keysStatus[event.code] = true;
		if (keysStatus['KeyC']) {
			gameInstance.SendMessage('MainCamera', 'SwitchCamera');
		} else if (keysStatus['ArrowUp'] && keysStatus['ArrowRight']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Top');
			firstTimeT = true;
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
			firstTimeR = true;
		} else if (keysStatus['ArrowUp'] && keysStatus['ArrowLeft']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Top');
			firstTimeT = true;
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
			firstTimeL = true;
		} else if (keysStatus['ArrowDown'] && keysStatus['ArrowRight']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
			firstTimeB = true;
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
			firstTimeR = true;
		} else if (keysStatus['ArrowDown'] && keysStatus['ArrowLeft']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
			firstTimeB = true;
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
			firstTimeL = true;
		} else if (keysStatus['ArrowUp']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Top');
			firstTimeT = true;
		} else if (keysStatus['ArrowDown']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
			firstTimeB = true;
		} else if (keysStatus['ArrowRight']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
			firstTimeR = true;
		} else if (keysStatus['ArrowLeft']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
			firstTimeL = true;
		}
	}
}

function handleKeyUp(event) {
	keysStatus[event.code] = false;
	if (!keysStatus['ArrowUp'] && firstTimeT) {
		gameInstance.SendMessage('MainCamera', 'MoveCamera', 'TopDisable');
		firstTimeT = false;
	} else if (!keysStatus['ArrowDown'] && firstTimeB) {
		gameInstance.SendMessage('MainCamera', 'MoveCamera', 'BottomDisable');
		firstTimeB = false;
	} else if (!keysStatus['ArrowRight'] && firstTimeR) {
		gameInstance.SendMessage('MainCamera', 'MoveCamera', 'RightDisable');
		firstTimeR = false;
	} else if (!keysStatus['ArrowLeft'] && firstTimeL) {
		gameInstance.SendMessage('MainCamera', 'MoveCamera', 'LeftDisable');
		firstTimeL = false;
	}
}

function handleWheel() {
	const delta = Math.sign(event.deltaY);
	gameInstance.SendMessage('MainCamera', 'ZoomCamera', delta);
}

document.addEventListener('click', function(event) {
	if (event.target.id == '#canvas') {
		//console.log("Clicked on our div with WebGL content, starting eventHandler to capture keypresses and mousewheel");
		document.addEventListener('keydown', handleKeyDown);
		document.addEventListener('keyup', handleKeyUp);
		document.addEventListener('wheel', handleWheel);
	} else {
		//console.log("Clicked on another div, removing event and restoring normal keys function");
		document.removeEventListener('keydown', handleKeyDown);
		document.removeEventListener('keyup', handleKeyUp);
		document.removeEventListener('wheel', handleWheel);
	}
});
