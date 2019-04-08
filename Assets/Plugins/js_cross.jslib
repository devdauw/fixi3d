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

	SendClear : function(){
		clearSelection();
	},
});