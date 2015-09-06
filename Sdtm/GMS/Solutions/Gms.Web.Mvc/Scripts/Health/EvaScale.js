
var EvaScale;
if (!EvaScale) {
    EvaScale = {};
}

EvaScale.Init = function () {

    $('#evascale_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [
            [
                { field: 'CreateTimeString', title: '评估日期', width: 100 },
                { field: 'Name', title: '量表名称', width: 200 },
                { field: 'Result', title: '评估得分', width: 100 },
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

            openDialog("/Health/EvaScaleEdit?id=" + row.EvaluationScaleId, "修改糖尿病评估相关量表");
            
            return true;
        }
    });

    $('#selectTypeDlg').dialog({
        title: '新增糖尿病评估相关量表',
        width: 260,
        height: 150,
        closed: true,
        modal: true,
        buttons: [{
            text: '确定',
            handler: function () {
                var patientid = $('#PatientId').val();

                var url = "/Health/EvaScaleEdit?id=0&patientid=" + patientid;
                url += "&type=";
                url += $("#typeCombo").val();
                
                openDialog(url, "新增糖尿病评估相关量表");

                $('#selectTypeDlg').dialog("close");

            }
        }, {
            text: '关闭',
            handler: function () {

                $('#selectTypeDlg').dialog("close");

            }
        }]
    });

    $("#btnAdd").click(function() {
        
        $('#selectTypeDlg').dialog('open');

    });

    $("#btnDel").click(function () {

        var row = $('#evascale_search_list').datagridEx("getSelected");

        if (row == null)
            return;

        $.messager.confirm('提示', '确定要删除选中的数据吗？', function (b) {
            if (b) {
                $.post("/Health/EvaScaleDelete", { id: row.EvaluationScaleId }, function (data) {
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

    //----------------------评估量表----------------------//
    $(':radio', "#formScale").live('click', function () {

        var ss = $(this);
        var curVal = ss.val();
     
        ss.parent().siblings().find('input[name="scaleresult"]').val(curVal);

        //计算总分
        var nTotal = 0;
        $('input[name="scaleresult"]', "#formScale").each(function () {
            var self = $(this);
            var tVal = self.val();
            if (tVal.length > 0) {
                var nVal = parseInt(tVal);
                nTotal += nVal;
            }
        });
        //填写总分
        $('input[name="scaletotal"]', "#formScale").val(nTotal);

    });
};

