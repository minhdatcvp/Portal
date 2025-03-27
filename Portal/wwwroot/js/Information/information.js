$(document).ready(function () {
    $.noConflict();
    var table = $('#dataTable').DataTable({
        processing: true,
        serverSide: true,
        ajax: function (data, callback) {
            var orders = data.order.map(o => ({
                sortBy: data.columns[o.column].data,
                sortOrder: o.dir 
            }));
            var pageSize = data.length;
            var page = (data.start / pageSize) + 1;
            var selectedDate = $('#dateFilter').val();

            $.ajax({
                url: `${BASE_URL}/MarketData`,
                type: "GET",
                data: {
                    page: page,
                    pageSize: pageSize,
                    sortBy: orders[0].sortBy,
                    sortOrder: orders[0].sortOrder,
                    search: selectedDate 
                },
                success: function (response) {
                    callback({
                        recordsTotal: response.totalRecords,
                        recordsFiltered: response.totalRecords,
                        data: response.data
                    });
                }
            });
        },
        columns: [
            { data: "date", title: "Date" },
            { data: "price", title: "Market Price" }
        ],
        order: [[0, "desc"]],
        paging: true,
        searching: false, // Ẩn ô tìm kiếm mặc định
        ordering: true,
        pageLength: 10,
        lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
        dom: "<'top'<'date-filter-container'>>rt<'bottom'lip>", // Thêm container cho date input
        language: {
            paginate: {
                previous: "<",
                next: ">"
            },
            lengthMenu: "Hiển thị _MENU_ mục",
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ mục"
        }
    });

    $('.date-filter-container').html(`
        <label for="dateFilter" style="margin-right: 10px; font-weight: 500;">Search by Date:</label>
        <input type="date" id="dateFilter" class="form-control" style="display: inline-block; width: auto;">
    `);

    $('#dateFilter').on('change', function () {
        table.ajax.reload();
    });
});
