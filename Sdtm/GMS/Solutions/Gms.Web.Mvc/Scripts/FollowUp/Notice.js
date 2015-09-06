
var Notice;
if (!Notice) {
    Notice = {};
}

Notice.Init = function () {

    $('#followupinfo_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [
            [
                { field: 'PatientName', title: '患者姓名', width: 100, formatter: function(value, row, index) {
                    return '<span style="color:#ff6600;" >' + row.PatientCodeNo + '</span>&ensp;' + value;
                } },
                { field: 'PatientIdCard', title: '身份证号', width: 120 },
                { field: 'PatientSex', title: '性别', width: 50 },
                { field: 'PatientAge', title: '年龄', width: 50 },
                { field: 'Mobile', title: '手机号码', width: 100 },
                { field: 'DiabetesString', title: '诊断分型', width: 100 },
                { field: 'FollowUpDateString', title: '下次复诊时间', width: 100 },
                { field: 'FollowUpWayString', title: '下次复诊形式', width: 100 },
                {
                    field: 'FuZhenStatusString', title: '复诊状态', width: 80, formatter: function (value, row, index) {

                        var text = '<span style="color:';
                        var color = 'black';
                        if (value == "候诊") {
                            color = 'green';
                        }
                        if (value == "过期") {
                            color = 'red';
                        }
                        text += color;
                        text += '">';
                        text += value;
                        text += '</span>';

                        return text;
                    }
                },
                { field: 'DoctorName', title: '主治医生', width: 80 },
                {
                    field: 'caozuo',
                    title: '操作',
                    width: 100,
                    formatter: function(value, row, index) {

                        return '<a href="#" class="btn-small" >随访管理</a>';
                    }
                }
            ]
        ]
        ,
        onClickCell: function (index, field, value) {

            if (field != "caozuo")
                return false;

            var rows = $(this).datagrid('getRows');
            var row = rows[index];
            if (row == null)
                return false;

            var title = row.PatientName;
            title += "【";
            title += row.PatientIdCard;
            title += "-";
            title += row.PatientCodeNo;
            title += "】";
            window.parent.addNewTab(title, "/FollowUp/Patient/" + row.PatientId, true);

            return true;
        }
    });
};

Notice.Patient = function () {

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
        var url = $("#followupinfo").attr("url");
        $("#followupinfo").load(url);
    };
    loadFollowupInfo();

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
};

