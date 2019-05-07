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

function SendProject(project) {
	var json = JSON.parse(project);
	document.getElementById('input_name').value = json.projectName;
	document.getElementById('input_num').value = json.projectNum;
	document.getElementById('input_customer').value = json.customerName;
	document.getElementById('input_user').value = json.userName;
}

function getFloatValueFromInput(input_name) {
	var size = document.getElementById(input_name).value;
	//Check if our float value has a comma in it, if so transform it to a dot
	if (size.indexOf(',')) {
		size = size.replace(/,/g, '.');
	}
	return parseFloat(size);
}

function saveProject() {
	var json = JSON.stringify({
		projectName: document.getElementById('input_name').value,
		projectNum: document.getElementById('input_num').value,
		customerName: document.getElementById('input_customer').value,
		userName: document.getElementById('input_user').value
	});
	gameInstance.SendMessage('WallCreator', 'SaveProject', json);
}

function DownloadJson(json) {
	var filename = document.getElementById('input_name').value + '.json';
	var type = 'application/json';
	var file = new Blob([ json ], { type: type });
	if (
		window.navigator.msSaveOrOpenBlob // IE10+
	)
		window.navigator.msSaveOrOpenBlob(file, filename);
	else {
		// Others
		var a = document.createElement('a'),
			url = URL.createObjectURL(file);
		a.href = url;
		a.download = filename;
		document.body.appendChild(a);
		a.click();
		setTimeout(function() {
			document.body.removeChild(a);
			window.URL.revokeObjectURL(url);
		}, 0);
	}
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
	console.log(wallObject);
	var event = new MouseEvent('click');
	wallObject = JSON.parse(wallObject);
	var butProject = document.getElementById('butProjet');
	if (butProject.getAttribute('class') == 'clickableCollapse active') butProject.dispatchEvent(event);
	var butCreate = document.getElementById('butCreateWall');
	if (butCreate.getAttribute('class') == 'clickableCollapse active') butCreate.dispatchEvent(event);
	var butAction = document.getElementById('ActionController');
	var butSelect = document.getElementById('butSelectWall');
	var butFixations = document.getElementById('FixationsDisplayer');
	var editionMode = document.getElementById('substract');
	editionMode.checked = true;
	gameInstance.SendMessage('WallCreator', 'Substract', 'true');
	butProject.setAttribute('style', 'display: none;');
	butCreate.setAttribute('style', 'display: none;');
	butAction.setAttribute('style', 'display: inline-block;');
	butSelect.setAttribute('style', 'display: inline-block;');
	butFixations.setAttribute('style', 'display: inline-block;');
	let length = document.getElementById('input_edit_length');
	let height = document.getElementById('input_edit_height');
	let width = document.getElementById('input_edit_width');
	let posX = document.getElementById('input_edit_posX');
	let posY = document.getElementById('input_edit_posY');
	let posZ = document.getElementById('input_edit_posZ');
	length.value = wallObject.modelSize.x;
	height.value = wallObject.modelSize.y;
	width.value = wallObject.modelSize.z;
	posX.value = wallObject.modelPosition.x;
	posY.value = wallObject.modelPosition.y;
	posZ.value = wallObject.modelPosition.z;
	//TODO: Implementation de toutes les fixations en arbre
	var array_fixName = wallObject.modelFixationsName;
	var array_fixPosition = wallObject.modelFixationsPosition;
	var currentDiv = document.getElementById('fixControl');
	for (var i = 0; i < array_fixName.length; i++) {
		var newName = document.createElement('LABEL');
		newName.setAttribute('class', 'label');
		newName.innerHTML = array_fixName[i];
		var newDiV = document.createElement('div');
		newDiV.setAttribute('class', 'control-label');
		newDiV.appendChild(newName);
		currentDiv.appendChild(newDiV);

		var newRendement = document.createElement('LABEL');
		newRendement.innerText = 'Rendement 80%';
		var newDiVRend = document.createElement('div');
		newDiVRend.setAttribute('class', 'control-label');
		newDiVRend.appendChild(newRendement);
		currentDiv.appendChild(newDiVRend);

		var newLabX = document.createElement('LABEL');
		newLabX.innerHTML = 'X:';
		var newDivPosX = document.createElement('div');
		newDivPosX.setAttribute('class', 'control-label');
		newDivPosX.appendChild(newLabX);
		currentDiv.appendChild(newDivPosX);

		var newInpX = document.createElement('INPUT');
		newInpX.setAttribute('type', 'text');
		newInpX.setAttribute('name', 'fixX' + i);
		newInpX.setAttribute('id', 'input_edit_fixX' + i);
		newInpX.setAttribute('class', 'input is-small');
		newInpX.value = array_fixPosition[i].x;
		var newDivInpPosX = document.createElement('div');
		newDivInpPosX.setAttribute('class', 'control');
		newDivInpPosX.appendChild(newInpX);
		currentDiv.appendChild(newDivInpPosX);

		var newLabY = document.createElement('LABEL');
		newLabY.innerHTML = 'Y:';
		var newDivPosY = document.createElement('div');
		newDivPosY.setAttribute('class', 'control-label');
		newDivPosY.appendChild(newLabY);
		currentDiv.appendChild(newDivPosY);

		var newInpY = document.createElement('INPUT');
		newInpY.setAttribute('type', 'text');
		newInpY.setAttribute('name', 'fixY' + i);
		newInpY.setAttribute('id', 'input_edit_fixY' + i);
		newInpY.setAttribute('class', 'input is-small');
		newInpY.value = array_fixPosition[i].y;
		var newDivInpPosY = document.createElement('div');
		newDivInpPosY.setAttribute('class', 'control');
		newDivInpPosY.appendChild(newInpY);
		currentDiv.appendChild(newDivInpPosY);

		var newLabZ = document.createElement('LABEL');
		newLabZ.innerHTML = 'Z:';
		var newDivPosZ = document.createElement('div');
		newDivPosZ.setAttribute('class', 'control-label');
		newDivPosZ.appendChild(newLabZ);
		currentDiv.appendChild(newDivPosZ);

		var newInpZ = document.createElement('INPUT');
		newInpZ.setAttribute('type', 'text');
		newInpZ.setAttribute('name', 'fixZ' + i);
		newInpZ.setAttribute('id', 'input_edit_fixZ' + i);
		newInpZ.setAttribute('class', 'input is-small');
		newInpZ.value = array_fixPosition[i].z;
		var newDivInpPosZ = document.createElement('div');
		newDivInpPosZ.setAttribute('class', 'control');
		newDivInpPosZ.appendChild(newInpZ);
		currentDiv.appendChild(newDivInpPosZ);
	}
	wall = wallObject;
}

function sub() {
	var sub = 'true';
	var renfort = 'false';
	if (document.getElementById('substract').checked) {
		gameInstance.SendMessage('WallCreator', 'AddRenfort', renfort);
		gameInstance.SendMessage('WallCreator', 'Substract', sub);
	} else {
		sub = 'false';
		renfort = 'true';
		gameInstance.SendMessage('WallCreator', 'AddRenfort', renfort);
		gameInstance.SendMessage('WallCreator', 'Substract', sub);
	}
}

function showLineRenderer() {
	var value = 'true';
	if (document.getElementById('lineRenderer').checked) {
		gameInstance.SendMessage('MouseManager', 'EnableLineRenderer', value);
	} else {
		value = 'false';
		gameInstance.SendMessage('MouseManager', 'EnableLineRenderer', value);
	}
}

function CalcFix() {
	var name = wall.modelName;
	gameInstance.SendMessage('WallCreator', 'PlaceFixation', name);
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

function clearSelection() {
	var event = new MouseEvent('click');
	var butProject = document.getElementById('butProjet');
	var butCreate = document.getElementById('butCreateWall');
	var butAction = document.getElementById('ActionController');
	if (butAction.getAttribute('class') == 'clickableCollapse active') butAction.dispatchEvent(event);
	var butSelect = document.getElementById('butSelectWall');
	if (butSelect.getAttribute('class') == 'clickableCollapse active') butSelect.dispatchEvent(event);
	var butFixations = document.getElementById('FixationsDisplayer');
	if (butFixations.getAttribute('class') == 'clickableCollapse active') butFixations.dispatchEvent(event);
	butProject.setAttribute('style', 'display: inline-block;');
	butCreate.setAttribute('style', 'display: inline-block;');
	butAction.setAttribute('style', 'display: none;');
	butSelect.setAttribute('style', 'display: none;');
	butFixations.setAttribute('style', 'display: none;');
	document.getElementById('fixControl').innerHTML = '';
	gameInstance.SendMessage('WallCreator', 'Substract', 'false');
	gameInstance.SendMessage('WallCreator', 'AddRenfort', 'false');
}

document.addEventListener('click', function(event) {
	if (event.target.id == '#canvas') {
		document.addEventListener('keydown', handleKeyDown);
		document.addEventListener('keyup', handleKeyUp);
		document.addEventListener('wheel', handleWheel);
	} else {
		document.removeEventListener('keydown', handleKeyDown);
		document.removeEventListener('keyup', handleKeyUp);
		document.removeEventListener('wheel', handleWheel);
	}
});

(function() {
	function onChange(event) {
		var reader = new FileReader();
		reader.onload = onReaderLoad;
		reader.readAsText(event.target.files[0]);
	}

	function onReaderLoad(event) {
		var json = event.target.result;
		gameInstance.SendMessage('WallCreator', 'LoadProject', json);
	}

	document.getElementById('pathLoader').addEventListener('change', onChange);
})();
