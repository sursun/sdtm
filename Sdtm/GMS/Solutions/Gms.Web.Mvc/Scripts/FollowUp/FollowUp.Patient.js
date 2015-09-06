
var FollowUp;
if (!FollowUp) {
    FollowUp = {};
}

FollowUp.Index = function() {

    $('#fuzhen_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [
            [
                { field: 'Name', title: '记录名称', width: 100 },
                { field: 'CreateTimeString', title: '就诊日期', width: 100 },
                { field: 'DoctorName', title: '责任医生', width: 100 },
                { field: 'DiseaseStageName', title: '病程阶段' }
            ]
        ]
    });
};

FollowUp.Patient = function () {

    $('#mainTab').tabs({
        onSelect: function (title, index) {
            var tab = $("#mainTab").tabs("getSelected");

            var src = $("iframe", tab).attr("src");
            if (src == undefined || src.length < 2) {
                var url = $("iframe", tab).attr("url");
                $("iframe", tab).attr("src", url);
            }

        }
    });

    $('#fullwin').dialog({
        title: '随访',
        //href: '/BaseLine/Index/1',
        inline:true,
        fit:true,
        closed: true,
        buttons: [{
            text: '关闭',
            handler: function () {
                $('#fullwin').dialog("close");
            }
        }]
    });

    $('.addfollowup').live("click", function () {
        
        openIframeDialog('/BaseLine/Index/1', "基线资料");

    });

    var openIframeDialog = function (url, title) {

        //设置标题
        var opts = $('#fullwin').dialog('options');
        opts.title = title;
        $('#fullwin').dialog(opts);

        //更改链接
        $("iframe", '#fullwin').attr("src", url);
        $('#fullwin').dialog("open");

    };
    //-----------------显示随访信息----------------------//
    var loadFollowupInfo = function () {
        $("#followupinfo").empty();
        //var url = $("#followupinfo").attr("url");//+ "&date=" + new Date();
        //$("#followupinfo").load(url);
        document.getElementById('followupinfo_iframe').contentWindow.location.reload(true);
    };
    //loadFollowupInfo();

    ///------------------------随访设置----------------///
    $('#editdlg').dialog({
        title: '随访设置',
        height: 350,
        width: 500,
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
                            loadFollowupInfo();
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

    $('.setfollowupinfo').live("click", function () {
        var id = this.hash.substr(1);
        openDialog('/FollowUp/FollowUpInfoEdit?patientid=' + id, "随访设置");
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

    //------------------修改主治医生--------------------------------//

    $("#selectDoctor").live("click", function () {

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
                   
                    $.post("/Patient/UpdateDoctor", {
                        id: patientid,
                        doctorid: docId
                    }, function (data) {
                        if (data.success) {
                            $("#mainDoctor").text(docName);
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


    ///------------------------转型----------------///
    $('#selectDiabetesDlg').dialog({
        title: '糖尿病转型',
        height: 150,
        width: 200,
        closed: true,
        buttons: [{
            text: '确定',
            handler: function () {

                var patientid = $("#PatientId").val();

                var typ = $("#Diabetes").val();
               
                //$.post("/Patient/UpdateDiabetesType", {
                //    id: patientid,
                //    diabetesid: typ
                //}, function (data) {
                //    if (data.success) {
                //        $("#Diabetes_Name").text(name);
                //        $("#btnChange").hide();
                //        $.messager.alert('成功', '转型成功！', 'warning');

                //        window.location.reload();
                //    } else {
                //        $.messager.alert('错误', '转型失败：' + data.data, 'error');
                //    }
                //}, 'json');

                $('#selectDiabetesDlg').dialog("close");

            }
        }, {
            text: '取消',
            handler: function () {
                $('#selectDiabetesDlg').dialog("close");
            }
        }]
    });

    $("#btnChange").live("click", function () {
        $("#selectDiabetesDlg").dialog('open');
    });

};

