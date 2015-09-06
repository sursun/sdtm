
var Doctor;
if (!Doctor) {
    Doctor = {};
}

Doctor.Init = function () {

    $('#doctor_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [
            [
                { field: 'CodeNo', title: '工号', width: 100 },
                { field: 'RealName', title: '姓名', width: 100 },
                { field: 'Department', title: '科室', width: 100 },
                { field: 'SexString', title: '性别', width: 100 },
                { field: 'EnabledString', title: '状态', width: 80 },
                {
                    field: 'Caozuo',
                    title: '操作',
                    width: 100,
                    formatter: function(value, row, index) {

                        return '<a href="#" class="btn-small">设置权限</a>';
                    }
                }
            ]
        ],
        onSelect: function(rowIndex, rowData) {
            if (rowData != null) {
                $('#doctor_selected_Id').val(rowData.Id);
                $('#doctor_selected_Name').val(rowData.RealName);
                $('#doctor_selected_CodeNo').val(rowData.CodeNo);
            }
        },
        onClickCell: function (index, field, value) {

            if (field != "Caozuo") {
                return false;
            }

            var rows = $(this).datagrid('getRows');
            var row = rows[index];
            if (row == null)
                return false;

            openFrameDialog('/Doctor/Scope/' + row.Id, "设置/查看权限范围");

            return true;
        }
    });
    
    $("#doctorform").validate({
        onkeyup: false,
        onfocusout: false,
        rules: {
            CodeNo: "required",
            psw: "required",
            RealName: "required",
            //Department.Name: "required",
            Mobile: "required"
        },
        messages: {
            LoginName: "请输入工号",
            psw: "请输入密码",
            RealName: "请输入真实姓名",
            //"Department.Name": "请选择班组",
            Mobile: "请输入手机号码"
        }
        , showErrors: function (errorMap, errorList) {
            var message = '请完善以下内容后，再提交。\n';
            var errors = "";
            if (errorList.length > 0) {
                for (x = 0; x < errorList.length; x++) {
                    errors += "<br/>\u25CF " + errorList[x].message;
                }

                $.messager.alert("提示", message + errors);
            }

        }

    });

    $("a.reset").live("click", function() {
        var rec = $('#doctor_search_list').datagrid("getSelected");
        if (rec == null) {
            return false;
        }
        
        $.post("/Doctor/ResetPassword", { id: rec.Id }, function (data) {
            if (data.success) {
                $.messager.alert("提示", "密码重新重置为[" + data.data + "]");
            } else {
                $.messager.alert("提示", data.data);
            }
        },"json");

    });

    //$("#user_edit").live("click", function () {
        
    //    var rec = $('#user_search_list').datagrid("getSelected");
    //    if (rec == null) {
    //        return false;
    //    }

    //    $("#user_search_edit_content").load("/User/Edit/" + rec.Id, {}, function() {
            
    //        var dlg = $("#user_search_edit");
    //        dlg.show();

    //        //隐藏需要隐藏的元素
    //        $('.addhide,.edithide').show();
    //        $('*[isrequired]', dlg).validatebox({ required: true });
    //        $('.easyui-combotree[isrequired]', dlg).combotree({ required: true });
            
    //        $('.edithide', dlg).hide();
    //        $('.edithide  *[isrequired]', dlg).validatebox({ required: false });
    //        $('.edithide  .easyui-combotree[isrequired]', dlg).combotree({ required: false });
    //        $('.editdisabled', dlg).attr("disabled", true);

    //        $(':hidden[isrequired]').validatebox({ required: false });

    //        dlg.dialog({
    //            title: "用户信息",
    //            buttons: [
    //                {
    //                    text: '保存',
    //                    iconCls: 'icon-ok',
    //                    handler: function() {

    //                        var formOptions = {
    //                            defaultbefore: false,
    //                            beforeSubmit: function(arr, $form) {
    //                                var result = $form.valid();
    //                                return result;
    //                            },
    //                            success: function(data) {

    //                                if (data.success) {
    //                                    $.messager.alert('提示', '保存成功', 'info');
    //                                    dlg.dialog('close');
    //                                    $('#user_search_list').datagrid('reload');
    //                                } else {
    //                                    $.messager.alert('错误', '保存失败：' + data.data, 'error');
    //                                }
    //                            }
    //                        };

    //                        formOptions.dataType = 'json';

    //                        $('form', dlg).submitForm(formOptions);
    //                    }
    //                }, {
    //                    text: '取消',
    //                    iconCls: 'icon-cancel',
    //                    handler: function() {
    //                        dlg.dialog('close');
    //                    }
    //                }
    //            ]
    //        });
    //    });

    //    //$("#user_search_edit").dialog(opt);

    //});

    //----------------------------------------- 选择科室 -------------------------------------//
    $("#selectDept").live("click",function () {

        $("#selcet_dept_dlg_content").attr("src", "/Department/Select");

        var opt = {
            title: '选择科室',
            width: 300,
            height: 200,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var deptContents = $("#selcet_dept_dlg_content").contents();

                    $("#Department_Id").val(deptContents.find("#dept_selected_Id").val());
                    $("#Department_Name").val(deptContents.find("#dept_selected_Name").val());
                 
                    $("#selcet_dept_dlg").dialog('close');
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#selcet_dept_dlg").dialog('close');
                }
            }]
        };

        $("#selcet_dept_dlg").show();
        $("#selcet_dept_dlg").dialog(opt);

        return false;

    });


    $('#frameeditdlg').dialog({
        title: "信息完善",
        height: 600,
        width: 900,
        closed: true,
        buttons: [{
            text: '关闭',
            handler: function () {
                $('#frameeditdlg').dialog("close");
            }
        }]
        //,
        //onClose: function () {
        //    $('#bldlg').datagrid("reload");
        //}
    });

    var openFrameDialog = function (url, title) {

        //设置标题
        var opts = $('#frameeditdlg').dialog('options');
        opts.title = title;
        $('#frameeditdlg').dialog(opts);

        //更改链接
        $("iframe", $('#frameeditdlg')).attr("src", url);

        $('#frameeditdlg').dialog("open");
    };
  
};

Doctor.Scope = function () {
    
    $("#btnAdd").live("click", function () {
        var rec = $('#doctor_search_list').datagridEx("getSelected");
        if (rec == null) {
            return false;
        }

        var doctorid = $("#doctorId").val();

        $.post("/Doctor/ScopeAdd", {id:doctorid, scopeid: rec.Id }, function (data) {
            if (data.success) {
                $('#doctor_scope_list').datagridEx('reload');
            } else {
                $.messager.alert("提示", data.data);
            }
        }, "json");
    });

    $("#btnDel").live("click", function () {
        var rec = $('#doctor_scope_list').datagridEx("getSelected");
        if (rec == null) {
            return false;
        }

        var doctorid = $("#doctorId").val();

        $.post("/Doctor/ScopeDelete", { id: doctorid, scopeid: rec.Id }, function (data) {
            if (data.success) {
                $('#doctor_scope_list').datagridEx('reload');
            } else {
                $.messager.alert("提示", data.data);
            }
        }, "json");
    });


    $("input[name='radioForScopeType']").live('click', function () {

        var curVal = $(this).val();
        var doctorid = $("#doctorId").val();

        $.post("/Doctor/UpdateScopeType", { id: doctorid, type: curVal }, function (data) {
            if (!data.success) {
                $.messager.alert("提示", data.data);
            }
        }, "json");
        
        if (curVal == 2) { //自定义
            showDiv();
        } else {
            $('#userDefine').hide();
        }
        
    });

    var initView = function () {
        var curVal = $("#scopeType").val();
        $("input[name='radioForScopeType']").each(function() {
            if ($(this).val() == curVal)
                $(this).attr("checked", "checked");
        });
        
        if (curVal == 2) { //自定义
            showDiv();
        } else {
            $('#userDefine').hide();
        }
    };

    var showDiv = function() {
        $('#userDefine').show();
        $('#doctor_search_list').datagridEx({
            toolbar: '#toolbar',
            pagination: true,
            singleSelect: true,
            columns: [
                [
                    { field: 'Department', title: '科室', width: 100 },
                    { field: 'CodeNo', title: '工号', width: 100 },
                    { field: 'RealName', title: '姓名', width: 100 }
                ]
            ]
        });

        $('#doctor_scope_list').datagridEx({
            singleSelect: true,
            columns: [
                [
                    { field: 'Department', title: '科室', width: 100 },
                    { field: 'CodeNo', title: '工号', width: 100 },
                    { field: 'RealName', title: '姓名', width: 100 }
                ]
            ]
        });
    };

    initView();
};