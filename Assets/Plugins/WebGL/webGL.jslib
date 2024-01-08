mergeInto( LibraryManager.library, {
	setHeaderLayout: function (type){
    	window.dispatchReactUnityEvent(
    			"setHeaderLayout",
    			Pointer_stringify(type)
    	);
    },
    openBoardModal: function (){
        window.dispatchReactUnityEvent(
    	    	"openBoardModal"
        );
    },
    openLoadingModal: function (){
        window.dispatchReactUnityEvent(
   	    	    "openLoadingModal"
        );
    },
    closeLoadingModal: function (){
        window.dispatchReactUnityEvent(
         	    "closeLoadingModal"
        );
    },
    okayToLeave: function (){
        window.dispatchReactUnityEvent(
         	    "okayToLeave"
        );
    },
    openNickAlert: function (){
         window.dispatchReactUnityEvent(
          	    "openNickAlert"
         );
    },
    pingAck: function (){
         window.dispatchReactUnityEvent(
          	    "pingAck"
         );
    },
    loadScene: function (percent){
         window.dispatchReactUnityEvent(
          	    "loadScene",
    			Pointer_stringify(percent)
         );
    },
    onDisconnectServer: function (percent){
         window.dispatchReactUnityEvent(
                "onDisconnectServer"
         );
    }
});