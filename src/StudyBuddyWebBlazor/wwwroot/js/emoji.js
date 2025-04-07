window.setupEmojiPicker = function (dotnetHelper) {
    const observer = new MutationObserver(() => {
        const picker = document.querySelector("emoji-picker");
        if (!picker) return;

        picker.addEventListener("emoji-click", (event) => {
            const emoji = event.detail.unicode;
            dotnetHelper.invokeMethodAsync("EmojiSelected", emoji);
        });

        observer.disconnect();
    });

    observer.observe(document.body, { childList: true, subtree: true });
};