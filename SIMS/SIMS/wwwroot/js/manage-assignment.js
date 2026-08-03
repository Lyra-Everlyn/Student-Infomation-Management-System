document.addEventListener('DOMContentLoaded', function () {

    const rows = document.querySelectorAll(".clickable-row");
    rows.forEach(row => {
        row.addEventListener("click", function (e) {
            if (e.target.closest('.btn-create')) return;

            const data = this.dataset;
            const modal = document.getElementById(data.target);
            if (!modal) return;

            if (data.target === "modal-edit-assignment") {
                modal.querySelector('#edit_assignment_id').value = data.id || '';
                modal.querySelector('#edit_assignment_name').value = data.name || '';
                modal.querySelector('#edit_assignment_description').value = data.description || '';
                modal.querySelector('#edit_assignment_start').value = data.start || '';
                modal.querySelector('#edit_assignment_end').value = data.end || '';

                const deleteAssignInput = document.getElementById('delete_assignment_id');
                if (deleteAssignInput) deleteAssignInput.value = data.id;
            }
            modal.style.display = "flex";
        });
    });

    document.querySelectorAll('.btn-create').forEach(btn => {
        btn.addEventListener('click', function () {
            const modalId = this.dataset.modalTarget;
            if (modalId) {
                const modal = document.querySelector(modalId);
                if (modal) modal.style.display = 'flex';
            }
        });
    });

    const deleteBtn = document.getElementById('delete-assignment-btn');
    if (deleteBtn) {
        deleteBtn.addEventListener('click', function () {
            if (confirm('Are you sure you want to delete this assignment? All submitted grades will also be removed.')) {
                document.getElementById('form-delete-assignment').submit();
            }
        });
    }

    document.querySelectorAll('.close-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            btn.closest('.modal').style.display = 'none';
        });
    });

    // window.addEventListener('click', e => {
    //     if (e.target.classList.contains('modal')) e.target.style.display = 'none';
    // });
});

document.addEventListener('DOMContentLoaded', function () {
    const filterSelect = document.getElementById('assignment-filter');

    if (filterSelect) {
        filterSelect.addEventListener('change', function () {
            const selectedAssignmentId = this.value;
            const rows = document.querySelectorAll('.submission-row');

            rows.forEach(row => {
                const assignmentId = row.getAttribute('data-assignment-id');

                if (selectedAssignmentId === 'all' || assignmentId === selectedAssignmentId) {
                    row.style.display = '';
                } else {
                    row.style.display = 'none';
                }
            });
        });
    }
});