function myAlert(str) {
    $('#myModal .modal-dialog .modal-content .modal-body').html(str);
    document.getElementById('tempdiv').style.marginTop = (str.length/800)*100+"px";
    $('#myModal').modal('show');
}

function myalert(str) {
    var div = '<div class="Mymark"><input type="button" style=";height:20px;width:100px" onclick="closeAlert()"/><div style="height:auto;width:240px;overflow:scroll;overflow-x:hidden; ">' + str + '</div></div>';
    $('body').append(div)
    $('.Mymark').show();
    // setTimeout(function () {
    //     $('.mark').hide();
    //     $('.mark').remove();
    // }, 2000)
}


function closeAlert() {
    $('.Mymark').hide();
}
// $('body').click(function(){
//     $('.Mymark').hide();
// });