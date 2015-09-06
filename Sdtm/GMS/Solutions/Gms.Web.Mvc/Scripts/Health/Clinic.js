
var Clinic;
if (!Clinic) {
    Clinic = {};
}

Clinic.Init = function () {

    var selectRowIndex = -1;

    $('#clinic_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [
            [
                { field: 'Name', title: '名称', width: 100 },
                { field: 'CreateTimeString', title: '登记日期', width: 100 },
                { field: 'IsBreathing', title: '是否健在', width: 100 },
                { field: 'Note', title: '并发症、特殊事件或新的疾病', width: 300 }
            ]
        ],
        onSelect: function (index, row) {

            if (row != undefined) {

                loadDetail(row.ClinicId);

                selectRowIndex = index;
            }
            
        },
        onLoadSuccess: function (data) {
            if (selectRowIndex > -1) {
                $('#clinic_search_list').datagrid("selectRow", selectRowIndex);
            }
        }
    });

    function loadDetail(id) {
        

        if (id > 0) {
            $("#DetailDiv").load("/Health/ClinicDetail", { clinicid: id }, function () {
                $("#DetailDiv").show();
                $("#EmptyDiv").hide();
            });
        } else {
            $("#DetailDiv").hide();
            $("#EmptyDiv").show();
        }
    }

    $("#btnAdd").click(function() {

        var patientId = $("#PatientId").val();

        var name = prompt("请输入名称", "");
        if (name != null && name != "") {

            $.post("/Health/ClinicAdd", { patientid: patientId,name:name }, function(data) {
                if (data.success) {
                    var pager = $('#clinic_search_list').datagrid("getPager");
                    selectRowIndex = 0;
                    pager.pagination('select', 1);
                } else {
                    $.messager.alert('错误', '添加失败：' + data.data, 'error');
                }
            }, 'json');
        }

    });

    $("#btnDel").click(function () {

        var row = $('#clinic_search_list').datagridEx("getSelected");

        if (row == null)
            return;

        $.messager.confirm('提示', '确定要删除选中的数据吗？', function (b) {
            if (b) {
                $.post("/Health/ClinicDelete", { clinicid: row.ClinicId }, function (data) {
                    if (data.success) {
                        selectRowIndex--;
                        $('#clinic_search_list').datagrid("reload");
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

        var name = prompt("请输入名称", $("#ClinicName").text());
        if (name != null && name != "") {
            $.post("/Health/ClinicUpdateName", { clinicid: id, name: name }, function (data) {
                if (data.success) {
                    $('#clinic_search_list').datagrid("reload");
                } else {
                    $.messager.alert('错误', '修改失败：' + data.data, 'error');
                }
            }, 'json');
        }

    });

    //-------------------------------------- 详细信息  -----------------------------------//
    $("#btnPingGu").live("click", function () {

        var id = this.hash.substr(1);

        openDialog('/Health/ClinicPingGu?clinicid=' + id, "临床事件评估");

    });
    
    $('#editdlg').dialog({
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
                            $('#clinic_search_list').datagrid("reload");
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

    var openDialog = function (url, title) {

        //设置标题
        var opts = $('#editdlg').dialog('options');
        opts.title = title;
        $('#editdlg').dialog(opts);

        //更改链接
        $('#editdlg').dialog("refresh", url);
        $('#editdlg').dialog("open");
    };

    //----------------------事件评估----------------------//
    $(':checkbox', "#formPingGu").live('click', function () {

        var ss = $(this);
     
        if (ss.attr('checked') == undefined) {
            ss.parent().parent().siblings().find('span').hide();
        } else {
            ss.parent().parent().siblings().find('span').show();
        }

    });
};

