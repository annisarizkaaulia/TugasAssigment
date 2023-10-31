// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


$('body').tooltip({
    selector: '[data-tooltip="tooltip"]',
    container: 'body',
    trigger: 'hover'
}); //tooltip

$(function () {
    var departmentSelect = document.getElementById('name_dept');
    $.ajax({
        type: 'GET',
        url: "https://localhost:7059/api/Department",
        dataType: 'json',
        dataSrc: 'data',
    }).then(function (result) {
        // create the option and append to Select2
        for (let i = 0; i < result.data.length; i++) {
            var option = new Option(result.data[i].name_Dept, result.data[i].dept_id, true, false);
            departmentSelect.add(option);
        }
    });

    var tables = $("#example1").DataTable({
        "responsive": true,
        "lengthChange": true,
        "lengthMenu": [[5, 10, 20, 50, 100, -1],[5, 10, 20, 50, 100,'All']],
        "autoWidth": false,
        "paging": true,
        "processing": true,
        "serverSide": true,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
        "ajax": {
            url: "https://localhost:7059/api/Employee/Paging",
            type: "POST",
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
            { "data": "nik", "name":"nik" },
            { "data": "firstName" },
            { "data": "lastName" },
            { "data": "phoneNumber" },
            { "data": "firstName" },
            { "data": "email" },
            { "data": null },
            { "data": "department.dept_ID" },
            { "data": "department.name" }
        ],
        columnDefs: [
            {
                targets: [1], orderable: false, searchable: false
            },
            {
                targets: [8], render: function (data) {
                    let activeBadge = `<span class="badge badge-pill badge-success">Aktif</span>`
                    let resignBadge = `<span class="badge badge-pill badge-danger">Tidak Aktif</span>`

                    return (data.isActive == true ? activeBadge : resignBadge);
                }
            },
            {
                targets: [0], render: function (data) {
                    let datastring = encodeURIComponent(JSON.stringify(data))

                    return `<div>
                    <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#modal-default" data-tooltip="tooltip" data-placement="left" title="Edit department" 
                    onclick="setEditEmployeetModal('${datastring}')">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button type="button" class="btn btn-danger" data-tooltip="tooltip" data-placement="right" title="Edit department"
                    onclick="deleteEmployee('${data.nik}')">
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

const setAddEmployeeModal = (e) => {
    $('#nikhide').hide()
    $('#emailhide').hide()
    $('#modalLabel').text("Add new employee") // ganti modal title
    $('#addButton').show()
    $('#editButton').hide()

    $('#firstName').val('');
    $('#lastname').val('');
    $('#phone').val('');
    $('#address').val('');
    $('#isactive').val('');
    $('#name_dept').val('');
    return false;
}

function addEmployee(e) {
    e.preventDefault()
    var employee = new Object()
    employee.nik = $('#nik').val()
    employee.firstName = $('#firstName').val()
    employee.lastName = $('#lastname').val()
    employee.phone = $('#phone').val()
    employee.email = $('#email').val()
    employee.address = $('#address').val()
    employee.isActive = $('#isactive').val() == 'true' ? true : false
    employee.Department_id = $('#name_dept').val()

    $.ajax({
        type: 'POST',
        url: "https://localhost:7059/api/Employee",
        data: JSON.stringify(employee),
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
            'error' )})
}

const setEditEmployeetModal = (data) => {

    $('#modalLabel').text("Edit employee") // ganti modal title
    $('#addButton').hide()
    $('#editButton').show()
    $('#nikhide').show();
    $('#emailhide').show()
    var object = JSON.parse(decodeURIComponent(data))
    console.log(object)
    $('#nik').val(object.nik)
    $('#firstName').val(object.firstName)
    $('#lastname').val(object.lastName)
    $('#phone').val(object.phoneNumber)
    $('#email').val(object.email)
    $('#address').val(object.address)
    $('#isactive').val(object.isActive)
    $('#name_dept').val(object.department.name_Dept)
    var selectIsActive = `
    <option id="isactive" value="${object.isActive}" selected>${object.isActive ? 'Aktif' : 'Tidak Aktif'}</option>`;
    $(selectIsActive).appendTo('#isactive');

    var selectDepartment = `
    <option id="name_dept" value="${object.department.dept_ID
        }" selected>${object.department.name}</option>`;
    $(selectDepartment).appendTo('#name_dept');
    // menampilkan data sebelumnya
    return false;
}
function editEmployee(e) {
    e.preventDefault()
    var employee = new Object()
    employee.nik = $('#nik').val()
    employee.firstName = $('#firstName').val()
    employee.lastName = $('#lastname').val()
    employee.phone = $('#phone').val()
    employee.email = $('#email').val()
    employee.address = $('#address').val()
    employee.isActive = $('#isactive').val() == 'true' ? true : false
    employee.Department_id = $('#name_dept').val()
    $.ajax({
        type: 'PUT',
        url: `https://localhost:7059/api/Employee?NIK=${employee.nik}`,
        data: JSON.stringify(employee),
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

function deleteEmployee(nik) {
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
            url: `https://localhost:7059/api/Employee?NIK=${nik}`,
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
$(function () {
    $.validator.setDefaults({
        submitHandler: function () {
            alert("Form successful submitted!");
        }
    });
    $('#addEmployee').validate({
        rules: {
            firstName: {
                required: true,
                firstName: true,
            },
            lastname: {
                required: true,
                lastname: true,
            },
            phone: {
                required: true,
                phone: true,
            },
            email: {
                required: true,
                email: true,
            },
            address: {
                required: true,
                address: true,
            },
        },
        messages: {
            firstName: {
                required: "Please enter a firstname",
                firstName: "Please enter a valid firstname"
            },
            lastname: {
                required: "Please enter a lastname",
                lastname: "Please enter a valid lastname"
            },
            phone: {
                required: "Please enter a phone",
                phone: "Please enter a valid phone"
            },
            email: {
                required: "Please enter a email",
                email: "Please enter a valid email"
            },
            address: {
                required: "Please enter a address",
                address: "Please enter a valid address"
            }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});