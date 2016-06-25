/// <reference path="../lib/jquery/dist/jquery.js" />
/// <reference path="../lib/knockout/dist/knockout.js" />

var UserApp = function () {
    var self = this;

    self.user = ko.observable(new User());

    self.saveUser = function () {
        $.ajax({
            method: "POST",
            contentType: "application/json",
            url: "/api/users",
            data: ko.toJSON(self.user),
            success: function (data) {
                resetErrorPane();
                alert("User has been successfully created.");
                self.user(new User);
                userListTable.ajax.reload();
            },
            error: function (xhr) {
                resetErrorPane();
                var error = xhr.responseJSON;
                $.each(error, function (field) {
                    $("#errorList").append("<li>" + error[field] + "</li>");
                });
                $("#errorPane").addClass("in");
            }
        });
    }
}

function resetErrorPane() {
    $("#errorPane").removeClass("in").addClass("out");
    $("#errorList > li").remove();
}

ko.applyBindings(new UserApp());

var userListTable;

$(function () {
    $("ul.nav-tabs a").click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });

    userListTable = $("#userTable").DataTable({
        "ajax": "/api/users/aoData"
    });
});