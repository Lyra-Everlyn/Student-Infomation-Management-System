function openAddStudentModal() {
    document.getElementById('modal-add-student').style.display = 'flex';
}

function confirmRemoveStudent(courseId, studentId, studentName, formId) {
    if (confirm(`Are you sure you want to remove ${studentName} from this course? This action will also delete their grades for this course.`)) {
        const form = document.getElementById(formId);
        form.querySelector('.remove_course_id').value = courseId;
        form.querySelector('.remove_student_id').value = studentId;
        form.submit();
    }
}

function filterAssignments(assignmentId) {
    const rows = document.querySelectorAll('.submission-row');
    rows.forEach(row => {
        if (assignmentId === 'all' || row.getAttribute('data-assignment-id') === assignmentId) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    });
}