function getOwnerNameCreate() {   

    var ownerPid = $("#productOwner").val();
    var url = "../../ad/api/loggedinuser/" + ownerPid;

    $.get(url)
        .done(function (data) {
            $("#productOwnerName").html("<div class='text-success'>Name: " + data.name + "</div>");
        })
        .fail(function () {
            $("#productOwnerName").html("<div class='text-danger'>PID not found</div>");
        });
}

function getOwnerNameEdit() {

    var ownerPid = $("#productOwner").val();
    var url = "../../../ad/api/loggedinuser/" + ownerPid;

    $.get(url)
        .done(function (data) {
            $("#productOwnerName").html("<div class='text-success'>Name: " + data.name + "</div>");
        })
        .fail(function () {
            $("#productOwnerName").html("<div class='text-danger'>PID not found</div>");
        });
}