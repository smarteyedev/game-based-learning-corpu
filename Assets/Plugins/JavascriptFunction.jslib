mergeInto(LibraryManager.library, {
  PlayerTokenFromUrl: function () {
    const urlParams = new URLSearchParams(window.location.search);
    const token = urlParams.get("token") || "";
    const lengthBytes = lengthBytesUTF8(token) + 1;
    const stringOnWasmHeap = _malloc(lengthBytes);
    stringToUTF8(token, stringOnWasmHeap, lengthBytes);
    return stringOnWasmHeap;
  },

  PlayerTokenFromLocalStorage: function () {
    const accessToken = localStorage.getItem("accessToken") || "";
    const lengthBytes = lengthBytesUTF8(accessToken) + 1;
    const stringOnWasmHeap = _malloc(lengthBytes);
    stringToUTF8(accessToken, stringOnWasmHeap, lengthBytes);
    return stringOnWasmHeap;
  },

  WebGLFullscreen: function () {
    try {
      var unityInstance = window.unityInstance;
      if (!unityInstance) {
        console.error("Unity instance not found.");
        return;
      }

      if (typeof unityInstance.SetFullscreen !== "function") {
        console.error("SetFullscreen function not found on unityInstance.");
        return;
      }

      unityInstance.SetFullscreen(1);
    } catch (error) {
      console.error("An error occurred in WebGLFullscreen:", error);
    }
  },

  IsMobile: function () {
    return /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
  },
});
