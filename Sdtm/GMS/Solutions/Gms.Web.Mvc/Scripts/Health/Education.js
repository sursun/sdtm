
var Education;
if (!Education) {
    Education = {};
}

Education.Init = function () {

    $('#education_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [
            [
                { field: 'CreateTimeString', title: '教育时间', width: 100 },
                { field: 'Teacher', title: '教育者', width: 100 },
                { field: 'YingYangShi', title: '营养师教育', width: 100 },
                { field: 'HuShi', title: '糖尿病护士教育', width: 100 },
                { field: 'ZuBing', title: '足病防护教育', width: 100 },
                {
                    field: 'Caozuo',
                    title: '操作',
                    width: 100,
                    formatter: function(value, row, index) {

                        return '<a href="#" class="btn-small">编辑/完善</a>';
                    }
                }
            ]
        ],
        onClickCell: function (index, field, value) {

            if (field != "Caozuo") {
                return false;
            }

            var rows = $(this).datagrid('getRows');
            var row = rows[index];
            if (row == null)
                return false;

            openDialog("/Health/EducationEdit?id=" + row.EducationId, "修改教育信息");
            
            return true;
        }
    });
    
    $("#btnAdd").click(function() {
        
        var patientid = $('#PatientId').val();
        openDialog("/Health/EducationEdit?id=0&patientid=" + patientid, "添加教育信息");

    });

    $("#btnDel").click(function () {

        var row = $('#education_search_list').datagridEx("getSelected");

        if (row == null)
            return;

        $.messager.confirm('提示', '确定要删除选中的数据吗？', function (b) {
            if (b) {
                $.post("/Health/EducationDelete", { id: row.EducationId }, function (data) {
                    if (data.success) {

                        window.location.reload();
                        //$('#education_search_list').datagrid("reload");

                    } else {
                        $.messager.alert('错误', '删除失败：' + data.data, 'error');
                    }
                }, 'json');
            }
        });

        
        
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
                            window.location.reload();//$('#education_search_list').datagrid("reload");
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

    ////----------------------事件评估----------------------//
    //$(':checkbox', "#formPingGu").live('click', function () {

    //    var ss = $(this);
     
    //    if (ss.attr('checked') == undefined) {
    //        ss.parent().parent().siblings().find('span').hide();
    //    } else {
    //        ss.parent().parent().siblings().find('span').show();
    //    }

    //});
};

