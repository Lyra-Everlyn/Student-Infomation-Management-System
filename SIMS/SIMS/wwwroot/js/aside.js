document.addEventListener('DOMContentLoaded', function () {
    // Tab switching logic
    const tabContents = document.querySelectorAll('.tab-content');
    const modals = document.querySelectorAll('.modal');
    const navLinks = document.querySelectorAll('.nav-link[data-tab]');

    const activeTab = sessionStorage.getItem('activeAdminTab') || 'tab-home';

    navLinks.forEach(n => n.classList.remove('active'));
    tabContents.forEach(t => {
        t.style.display = 'none';
        t.classList.remove('active');
    });

    const savedLink = document.querySelector(`.nav-link[data-tab="${activeTab}"]`);
    const savedTab = document.getElementById(activeTab);

    if (savedLink && savedTab) {
        savedLink.classList.add('active');
        savedTab.style.display = 'block';
        savedTab.classList.add('active');
    }

    navLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            if (this.dataset.tab) {
                e.preventDefault();
                modals.forEach(modal => modal.style.display = 'none');

                const targetTabId = this.dataset.tab.trim();
                sessionStorage.setItem('activeAdminTab', targetTabId);

                navLinks.forEach(n => n.classList.remove('active'));
                this.classList.add('active');

                tabContents.forEach(t => {
                    t.style.display = 'none';
                    t.classList.remove('active');
                });

                const targetTab = document.getElementById(targetTabId);
                if (targetTab) {
                    targetTab.style.display = 'block';
                    targetTab.classList.add('active');
                }
            }
        });
    });
});