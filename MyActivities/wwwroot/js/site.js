// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function editCell(id) {
    var span = $("#span_" + id);
    var input = $("#input_" + id);
    var button = $("#btn_" + id);
    var cancel = $("#cancel_" + id);
    var err = $("#error_" + id);
    var deleteButtons = $(".btnDelete");

    if (span.is(":visible")) {
        span.hide();
        input.val(span.html());
        input.show();
        cancel.show();
        deleteButtons.hide();
        button.html('Save');
    }
    else {
        if (input.val() != "") {
            $.post({
                url: "/api/activity/post",
                data: JSON.stringify({
                    "Id": id,
                    "Name": input.val(),
                    "IsDeleted": false
                }),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //span.html(input.val());
                    //span.show();
                    //input.hide();
                    //cancel.hide();
                    //button.html('Edit');
                    document.location.reload();
                },
                error: function (xhr, status, error) {
                    var errMsg = xhr.responseText;
                    if (errMsg.indexOf("IX_Activities") > 0)
                        err.html("Activity already exists");
                    else
                        err.html("Error saving activity");
                }
            });
        }
    }
}

function cancelEdit(id) {
    var span = $("#span_" + id);
    var input = $("#input_" + id);
    var button = $("#btn_" + id);
    var cancel = $("#cancel_" + id); span.show();
    var del = $("#del_" + id);
    var tr = $("#tr_" + id); span.show();
    var deleteButtons = $(".btnDelete");
    var err = $("#error_" + id);

    if (tr.attr("isnew") == "true") {
        tr.remove();
        del.hide();
    }
    else {
        input.hide();
        cancel.hide();
        del.show();
        button.html('Edit');
        deleteButtons.show();
        err.html('');
    }

}

function addNew() {
    var guid = uuidv4();
    var str = ` <tr id="tr_` + guid + `" isnew="true">
        <td style="width: 300px;">
                <span id="span_` + guid + `"></span>
                <input type="text" id="input_` + guid + `" value="" style="display:none" />
        </td>
        <td>
                <button id="btn_` + guid + `" class="btn btn-primary" onclick="editCell('` + guid + `')">Edit</button>
                <button id="cancel_` + guid + `" class="btn btn-secondary" onclick="cancelEdit('` + guid + `')" style="display:none">Cancel</button>
        </td>
        <td>
            <p id="error_` + guid + `" class="error"></p>
        </td>
    </tr>`;
    $(".list").append(str);
    editCell(guid);
}

function deleteRow(id, name) {
    if (confirm("Are you sure you want to remove activity '" + name + "'?")) {
        $.ajax({
            url: "/api/activity/delete/" + id,
            type: 'DELETE',
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                //span.html(input.val());
                //span.show();
                //input.hide();
                //cancel.hide();
                //button.html('Edit');
                document.location.reload();
            },
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            }
        });
    }
}

function uuidv4() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (+c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> +c / 4).toString(16)
    );
}

function dateToString(day) {
    return day.toString('yyyy-MM-dd');
}
function checkDayActivity(activityId, day) {
    let objDate = new Date(day);
    let strDate = objDate.toISOString().split('T')[0];
    alert(str);
    $.post({
        url: "/api/dayactivity/post/" + activityId + '/' + strDate,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            return true;
        },
        error: function (xhr, status, error) {
            return false;
        }
    });
}