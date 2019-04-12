mergeInto (LibraryManager.library, {

	SendWallsToPage : function(cSharpList){
		cSharpList = Pointer_stringify(cSharpList);
		getCSharpModelsList(cSharpList);
	},
	
	SendClickedWallToPage : function(wallObject) {
		wallObject = Pointer_stringify(wallObject);
		mouseSelectAction(wallObject);	
	},
	
	GetFloatValueFromInput : function(input_name) {
		input_name = Pointer_stringify(input_name);
		return getFloatValueFromInput(input_name);
	},

	GetStringValueFromInput : function(input_name){
		input_name = Pointer_stringify(input_name);
		console.log("Before get it");
		var value = getStringValueFromInput(input_name) + '';
		console.log("After getting it");
		console.log(value);
		console.log(typeof value);
		return value;
	},

	SendClear : function(){
		clearSelection();
	},
});