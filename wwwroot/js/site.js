$(document).ready(function () {
    console.log("JavaScript und jQuery geladen.");

    $('#adminTable').DataTable({
        pagingType: "full_numbers",
        language: {
            search: "Suche:",
            lengthMenu: "Zeige _MENU_ Einträge",
            info: "Zeige _START_ bis _END_ von _TOTAL_ Einträgen",
            paginate: {
                first: "Erste",
                last: "Letzte",
                next: "Nächste",
                previous: "Vorherige"
            }
        },
        responsive: true,
        paging: true,
        lengthChange: true,
        searching: true,
        ordering: true,
        info: true,
        autoWidth: false
    });

    const deleteJobPostingUrl = $('#deleteJobPostingConfig').data('url');

    // Funktion global verfügbar 
    window.deleteJobPosting = function (jobPostingId) {
        
        Swal.fire({
            title: "Wollen Sie diesen Eintrag wirklich löschen?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Ja, löschen",
            cancelButtonText: "Abbrechen",
            confirmButtonColor: "#dc3545",
            cancelButtonColor: "#6c757d"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: deleteJobPostingUrl,
                    data: { id: jobPostingId },
                    success: function () {
                        Swal.fire("Eintrag gelöscht", "", "success").then(() => {
                            location.reload();
                        });
                    },
                    error: function () {
                        Swal.fire("Fehler beim Löschen", "Bitte versuchen Sie es erneut.", "error");
                    }
                });
            }
        });
    };
});
