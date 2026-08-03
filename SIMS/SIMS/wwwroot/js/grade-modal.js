document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('modal-grade');
    const closeBtn = document.querySelector('.close-grade-modal');

    document.querySelectorAll('.btn-open-grade-modal').forEach(btn => {
        btn.addEventListener('click', function () {
            const gradeId = this.dataset.gradeid;
            const score = this.dataset.score;
            const feedback = this.dataset.feedback;
            const student = this.dataset.student;
            const assignment = this.dataset.assignment;

            document.getElementById('modal_grade_id').value = gradeId;
            document.getElementById('modal_score').value = score !== "null" ? score : "";
            document.getElementById('modal_feedback').value = feedback !== "null" ? feedback : "";
            document.getElementById('modal_info_text').innerText = `${student} - ${assignment}`;

            modal.style.display = 'flex';
        });
    });

    if (closeBtn) {
        closeBtn.addEventListener('click', () => modal.style.display = 'none');
    }
    window.addEventListener('click', e => {
        if (e.target === modal) modal.style.display = 'none';
    });
});