/* Classes defined in "..\..\..\hakobio-react\src\Shared\UnityAPI.js" */
mergeInto(LibraryManager.library, { 

	Call : function ( name, argument) {

		name = Pointer_stringify(name);
		argument = JSON.parse(Pointer_stringify(argument));
		
		if( 'undefined' != typeof( solutionBuilder.hook.hooks[name] ) )
			for( i = 0; i < solutionBuilder.hook.hooks[name].length; ++i )
				solutionBuilder.hook.hooks[name][i]( argument )
	},

	ExportAPI : function() { 
	
		window.solutionBuilder = {

			/* Model */

			//camera : {},
			
			/* Hook system */

			hook : {
				hooks : [],

				register : function ( name, callback ) {
					if( 'undefined' === typeof( solutionBuilder.hook.hooks[name] ) )
						solutionBuilder.hook.hooks[name] = []
					solutionBuilder.hook.hooks[name].push( callback )
				},
				
				unregister : function(name, callback) {
					if( 'undefined' !== typeof( solutionBuilder.hook.hooks[name] ) ) {
						var index = solutionBuilder.hook.hooks[name].indexOf(callback);
						if(index !== -1) {
						  solutionBuilder.hook.hooks[name].splice(index, 1);
						}
					}
				}
			},

		}
	}
});

