// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


$('body').tooltip({
    selector: '[data-tooltip="tooltip"]',
    container: 'body',
    trigger: 'hover'
}); //tooltip

$(function () {
    var tables = $("#example1").DataTable({
        "responsive": true, "lengthChange": true, "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
        "ajax": {
            url: "https://localhost:7059/api/Department",
            type: "GET",
            "datatype": "json",
            "dataSrc": "data",
            //debugger;
            // success: function(result){
            //     console.log(result.data)
            // }
        },order:[[2,'asc']],
        "columns": [
            { "data": null, "orderable": false },
            { "data": null },
            { "data": "dept_id" },
            { "data": "name_Dept" }
        ],
        columnDefs: [
            {
                targets: [1], orderable: false, searchable: false
            },
            {
                targets: [0], render: function (data) {
                    return `<div>
                    <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#modal-default" data-tooltip="tooltip" data-placement="left" title="Edit department" 
                    onclick="setEditDepartmentModal('${data.dept_id}', '${data.name_Dept}')">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button type="button" class="btn btn-danger" data-tooltip="tooltip" data-placement="right" title="Edit department"
                    onclick="deleteDepartment('${data.dept_id}')">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>`;
                }
            },
        ]
    })

    tables.on('draw.dt', function () {
        var PageInfo = $('#example1').DataTable().page.info();
        tables.column(1, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });

    tables.buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
});

function cleandata() {
    $('#addDepartmentName').val('');
    $('#addDepartment').addClass("d-none"); // menampilkan form untuk menambahkan data
    $('#editDepartment').removeClass("d-none"); // menyembunyikan form edit
}

function save(e) {
    e.preventDefault()
    var departement = new Object()
    departement.dept_id = ''
    departement.name_Dept = $('#addDepartmentName').val()
    if (departement.name_Dept == '') {
        Swal.fire(
            'Gagal',
            'Nama department tidak boleh kosong',
            'error'
        )
    } else {
    $.ajax({
        type: 'POST',
        url: "https://localhost:7059/api/Department",
        data: JSON.stringify(departement),
        contentType: "application/json; charset=utf-8"
    }).then((result) => {
        $('#example1').DataTable().ajax.reload();
        $('#modal-default').modal('hide');
        Swal.fire(
                'Berhasil',
                result.message,
                'success'
        )
        cleandata();
    }).catch((error) => {
        Swal.fire(
            'Berhasil',
            error.JSONresponse.message,
            'error' )})
    }
}

function update(e) {
    e.preventDefault()
    var departement = new Object()
    departement.dept_id = $('#editDepartmentId').val()
    departement.name_Dept = $('#editDepartmentName').val()
    if (departement.name_Dept == '') {
        Swal.fire(
            'Gagal',
            'Nama department tidak boleh kosong',
            'error'
        )
    } else {
        $.ajax({
            type: 'PUT',
            url: "https://localhost:7059/api/Department",
            data: JSON.stringify(departement),
            contentType: "application/json; charset=utf-8"
        }).then((result) => {
            $('#example1').DataTable().ajax.reload();
            $('#modal-default').modal('hide');
            Swal.fire(
                'Berhasil',
                result.message,
                'success'
            )
        }).catch((error) => {
            Swal.fire(
                'Berhasil',
                error.JSONresponse.message,
                'error')
        })
    }
}

function deleteDepartment(dept_id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
        $.ajax({
            type: 'DELETE',
            url: `https://localhost:7059/api/Department?Dept_id=${dept_id}`,
            contentType: "application/json; charset=utf-8"
        }).then((result) => {
            $('#example1').DataTable().ajax.reload();
            $('#modal-default').modal('hide');
            Swal.fire(
                'Berhasil',
                result.message,
                'success'
            )
        }).catch((error) => {
            Swal.fire(
                'Berhasil',
                error.JSONresponse.message,
                'error')
        })
        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire(
                'Cancelled',
            )
        }
    })
}

const setAddDepartmentModal = () => {
    $('#modalLabel').text("Add new department") // ganti modal title
    $('#addDepartment').addClass("d-block") // menampilkan form untuk menambahkan data
    $('#editDepartment').removeClass("d-block") // menyembunyikan form edit
    return false;
}

const setEditDepartmentModal = (id, name) => {
    $('#modalLabel').text("Edit department") // ganti modal title
    $('#editDepartment').addClass("d-block") // menampilkan form untuk mengedit data
    $('#addDepartment').removeClass("d-block") // menyembunyikan form tambah

    $('#editDepartmentId').val(id)
    $('#editDepartmentName').val(name) // menampilkan data sebelumnya
    return false;
}