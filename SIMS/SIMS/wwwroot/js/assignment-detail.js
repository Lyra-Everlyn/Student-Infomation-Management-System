document.addEventListener('DOMContentLoaded', function () {
    const btnToggleUpload = document.getElementById('btn-toggle-upload');
    const btnCancelUpload = document.getElementById('btn-cancel-upload');
    const uploadSection = document.getElementById('upload-section');

    // Add/Edit -> Show Form
    if (btnToggleUpload && uploadSection) {
        btnToggleUpload.addEventListener('click', function () {
            uploadSection.style.display = 'block';
            btnToggleUpload.style.display = 'none';
        });
    }

    // Cancel -> Hide Form
    if (btnCancelUpload && uploadSection) {
        btnCancelUpload.addEventListener('click', function () {
            uploadSection.style.display = 'none';
            if (btnToggleUpload) {
                btnToggleUpload.style.display = 'inline-block';
            }
        });
    }
});