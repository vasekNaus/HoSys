// ── Dark mode toggle ─────────────────────────────────────────────────────────

(function () {
    var btn = document.getElementById('themeToggle');
    if (!btn) return;

    btn.addEventListener('click', function () {
        var current = document.documentElement.getAttribute('data-theme') || 'light';
        var next = current === 'dark' ? 'light' : 'dark';
        document.documentElement.setAttribute('data-theme', next);
        localStorage.setItem('theme', next);
    });
})();

// ── Layout wide toggle ────────────────────────────────────────────────────────

(function () {
    var btn = document.getElementById('layoutToggle');
    if (!btn) return;

    function syncIcon() {
        var icon = btn.querySelector('i');
        if (!icon) return;
        var isWide = document.documentElement.classList.contains('layout-wide');
        icon.className = isWide ? 'fa-solid fa-compress fa-fw' : 'fa-solid fa-expand fa-fw';
    }

    syncIcon();

    btn.addEventListener('click', function () {
        var isWide = document.documentElement.classList.toggle('layout-wide');
        localStorage.setItem('layout', isWide ? 'wide' : 'default');
        syncIcon();
    });
})();

// ── GET → POST helper ─────────────────────────────────────────────────────────
// Používání: <a href="/url" data-convert-to-post="true" data-post-confirm="Opravdu?">Smazat</a>

(function () {
    document.addEventListener('click', function (e) {
        var link = e.target.closest('[data-convert-to-post="true"]');
        if (!link) return;

        var msg = link.dataset.postConfirm;
        if (msg && !window.confirm(msg)) return;

        e.preventDefault();

        var form = document.createElement('form');
        form.method = 'post';
        form.action = link.getAttribute('href');
        form.style.display = 'none';

        // Zkopíruj CSRF token, pokud existuje
        var token = document.querySelector('input[name="__RequestVerificationToken"]');
        if (token) {
            var hidden = document.createElement('input');
            hidden.type = 'hidden';
            hidden.name = '__RequestVerificationToken';
            hidden.value = token.value;
            form.appendChild(hidden);
        }

        document.body.appendChild(form);
        form.submit();
    });
})();
