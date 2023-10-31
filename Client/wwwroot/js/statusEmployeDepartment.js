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
        "responsive": true, "lengthChange": true, "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
        "ajax": {
            url: "https://localhost:7059/api/Employee/GetActiveEmployee",
            type: "GET",
            "datatype": "json",
            "dataSrc": "data",
            //debugger;
            // success: function(result){
            //     console.log(result.data)
            // }
        }, order: [[2, 'asc']],
        "columns": [
            { "data": null },
            { "data": "nik" },
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
                targets: [7], render: function (data) {
                    let activeBadge = `<span class="badge badge-pill badge-success">Aktif</span>`
                    let resignBadge = `<span class="badge badge-pill badge-danger">Tidak Aktif</span>`

                    return (data.isActive == true ? activeBadge : resignBadge);
                }
            },
        ]
    })

    tables.on('draw.dt', function () {
        var PageInfo = $('#example1').DataTable().page.info();
        tables.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });

    tables.buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
});

const InactiveEmployee = (e) => {
    $('#ActiveEmployee').hide()
    $('#InactiveEmployee').show()
    
}

const ActiveEmployee = (e) => {
    $('#ActiveEmployee').show()
    $('#InactiveEmployee').hide()

}

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

    var tables = $("#example2").DataTable({
        "responsive": true, "lengthChange": true, "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
        "ajax": {
            url: "https://localhost:7059/api/Employee/GetDeactiveEmployee",
            type: "GET",
            "datatype": "json",
            "dataSrc": "data",
            //debugger;
            // success: function(result){
            //     console.log(result.data)
            // }
        }, order: [[2, 'asc']],
        "columns": [
            { "data": null },
            { "data": "nik" },
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
                targets: [7], render: function (data) {
                    let activeBadge = `<span class="badge badge-pill badge-success">Aktif</span>`
                    let resignBadge = `<span class="badge badge-pill badge-danger">Tidak Aktif</span>`

                    return (data.isActive == false ? resignBadge : activeBadge);
                }
            },
        ]
    })

    tables.on('draw.dt', function () {
        var PageInfo = $('#example2').DataTable().page.info();
        tables.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });

    tables.buttons().container().appendTo('#example2_wrapper .col-md-6:eq(0)');
});