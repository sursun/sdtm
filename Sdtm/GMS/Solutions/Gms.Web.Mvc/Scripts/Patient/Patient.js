
var Patient;
if (!Patient) {
    Patient = {};
}

Patient.Init = function () {

    $('#patient_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [[
            { field: 'RealName', title: '姓名', width: 100 ,formatter: function(value, row, index) {
                return '<span style="color:#ff6600;" >' + row.CodeNo + '</span>&ensp;' + value ;
            } },
            { field: 'IdCard', title: '身份证号', width: 120 },
            { field: 'SexString', title: '性别', width: 40 },
            { field: 'Age', title: '年龄', width: 100 },
            { field: 'DiabetesString', title: '诊断分型', width: 100 },
            { field: 'DiseaseCourse', title: '病程', width: 100 },
            { field: 'DiseaseStageString', title: '病程阶段 ', width: 100 },
            { field: 'FollowUpStatusString', title: '随访状态', width: 100 },
            { field: 'FollowUpDateString', title: '下次随访时间', width: 100 },
            {
                field: 'caozuo', title: '操作', width: 100, formatter: function(value, row, index) {

                    return '<a href="#" class="btn-small" >随访管理</a>';
                }
            }
        ]],
        onClickCell: function (index, field, value) {

            if (field != "caozuo")
                return false;

            var rows = $(this).datagrid('getRows');
            var row = rows[index];
            if (row == null)
                return false;

            var title = row.RealName;
            title += "【";
            title += row.IdCard;
            title += "-";
            title += row.CodeNo;
            title += "】";
            window.parent.addNewTab(title, "/FollowUp/Patient/" + row.Id, true);

            return true;
        }
    });
};

Patient.Add = function () {

    $.extend($.fn.validatebox.defaults.rules, {
        IdCard: {
            validator: function (value, param) {
                var bRet = IdCardValidate(value);

                if (bRet) {
                    var brith = getBrith(value);
                    var strBrith = brith.getFullYear();//getFullYear(); getMonth()//.getDate()
                    strBrith += "-";
                    strBrith += (brith.getMonth() + 1);
                    strBrith += "-";
                    strBrith += brith.getDate();
                    
                    $(param[1]).datebox('setValue', strBrith);

                    var nSex = 0;
                    var strSex = maleOrFemalByIdCard(value);
                    if (strSex == "female") {
                        nSex = 1;
                    }

                    var selObj = document.getElementById(param[0]);

                    for (var i = 0; i < selObj.length; i++) {//给select赋值  
                        if (nSex == selObj.options[i].value) {
                            selObj.options[i].selected = true;
                        }
                    }

                }

                return bRet;
            },
            message: '身份证号无效！'
        }
    });

    $("#PatientAdd").validate({
        onkeyup: false,
        onfocusout: true,
        rules: {
            RealName: "required",
            IdCard: "required",
            Birthday: "required"
        },
        messages: {
            RealName: "请输入患者姓名",
            IdCard: "请输入身份证号",
            Birthday: "请填写出生日期"
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

    $('#btnSubmit').click(function () {
        var idcard = $("input[name='IdCard']").val();
        if (!IdCardValidate(idcard)) {
            $.messager.alert('错误', '身份证号验证失败', 'error');
            return false;
        }

        var formOptions = {
            defaultbefore: false,
            dataType: 'json',
            success: function (data, status, xhr, $form) {

                if (data.success) {
                    //alert("成功");
                    window.location.href = "/FollowUp/Patient/" + data.data;
                } else {
                    $.messager.alert('错误', '保存失败：' + data.data, 'error');
                }
            }
        };

        $('#PatientAdd').submitForm(formOptions);
    });

    var initVal = function() {
        var code = $("#newcodeno").val();
        $("#CodeNo").val(code);
    };
    initVal();


};

