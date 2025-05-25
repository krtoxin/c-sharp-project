window.openInNewTab = function (url) {
    window.open(url, '_blank');
};

window.applyTheme = function (theme) {
    const body = document.body;
    body.classList.remove("light-theme", "default-theme");

    if (theme === "light") {
        body.classList.add("light-theme");
    } else {
        body.classList.add("default-theme");
    }

    localStorage.setItem("theme", theme);
};

document.addEventListener("DOMContentLoaded", function () {
    const saved = localStorage.getItem("theme");
    if (saved) {
        window.applyTheme(saved);
    }
});
