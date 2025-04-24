window.setupEmojiPicker = function (dotnetHelper) {
    const observer = new MutationObserver(() => {
        const picker = document.querySelector("emoji-picker");
        if (!picker) return;

        picker.addEventListener("emoji-click", (event) => {
            const emoji = event.detail.unicode;
            dotnetHelper.invokeMethodAsync("EmojiSelected", emoji);
        });

        const handleOutside = (e) => {
            if (!picker.contains(e.target)) {
                dotnetHelper.invokeMethodAsync("CloseEmojiPicker");
                document.removeEventListener("click", handleOutside);
            }
        };

        setTimeout(() => {
            document.addEventListener("click", handleOutside);
        }, 0);

        observer.disconnect();
    });

    observer.observe(document.body, { childList: true, subtree: true });
};
