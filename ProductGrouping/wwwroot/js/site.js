function getOwnerName(page) {

    var ownerPid = $("#productOwner").val();
    var url;

    if (ownerPid === "") {
        return;
    }       

    if (page === "Create") {
        url = "../../ad/api/loggedinuser/" + ownerPid;
    } else if(page === "Edit"){
        url = "../../../ad/api/loggedinuser/" + ownerPid;
    }    

    $.get(url)
        .done(function (data) {
            $("#productOwnerName").html("<div class='text-success'>Name: " + data.name + "</div>");
        })
        .fail(function () {
            $("#productOwnerName").html("<div class='text-danger'>PID not found</div>");
        });
}

function getOwnerData(ownerPid) {

    var url = "../../ad/api/loggedinuser/" + ownerPid;

    return $.get(url);
 }

function showUserData(ownerDetails) {

    if (ownerDetails === undefined) {
        $("#modalPid").val("PID not found");
        $("#modalName").val("Name not found");
        $("#modalPhone").val("Phone not found");
        $("#modalEmail").val("Email not found");
    } else {
        $("#modalPid").val(ownerDetails.pid);
        $("#modalName").val(ownerDetails.name);
        $("#modalPhone").val(ownerDetails.phoneNumber);
        $("#modalEmail").val(ownerDetails.email);
    }
}

$('#OwnerModal').on('show.bs.modal', function (event) {

    $("#modalPid").val("");
    $("#modalName").val("");
    $("#modalPhone").val("");
    $("#modalEmail").val("");

    var button = $(event.relatedTarget);
    var owner = button.data('whatever');

    getOwnerData(owner)
        .done(function (data) {
            showUserData(data);
        })
        .fail(function () {
            showUserData();
        });
});

