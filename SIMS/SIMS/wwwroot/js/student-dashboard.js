document.addEventListener('DOMContentLoaded', function () {

    // 1. Open Modal Create/Join
    document.querySelectorAll('.btn-create').forEach(btn => {
        btn.addEventListener('click', function () {
            const modalId = this.dataset.modalTarget;
            if (modalId) {
                const modal = document.querySelector(modalId);
                if (modal) modal.style.display = 'flex';
            }
        });
    });

    // 2. Close Modals
    document.querySelectorAll('.close-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            btn.closest('.modal').style.display = 'none';
        });
    });

    // window.addEventListener('click', e => {
    //     if (e.target.classList.contains('modal')) e.target.style.display = 'none';
    // });

    // 3. Leave Class
    document.querySelectorAll('.btn-leave-class').forEach(btn => {
        btn.addEventListener('click', function () {
            const courseId = this.dataset.id;
            const courseName = this.dataset.name;

            if (!courseId) return;

            if (confirm(`Are you sure you want to leave the class "${courseName}"?\nNote: All your submitted assignments and grades for this class may be permanently unlinked!`)) {
                window.location.href = `/Student/Course/Leave/${courseId}`;
            }
        });
    });
});