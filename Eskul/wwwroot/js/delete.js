
function del(id) {
    $('#id').val(id);
    $('#modal-warning').modal('show');
}

var form = document.getElementById('confirm');
document.getElementById("confirm").addEventListener('submit', function (e) {
    e.preventDefault();
    console.log($('#id').val())
    var baseUrl = window.location.protocol + "//" + window.location.host;
    var pathName = window.location.pathname;
    if (pathName.charAt(0) === '/') {
        pathName = pathName.slice(1);
    }
    var fullUrl = baseUrl + '/' + pathName;
    var controller = $('#confirm').data('controller');
    console.log('Controller Is', controller);
    fullUrl = fullUrl.replace('/undefined', '');
    console.log(fullUrl);
    var url = fullUrl + '/' +controller + '/Delete';
    var CUrl = url.replace('/undefined', '').replace('Details', '').replace('//Delete', '/Delete'). replace(/\/ScoreRank\/.*\/Delete/, '/ScoreRank/Delete').replace('/Index','');
    console.log(CUrl);

    var id = $('#id').val();
    console.log(id);

    $.ajax({
        url: CUrl,
        method: 'GET',
        data: {
            id: id
        },
        beforeSend: function (xhr) {
            $('#loader').removeClass('d-none');
            $('#confirm').addClass('d-none');
        },
        success: function (res) {
            console.log(res);
            $('#modal-info').modal('hide');
            $('#confirm').removeClass('d-none');
            $('#loader').addClass('d-none');
            if (res.status == 200) {
                toastr.success(res.res);
                setTimeout(function () {
                    location.reload();
                }, 3000);
            } else {
                console.log(res.res)
                toastr.error(res.res);
                setTimeout(function () {
                    location.reload();
                }, 3000);
            }
        }
    });
});
