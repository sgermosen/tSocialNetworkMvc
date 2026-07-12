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

        initNotifications();
    });

    function antiforgeryToken() {
        var el = document.querySelector('input[name="__RequestVerificationToken"]');
        return el ? el.value : "";
    }

    function timeAgo(iso) {
        var then = new Date(iso).getTime();
        var mins = Math.floor((Date.now() - then) / 60000);
        if (mins < 1) { return "just now"; }
        if (mins < 60) { return mins + "m ago"; }
        var hrs = Math.floor(mins / 60);
        if (hrs < 24) { return hrs + "h ago"; }
        return Math.floor(hrs / 24) + "d ago";
    }

    function initNotifications() {
        var bell = document.getElementById("bellBtn");
        if (!bell) { return; }

        var panel = document.getElementById("notifPanel");
        var list = document.getElementById("notifList");
        var badge = document.getElementById("notifBadge");
        var markAll = document.getElementById("markAllBtn");

        function setBadge(count) {
            if (count > 0) {
                badge.textContent = count > 99 ? "99+" : count;
                badge.hidden = false;
            } else {
                badge.hidden = true;
            }
        }

        function render(data) {
            setBadge(data.unread);
            if (!data.items || data.items.length === 0) {
                list.innerHTML = '<div class="notif-empty">No notifications yet.</div>';
                return;
            }
            list.innerHTML = data.items.map(function (n) {
                var cls = n.isRead ? "notif-item" : "notif-item unread";
                var href = n.url ? n.url : "#";
                return '<a class="' + cls + '" href="' + href + '" data-id="' + n.id + '">' +
                    escapeHtml(n.message) +
                    '<span class="notif-time">' + timeAgo(n.createdAt) + "</span></a>";
            }).join("");
        }

        function escapeHtml(s) {
            var d = document.createElement("div");
            d.textContent = s == null ? "" : s;
            return d.innerHTML;
        }

        function load() {
            fetch("/Notifications/List", { headers: { "X-Requested-With": "XMLHttpRequest" } })
                .then(function (r) { return r.ok ? r.json() : Promise.reject(); })
                .then(render)
                .catch(function () {});
        }

        bell.addEventListener("click", function () {
            var showing = panel.hidden;
            panel.hidden = !showing;
            if (showing) { load(); }
        });

        document.addEventListener("click", function (e) {
            if (!document.getElementById("notif").contains(e.target)) {
                panel.hidden = true;
            }
        });

        markAll.addEventListener("click", function () {
            fetch("/Notifications/MarkAllRead", {
                method: "POST",
                headers: { "RequestVerificationToken": antiforgeryToken() }
            }).then(function () { load(); }).catch(function () {});
        });

        list.addEventListener("click", function (e) {
            var item = e.target.closest(".notif-item");
            if (item && item.dataset.id) {
                fetch("/Notifications/MarkRead?id=" + item.dataset.id, {
                    method: "POST",
                    headers: { "RequestVerificationToken": antiforgeryToken() }
                }).catch(function () {});
            }
        });

        load();

        if (window.signalR) {
            var connection = new signalR.HubConnectionBuilder()
                .withUrl("/hubs/notifications")
                .withAutomaticReconnect()
                .build();

            connection.on("notify", function (data) {
                setBadge(data.unread);
                if (!panel.hidden) { load(); }
            });

            connection.start().catch(function () {});
        }
    }

    window.tetasReport = function (postId) {
        var reason = window.prompt("Why are you reporting this post? (optional)");
        if (reason === null) { return; }
        var tokenEl = document.querySelector('input[name="__RequestVerificationToken"]');
        var token = tokenEl ? tokenEl.value : "";
        fetch("/Posts/Report?id=" + postId + "&reason=" + encodeURIComponent(reason), {
            method: "POST",
            headers: { "RequestVerificationToken": token }
        })
            .then(function (r) { return r.ok ? r.json() : Promise.reject(); })
            .then(function () { window.alert("Thanks. This post has been reported to the moderators."); })
            .catch(function () { window.alert("Could not send the report right now."); });
    };

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
