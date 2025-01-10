// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function editCell(id) {
    var span = $("#span_" + id);
    var spanWeight = $("#span_weight_" + id);
    var input = $("#input_" + id);
    var inputWeight = $("#input_weight_" + id);
    var button = $("#btn_" + id);
    var cancel = $("#cancel_" + id);
    var err = $("#error_" + id);
    var deleteButtons = $(".btnDelete");

    if (span.is(":visible")) {
        span.hide();
        input.val(span.html());
        input.show();
        input.focus();
        if (spanWeight.length > 0) {
            spanWeight.hide();
            inputWeight.show();
        }
        var strLength = input.val().length * 2;
        input[0].setSelectionRange(strLength, strLength);
        cancel.show();
        deleteButtons.hide();
        button.html('Save');
    }
    else {
        if (input.val() != "") {
            var url = "/api/activity/post";
            var data = {
                "Id": id,
                "Name": input.val(),
                "IsDeleted": false
            };

            if (spanWeight.length == 0) {
                url = "/api/activity/post";
                data = {
                    "Id": id,
                    "Name": input.val(),
                    "IsDeleted": false
                };
            }
            else {
                url = "/api/gymactivity/post";
                data = {
                    "Id": id,
                    "Name": input.val(),
                    "Setting": inputWeight.val(),
                    "IsDeleted": false
                };
            }
            $.post({
                url: url,
                data: JSON.stringify(data),
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
    var spanWeight = $("#span_weight_" + id);
    var input = $("#input_" + id);
    var inputWeight = $("#input_weight_" + id);
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
        if (inputWeight.length > 0) {
            spanWeight.show();
            inputWeight.hide();
        }
        del.show();
        button.html('Edit');
        deleteButtons.show();
        err.html('');
    }

}

function addNew(isGymActivity) {
    var guid = uuidv4();
    var str = ` <tr id="tr_` + guid + `" isnew="true">
        <td style="width: 300px;">
                <span id="span_` + guid + `"></span>
                <input type="text" id="input_` + guid + `" value="" style="display:none" onkeydown="onInputKeyDown(event, '` + guid + `');" />
        </td>`;
    if (isGymActivity) {
        str += ` <td class="weight">
                    <span id="span_weight_` + guid + `"></span>
                    <input type="text" id="input_weight_` + guid + `" value="" style="display:none" class="weight" onkeydown="onInputKeyDown(event, '` + guid + `')" />
                </td>`
    }
        str += `
        <td>
                <button id="btn_` + guid + `" class="btn btn-primary edit-save" onclick="editCell('` + guid + `')">Edit</button>
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
    var inputWeight = $("#input_weight_" + id);
    var url;
    if (inputWeight.length = 0) {
        url = "/api/activity/delete";
    }
    else {
        url = "/api/gymactivity/delete";
    }

    if (confirm("Are you sure you want to remove activity '" + name + "'?")) {
        $.ajax({
            url: url + "/" + id,
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

function checkDayGymActivity(gymActivityId, day) {
    let objDate = new Date(day);
    let strDate = objDate.toISOString().split('T')[0];

    $.post({
        url: "/api/daygymactivity/post/" + gymActivityId + '/' + strDate,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            return true;
        },
        error: function (xhr, status, error) {
            return false;
        }
    });
}

function onInputKeyDown(e, activityId) {
    if (e.keyCode == 13) {
        var btn = $("#btn_" + activityId);
        btn.click();
    }
    else if (e.keyCode == 27 && $("#input_" + activityId).is(":visible")) {
        cancelEdit(activityId);
    }
}

function onActivityChange() {
    var selectObj = $("#selActivity");
    $.ajax({
        url: "/api/gymactivity/gethistory/" + selectObj.val(),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.length) {
                var str = '<table>';
                for (var i = 0; i < result.length; i++) {
                    str += '<tr><td>' + moment(result[i].dateTime).format('MMM DD YYYY, h:mm a') + '</td><td class="setting">' + result[i].newSetting + '</td></tr>'
                }
                str + '</table>'
            }
            else
                str = '';
            $('#divResult').html(str);
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}