var gameInstance;
var jsonWallsList;
var keysStatus = {};

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
	let selectedWall = getCurrentSelectedWall();
	let jsonString = JSON.stringify(selectedWall);
	gameInstance.SendMessage('WallCreator', 'EditWall', jsonString);
}

function removeWall() {
	let selectedWall = getCurrentSelectedWall();
	let jsonString = JSON.stringify(selectedWall);
	gameInstance.SendMessage('WallCreator', 'RemoveWall', jsonString);
}

function copyWall() {
	let selectedWall = getCurrentSelectedWall();
	let jsonString = JSON.stringify(selectedWall);
	gameInstance.SendMessage('WallCreator', 'CopyWall', jsonString);
}

function getCSharpModelsList(cSharpList) {
	jsonWallsList = JSON.parse(cSharpList);
	let wallsList = document.getElementById('wallsSelect'),
		option,
		length = jsonWallsList.Items.length;
	for (let item in wallsList.options) {
		wallsList.options.remove(0);
	}
	for (let i = 0; i < length; i++) {
		option = document.createElement('option');
		option.setAttribute('value', jsonWallsList.Items[i]['modelName']);
		option.appendChild(document.createTextNode(jsonWallsList.Items[i]['modelName']));
		wallsList.appendChild(option);
	}
	return jsonWallsList;
}

function mouseSelectAction(wallObject) {
	wallObject = JSON.parse(wallObject);
	console.log(wallObject);
}

function getCurrentSelectedWall() {
	let ob = {};
	let select = document.getElementById('wallsSelect');
	let selectValue = select[select.selectedIndex].value;
	for (let items in jsonWallsList) {
		jsonWallsList[items].forEach((element) => {
			if (element.modelName == selectValue) {
				ob = element;
			}
		});
	}
	return ob;
}

//Function to handlekey press, to use with an eventHandler
function handleKeyDown(event) {
	event.preventDefault();
	if (event.ctrlKey) {
		keysStatus[event.code] = true;
		if (keysStatus['ArrowUp'] && keysStatus['ArrowRight']) {
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
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
		} else if (keysStatus['ArrowUp'] && keysStatus['ArrowLeft']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Top');
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
		} else if (keysStatus['ArrowDown'] && keysStatus['ArrowRight']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
		} else if (keysStatus['ArrowDown'] && keysStatus['ArrowLeft']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
		} else if (keysStatus['ArrowUp']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Top');
		} else if (keysStatus['ArrowDown']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Bottom');
		} else if (keysStatus['ArrowRight']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Right');
		} else if (keysStatus['ArrowLeft']) {
			gameInstance.SendMessage('MainCamera', 'MoveCamera', 'Left');
		}
	}
}

function handleKeyUp(event) {
	keysStatus[event.code] = false;
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

document.getElementById('wallsSelect').onchange = function(event) {
	let length = document.getElementById('input_edit_length');
	let height = document.getElementById('input_edit_height');
	let width = document.getElementById('input_edit_width');
	let select = document.getElementById('wallsSelect');
	let selectValue = select[select.selectedIndex].value;
	for (let item in jsonWallsList) {
		jsonWallsList[item].forEach((element) => {
			if (element.modelName == selectValue) {
				let ob = element;
				length.value = ob.modelSize.x.toFixed(3);
				height.value = ob.modelSize.y.toFixed(3);
				width.value = ob.modelSize.z.toFixed(3);
			}
		});
	}
};
