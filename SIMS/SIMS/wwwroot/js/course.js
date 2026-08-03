document.addEventListener('DOMContentLoaded', function () {
    // 1. AJAX Lọc Dữ Liệu
    const filterDept = document.getElementById('filter_department');
    const selectSubject = document.getElementById('select_subject');
    const selectFaculty = document.getElementById('select_faculty');
    const selectClass = document.getElementById('select_class');
    const inputCourseName = document.getElementById('auto_course_name');

    if (filterDept) {
        filterDept.addEventListener('change', async function () {
            const deptId = this.value;

            selectSubject.disabled = selectFaculty.disabled = selectClass.disabled = true;
            selectSubject.innerHTML = selectFaculty.innerHTML = selectClass.innerHTML = '<option value="">Loading...</option>';

            try {
                const response = await fetch(`/admin/api/department-data/${deptId}`);
                const data = await response.json();

                selectSubject.innerHTML = '<option value="" disabled selected>-- Select Subject --</option>';
                data.subjects.forEach(s => selectSubject.innerHTML += `<option value="${s.id}">${s.code} - ${s.name}</option>`);

                selectFaculty.innerHTML = '<option value="" disabled selected>-- Select Faculty --</option>';
                // Không hiển thị faculty code nữa
                data.faculties.forEach(f => selectFaculty.innerHTML += `<option value="${f.id}">${f.name}</option>`);

                selectClass.innerHTML = '<option value="" disabled selected>-- Select Class to auto-enroll --</option>';
                data.classes.forEach(c => selectClass.innerHTML += `<option value="${c.id}">${c.name}</option>`);

                selectSubject.disabled = selectFaculty.disabled = selectClass.disabled = false;
            } catch (err) {
                console.error("Error fetching data", err);
                alert("Failed to load data.");
            }
        });
    }

    // 2. Auto Generate Course Name
    function updateAutoCourseName() {
        if (selectSubject.value && selectClass.value) {
            const subjectText = selectSubject.options[selectSubject.selectedIndex].text.split(' - ')[0];
            const classText = selectClass.options[selectClass.selectedIndex].text;
            inputCourseName.value = `${subjectText} - ${classText}`;
        }
    }

    if (selectSubject && selectClass) {
        selectSubject.addEventListener('change', updateAutoCourseName);
        selectClass.addEventListener('change', updateAutoCourseName);
    }

    // 3. Edit & Delete Modal
    const rows = document.querySelectorAll(".clickable-row");
    rows.forEach(row => {
        row.addEventListener("click", () => {
            const data = row.dataset;
            const modal = document.getElementById(data.target);
            if (!modal) return;

            if (data.target === "modal-edit-course") {
                modal.querySelector('#course_id').value = data.id || '';
                modal.querySelector('#course_name').value = data.name || '';
                modal.querySelector('#course_subjectid').value = data.subjectid || '';
                modal.querySelector('#course_facultyid').value = data.facultyid || '';

                const deleteCourseInput = document.getElementById('delete_course_id');
                if (deleteCourseInput) deleteCourseInput.value = data.id;
            }
        });
    });

    document.querySelectorAll('.btn-delete').forEach(btn => {
        btn.addEventListener('click', function () {
            if (this.id === 'delete-course-btn') {
                const courseId = document.getElementById('delete_course_id').value;
                if (confirm(`Are you sure you want to delete Course #${courseId}? All students will be unenrolled. This cannot be undone.`)) {
                    document.getElementById('form-delete-course').submit();
                }
            }
        });
    });
});