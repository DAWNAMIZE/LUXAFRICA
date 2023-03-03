mergeInto(LibraryManager.library, {

  CartTime: function (str) {
    window.dispatchReactUnityEvent("CartTime",
      Pointer_stringify(str)
    );
  },

  RequestItem: function (str) {
    window.dispatchReactUnityEvent("RequestItem",
      Pointer_stringify(str)
    );
  },

});