(function () {
    var root = document.documentElement;

    function applyTheme(theme) {
        root.setAttribute("data-theme", theme);
        var icon = document.querySelector("#themeToggle i");
        if (icon) {
            icon.className = theme === "dark" ? "fas fa-sun" : "fas fa-moon";
        }
    }

    function currentTheme() {
        var stored = localStorage.getItem("tetas-theme");
        if (stored) {
            return stored;
        }
        return window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light";
    }

    applyTheme(currentTheme());

    document.addEventListener("DOMContentLoaded", function () {
        applyTheme(currentTheme());

        var toggle = document.getElementById("themeToggle");
        if (toggle) {
            toggle.addEventListener("click", function () {
                var next = root.getAttribute("data-theme") === "dark" ? "light" : "dark";
                localStorage.setItem("tetas-theme", next);
                applyTheme(next);
            });
        }

        var sidebar = document.getElementById("sidebar");
        var scrim = document.getElementById("scrim");
        var menu = document.getElementById("menuToggle");

        function closeSidebar() {
            if (sidebar) { sidebar.classList.remove("open"); }
            if (scrim) { scrim.classList.remove("show"); }
        }

        if (menu) {
            menu.addEventListener("click", function () {
                if (sidebar) { sidebar.classList.toggle("open"); }
                if (scrim) { scrim.classList.toggle("show"); }
            });
        }
        if (scrim) {
            scrim.addEventListener("click", closeSidebar);
        }
    });

    window.tetasReact = function (btn, postId) {
        var tokenEl = document.querySelector('input[name="__RequestVerificationToken"]');
        var token = tokenEl ? tokenEl.value : "";
        btn.disabled = true;
        fetch("/Posts/React?id=" + postId + "&type=Like", {
            method: "POST",
            headers: { "RequestVerificationToken": token }
        })
            .then(function (r) { return r.ok ? r.json() : Promise.reject(); })
            .then(function (data) {
                btn.classList.toggle("reacted", !!data.mine);
                var countEl = btn.querySelector(".react-count");
                if (countEl) { countEl.textContent = data.total; }
            })
            .catch(function () {})
            .finally(function () { btn.disabled = false; });
    };
})();
