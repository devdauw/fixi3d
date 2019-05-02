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

	Download : function(json){
		json = Pointer_stringify(json);
		DownloadJson(json);
	},

	SendClear : function(){
		clearSelection();
	},

	SendProjectInfo : function(project){
		project = Pointer_stringify(project);
		SendProject(project);
	}
});