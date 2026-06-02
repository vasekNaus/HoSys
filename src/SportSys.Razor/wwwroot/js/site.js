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
