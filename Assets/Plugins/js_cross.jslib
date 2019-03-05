mergeInto (LibraryManager.library, {

	GetModelsList : function(cSharpList){
		cSharpList = Pointer_stringify(cSharpList);
		getCSharpModelsList(cSharpList);
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