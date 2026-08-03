document.addEventListener('DOMContentLoaded', function () {
    // 1. Modal edit logic
    const rows = document.querySelectorAll(".clickable-row");
    rows.forEach(row => {
        row.addEventListener("click", () => {
            const data = row.dataset;
            const modal = document.getElementById(data.target);

            if (!modal) return;
            switch (data.target) {
                case "modal-edit-admission":
                    modal.querySelector('#application_id').value = data.id || '';
                    modal.querySelector('#application_userid').value = data.userid || '';
                    modal.querySelector('#application_fullname').value = data.fullname || '';
                    modal.querySelector('#application_email').value = data.email || '';
                    modal.querySelector('#application_phone').value = data.phone || '';
                    modal.querySelector('#application_dob').value = data.dob || '';
                    modal.querySelector('#application_department').value = data.departmentid || '';
                    modal.querySelector('#application_status').value = data.status || '';

                    const deleteInput = document.getElementById('delete_application_id');
                    if (deleteInput) deleteInput.value = data.id;
                    break;

                case "modal-edit-department":
                    modal.querySelector('#department_id').value = data.id || '';
                    modal.querySelector('#department_code').value = data.code || '';
                    modal.querySelector('#department_name').value = data.name || '';

                    const deleteDeptInput = document.getElementById('delete_department_id');
                    if (deleteDeptInput) deleteDeptInput.value = data.id;
                    break;

                case "modal-edit-class":
                    modal.querySelector('#class_id').value = data.id || '';
                    modal.querySelector('#class_name').value = data.name || '';
                    modal.querySelector('#class_department').value = data.departmentid || '';

                    const deleteClassInput = document.getElementById('delete_class_id');
                    if (deleteClassInput) deleteClassInput.value = data.id;
                    break;

                case "modal-edit-subject":
                    modal.querySelector('#subject_id').value = data.id || '';
                    modal.querySelector('#subject_name').value = data.name || '';
                    modal.querySelector('#subject_code').value = data.code || '';
                    modal.querySelector('#subject_credits').value = data.credits || '';
                    modal.querySelector('#subject_department').value = data.departmentid || '';

                    const deleteSubjectInput = document.getElementById('delete_subject_id');
                    if (deleteSubjectInput) deleteSubjectInput.value = data.id;
                    break;

                case "modal-edit-user":
                    modal.querySelector('#user_id').value = data.id || '';
                    modal.querySelector('#user_fullname').value = data.fullname || '';
                    modal.querySelector('#user_email').value = data.email || '';
                    modal.querySelector('#user_role').value = data.role || 'Applicant';
                    modal.querySelector('#user_phone').value = data.phone || '';
                    modal.querySelector('#user_address').value = data.address || '';
                    modal.querySelector('#user_dob').value = data.dob || '';
                    modal.querySelector('#user_status').value = data.status || 'Active';
                    modal.querySelector('#user_created_at').value = data.createdat || '';
                    modal.querySelector('#user_updated_at').value = data.updatedat || '';

                    const deleteUserInput = document.getElementById('delete_user_id');
                    if (deleteUserInput) deleteUserInput.value = data.id;
                    break;

                case "modal-edit-student":
                    modal.querySelector('#student_id').value = data.id || '';
                    modal.querySelector('#student_fullname').value = data.name || '';
                    modal.querySelector('#student_email').value = data.email || '';
                    modal.querySelector('#student_code').value = data.code || '';
                    modal.querySelector('#student_department').value = data.departmentid || '';
                    modal.querySelector('#student_class').value = data.classid || '';
                    break;

                case "modal-edit-faculty":
                    modal.querySelector('#faculty_id').value = data.id || '';
                    modal.querySelector('#faculty_fullname').value = data.fullname || '';
                    modal.querySelector('#faculty_email').value = data.email || '';
                    modal.querySelector('#faculty_department').value = data.departmentid || '';
                    break;

            }
            modal.style.display = "flex";
        });
    });

    // 2. Delete item
    document.querySelectorAll('.btn-delete').forEach(btn => {
        btn.addEventListener('click', function () {
            if (this.id === 'delete-admission-btn') {
                const appId = document.getElementById('delete_application_id').value;
                if (confirm(`Are you sure you want to delete Admission Application #${appId}? This action cannot be undone.`)) {
                    document.getElementById('form-delete-admission').submit();
                }
            }
            if (this.id === 'delete-department-btn') {
                const deptId = document.getElementById('delete_department_id').value;
                if (confirm(`Are you sure you want to delete Department #${deptId}? This action cannot be undone.`)) {
                    document.getElementById('form-delete-department').submit();
                }
            }
            if (this.id === 'delete-class-btn') {
                const classId = document.getElementById('delete_class_id').value;
                if (confirm(`Are you sure you want to delete Class #${classId}? This action cannot be undone.`)) {
                    document.getElementById('form-delete-class').submit();
                }
            }
            if (this.id === 'delete-subject-btn') {
                const subjectId = document.getElementById('delete_subject_id').value;
                if (confirm(`Are you sure you want to delete Subject #${subjectId}? This action cannot be undone.`)) {
                    document.getElementById('form-delete-subject').submit();
                }
            }
            if (this.id === 'delete-user-btn') {
                const userId = document.getElementById('delete_user_id').value;
                if (confirm(`Are you sure you want to delete User #${userId}? This action cannot be undone.`)) {
                    document.getElementById('form-delete-user').submit();
                }
            }

        });
    });

    // 3. Create modals
    document.querySelectorAll('.btn-create').forEach(btn => {
        btn.addEventListener('click', function () {
            const modalId = this.dataset.modalTarget;
            const modal = document.querySelector(modalId);
            if (modal) modal.style.display = 'flex';
        });
    });

    // 4. Close modals
    document.querySelectorAll('.close-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            btn.closest('.modal').style.display = 'none';
        });
    });
    // window.addEventListener('click', e => {
    //     if (e.target.classList.contains('modal')) e.target.style.display = 'none';
    // });
});
