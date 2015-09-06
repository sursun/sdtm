
var BaseLine;
if (!BaseLine) {
    BaseLine = {};
}

BaseLine.Init = function () {

    $('#bldlg').datagrid({
        singleSelect: true,
        columns: [[
            { field: 'Title', title: '标题', width: 120 },
            //{ field: 'Doctor', title: '责任医生', width: 80 },
            //{ field: 'CreateUser', title: '填表人', width: 80 },
            //{ field: 'CreateTime', title: '填表时间', width: 120 },
            {
                field: 'Progress', title: '完成进度', width: 120 ,formatter: function(value, row, index) {

                    var text = '<div class="progress ';
                    var color = 'progress-red';
                    if (value >= 50) {
                        color = 'progress-blue';
                    }
                    if (value >= 100) {
                        color = 'progress-green';
                    }
                    text += color;
                    text += '"><span style="width: ';
                    
                    text += value;
                    text += '%;"><b>';
                    text += value;
                    text += '%</b></span></div>';

                    return text;
                }

            },
            {
                field: 'Caozuo', title: '操作', width: 100, formatter: function (value, row, index) {

                    return '<a href="#" class="btn-small">编辑/完善</a>';
                }
            }
        ]],
        onClickCell: function (index, field, value) {

            if (field != "Caozuo") {
                return false;
            }

            var rows = $(this).datagrid('getRows');
            var row = rows[index];
            if (row == null)
                return false;

            if (row.Url == "Ylzd" || row.Url == "Zlfa") {

                openFrameDialog('/Health/' + row.Url + '?' + row.Params, row.Title);

            }
            else {

                openDialog('/BaseLine/' + row.Url + '?' + row.Params, row.Title);

            }

            return true;
        },
        onLoadSuccess: function (data) {
            $("#createtime").text(data.rows[0].CreateTime);
            $("#doctor-name").text(data.rows[0].Doctor);
            $("#practicedoctor-name").text(data.rows[0].CreateUser);
        }
    });

    $('#editdlg').dialog({
        title: '信息完善',
        height: 600,
        width: 1000,
        closed: true,
        buttons: [{
            text: '保存',
            handler: function () {
                var formOptions = {
                    defaultbefore: false,
                    dataType: 'json',
                    success: function (data, status, xhr, $form) {

                        if (data.success) {
                            $.messager.alert('提示', '保存成功！');
                            $('#editdlg').dialog("close");
                            $('#bldlg').datagrid("reload");
                        } else {
                            $.messager.alert('错误', '保存失败：' + data.data, 'error');
                        }
                    }
                };

                $("Form", $('#editdlg')).submitForm(formOptions);
            }
        }, {
            text: '关闭',
            handler: function () {
                $('#editdlg').dialog("close");
            }
        }]
    });

    $('#frameeditdlg').dialog({
        title: "信息完善",
        height: 600,
        width: 1200,
        closed: true,
        buttons: [{
            text: '关闭',
            handler: function () {
                $('#frameeditdlg').dialog("close");
            }
        }],
        onClose: function() {
            $('#bldlg').datagrid("reload");
        }
    });

    var openDialog = function (url, title) {
        
        //设置标题
        var opts = $('#editdlg').dialog('options');
        opts.title = title;
        $('#editdlg').dialog(opts);

        //更改链接
        $('#editdlg').dialog("refresh", url);
        $('#editdlg').dialog("open");
    };

    var openFrameDialog = function (url, title) {
        
        //设置标题
        var opts = $('#frameeditdlg').dialog('options');
        opts.title = title;
        $('#frameeditdlg').dialog(opts);

        //更改链接
        $("iframe", $('#frameeditdlg')).attr("src", url);

        $('#frameeditdlg').dialog("open");
    };

    //
    //-------------- 修改地区---------------------------------//
    //
    $("#selectArea").live("click", function () {

        var src = this.hash.substr(1);

        $("#selcet_area_dlg_content").attr("src", src);

        var opt = {
            title: '选择地区',
            width: 500,
            height: 400,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var subContents = $("#selcet_area_dlg_content").contents();
                    
                    $("#Patient_Area_Id").val(subContents.find("#commoncode_selected_Id").val());
                    $("#showArea").text(subContents.find("#commoncode_selected_FullName").val());

                    $("#selcet_area_dlg").dialog('close');
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#selcet_area_dlg").dialog('close');
                }
            }]
        };

        $("#selcet_area_dlg").show();
        $("#selcet_area_dlg").dialog(opt);

        return false;

    });

    //修改医生
    $(".change-doctor").live("click", function () {

        var nType = this.hash.substr(1);

        var patientid = $("#PatientId").val();

        $("#selcet_doctor_dlg_content").attr("src", "/Doctor/Select");

        var opt = {
            title: '选择医生',
            width: 500,
            height: 400,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var subContents = $("#selcet_doctor_dlg_content").contents();

                    //先关闭窗口
                    $("#selcet_doctor_dlg").dialog('close');

                    var docId = subContents.find("#doctor_selected_Id").val();
                    var docName = subContents.find("#doctor_selected_Name").val();

                    var dat = {
                        patientid: patientid,
                        doctorid: docId
                    };

                    if (nType == 2) {
                        dat = {
                            patientid: patientid,
                            pdoctorid: docId
                        };
                    }

                    $.post("/BaseLine/UpdateDoctor", dat, function (data) {
                        if (data.success) {
                            if (nType == 1) {
                                $("#doctor-name").text(docName);
                            } else {
                                $("#practicedoctor-name").text(docName);
                            }
                            
                        } else {
                            $.messager.alert('错误', '更新失败：' + data.data, 'error');
                        }
                    }, 'json');

                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#selcet_doctor_dlg").dialog('close');
                }
            }]
        };

        $("#selcet_doctor_dlg").show();
        $("#selcet_doctor_dlg").dialog(opt);

        return false;

    });
};

