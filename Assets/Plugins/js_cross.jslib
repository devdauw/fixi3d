mergeInto (LibraryManager.library, {

	SendWallsToPage : function(cSharpList){
		cSharpList = Pointer_stringify(cSharpList);
		getCSharpModelsList(cSharpList);
	},
	
	SendClickedWallToPage : function(wallObject) {
		wallObject = Pointer_stringify(wallObject);
		mouseSelectAction(wallObject);	
	},
	
	GetLengthFromPage : function() {
		return getLengthFromPage();
	},

	GetHeightFromPage : function() {
		return getHeightFromPage();
	},

	GetWidthFromPage : function() {
		return getWidthFromPage();
	},

});