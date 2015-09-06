
var ChanHou;
if (!ChanHou) {
    ChanHou = {};
}

ChanHou.Init = function () {

    var selectRowIndex = -1;

    $('#followup_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [
            [
                { field: 'Name', title: '随访名称', width: 150 },
                { field: 'CreateTimeString', title: '随访日期', width: 100 },
                { field: 'DoctorName', title: '责任医生', width: 100 }
            ]
        ],
        onSelect: function (index, row) {

            if (row != undefined) {

                loadDetail(row.ChanHouId);
                selectRowIndex = index;
            }

        },
        onLoadSuccess: function (data) {

            if (selectRowIndex > -1) {
                $('#followup_search_list').datagrid("selectRow", selectRowIndex);
            } else {
                $("#DetailDiv").hide();
                $("#EmptyDiv").show();
            }
        }
    });

    function loadDetail(id) {
        
        if (id > 0) {
            $("#DetailDiv").load("/FollowUp/ChanHouDetail", { chanhouid: id }, function() {
                $("#DetailDiv").show();
                $("#EmptyDiv").hide();
            });
        } else {
            $("#DetailDiv").hide();
            $("#EmptyDiv").show();
        }
    }
    
    $("#btnAdd").click(function () {

        var patientId = $("#PatientId").val();

        var name = prompt("请输入名称", "");
        if (name != null && name != "") {

            $.post("/FollowUp/ChanHouAdd", { patientid: patientId,name:name }, function (data) {
                if (data.success) {
                    var pager = $('#followup_search_list').datagrid("getPager");
                    selectRowIndex = 0;
                    pager.pagination('select', 1);
                } else {
                    $.messager.alert('错误', '添加失败：' + data.data, 'error');
                }
            }, 'json');
        }

        

    });

    $("#btnDel").click(function () {

        var row = $('#followup_search_list').datagrid("getSelected");

        if (row == null)
            return;

        $.messager.confirm('提示', '确定要删除选中的数据吗？', function (b) {
            if (b) {
                $.post("/FollowUp/ChanHouDelete", { chanhouid: row.ChanHouId }, function (data) {
                    if (data.success) {
                        selectRowIndex--;
                        $('#followup_search_list').datagrid("reload");
                        loadDetail(0);
                    } else {
                        $.messager.alert('错误', '删除失败：' + data.data, 'error');
                    }
                }, 'json');
            }
        });
        

    });

    //-------------------------------------- 名称  -----------------------------------//
    $("#btnName").live("click", function () {

        var id = this.hash.substr(1);

        var name = prompt("请输入名称", $("#FollowUpName").text());
        if (name != null && name != "") {
            $.post("/FollowUp/ChanHouUpdateName", { followupid: id, name: name }, function (data) {
                if (data.success) {
                    $('#followup_search_list').datagrid("reload");
                } else {
                    $.messager.alert('错误', '修改失败：' + data.data, 'error');
                }
            }, 'json');
        }

    });

    //------------------------------------- 详细信息 -----------------------------------//
    
    $('#editdlg').dialog({
        title: '产后',
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
                            $('#followup_search_list').datagrid("reload");
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
        width: 1000,
        closed: true,
        buttons: [
        {
            text: '关闭',
            handler: function() {
                $('#frameeditdlg').dialog("close");
            }
        }]
    });

    $("#btnZhiBiao").live("click", function () {

        var id = this.hash.substr(1);

        openDialog('/FollowUp/ChanHouZhiBiao?chanhouid=' + id, "随访指标");

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
    
    //修改医生
    $("#selectDoctor").live("click", function () {

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

                    var row = $('#followup_search_list').datagrid("getSelected");
                    $.post("/FollowUp/ChanHouUpdate", {
                        chanhouid: row.ChanHouId,
                        doctorid: docId
                    }, function (data) {
                        if (data.success) {

                            $('#followup_search_list').datagrid("reload");

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

