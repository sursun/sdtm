
var Ylzd;
if (!Ylzd) {
    Ylzd = {};
}

Ylzd.Init = function () {

    $('#disease_list').datagrid({
        toolbar: '#toolbar',
        singleSelect: true,
        columns: [[
            { field: 'CodeNo', title: '国际码', width: 100 },
            { field: 'Name', title: '疾病名称', width: 100 },
            { field: 'TypeString', title: '所属类别', width: 100 },
            {
                field: 'Caozuo', title: '操作', width: 80, formatter: function (value, row, index) {
                    
                    return '<a href="#">移除</a>';

                }
            }
        ]],
        onClickCell: function (index, field, value) {

            if (field != "Caozuo") {
                return false;
            }

            $(this).datagrid('deleteRow', index);

            //var rows = $(this).datagrid('getRows');
            //var row = rows[index];
            //if (row == null)
            //    return false;
   
            //var did = $("#diagnosesId").val();
            //$.post("/Health/DeleteDisease", { diagnosesid: did, diseaseid: row.Id }, function (data) {
            //    if (data.success) {
            //        $('#disease_list').datagrid('reload');
            //    } else {
            //        $.messager.alert('错误', '删除失败：' + data.data, 'error');
            //    }
            //}, 'json');

            return true;
        }
    });

    $('#diseasetag').combobox({
        valueField: 'value',
        textField: 'label',
        mode: "remote",
        url: "/Disease/AutocompleteGetDisease",
        onSelect: function (record) {
            insertRow(record.value);
        }
    });
  
    //$("#diseasetag").autocomplete({
    //    source: function (request, response) {

    //        var url = '/Disease/AutocompleteGetDisease?q=' + request.term;

    //        $.ajax({
    //            'url': url,
    //            dataType: 'json',
    //            success: function (dataObj) {
    //                response(dataObj); //将数据交给autocomplete去展示
    //            }
    //        });

    //    },
    //    select: function (event, ui) {

    //        insertRow(ui.item.value);
  
    //        //$('#disease_list').datagrid('insertRow', {
    //        //    index: 0,	// index start with 0
    //        //    row: {
    //        //        Id: ui.item.value,
    //        //        CodeNo: ui.item.param1,
    //        //        Name: ui.item.label,
    //        //        TypeString: ui.item.param2,
    //        //        Caozuo: '<a href="#">移除</a>'
    //        //    }
    //        //});

    //        //var did = $("#diagnosesId").val();
    //        //var did = document.getElementById("diagnosesId").defaultValue;

    //        //$.post("/Health/AddDisease", { diagnosesid: did, diseaseid: ui.item.value }, function (data) {
    //        //    if (data.success) {
    //        //        $('#disease_list').datagrid('reload');
    //        //    } else {
    //        //        $.messager.alert('错误', '删除失败：' + data.data, 'error');
    //        //    }
    //        //}, 'json');

    //        event.preventDefault();
    //    }
    //});

    function insertRow(diseaseid) {
        
        var rows = $('#disease_list').datagrid('getData').rows;
        for (var index in rows) {
            if (diseaseid == rows[index].Id) {
                $.messager.alert("提示", "此疾病已经存在！");
                return false;
            }
        }

        $.post("/Disease/GetDisease", { id: diseaseid }, function (data) {
            if (data.success) {

                $('#disease_list').datagrid('insertRow', {
                    index: 0, // index start with 0
                    row: data.data
                });
            }
        }, "json");

    };


    //$("#DiseaseStage").change(function() {
  
    //    var did = $("#diagnosesId").val();

    //    $.post("/Health/UpdateDiseaseStage", { diagnosesid: did, stage: $("#DiseaseStage").val() }, function (data) {
    //        if (!data.success) {
    //            $.messager.alert('错误', '更改失败：' + data.data, 'error');
    //        }
    //    }, 'json');
    //});

    //$("#Diabetes").change(function () {

    //    var did = $("#diagnosesId").val();

    //    $.post("/Health/UpdateDiabetes", { diagnosesid: did, diabetesid: $("#Diabetes").val() }, function (data) {
    //        if (!data.success) {
    //            $.messager.alert('错误', '更改失败：' + data.data, 'error');
    //        }
    //    }, 'json');
    //});

    $('#btnSave').click(function () {

        var datagridData = $('#disease_list').datagrid('getData');
   
        var aToStr = JSON.stringify(datagridData.rows);
        var id = $("#diagnosesId").val();

        $.post("/Health/SaveOrUpdateDiagnoses", {
            diagnosesid: id,
            diagnoseslist: aToStr,
            diabetesid: $("#Diabetes").val(),
            stage: $("#DiseaseStage").val()
        }, function(data) {
            if (data.success) {
                alert("保存成功！");
            } else {
            }
        }, "json");

    });

    //添加诊断
    $("#btnAdd").click(function () {

        $("#selcet_disease_dlg_content").attr("src", "/Disease");

        var opt = {
            title: '选择诊断',
            width: 500,
            height: 400,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var subContents = $("#selcet_disease_dlg_content").contents();
                                        
                    $("#selcet_disease_dlg").dialog('close');

                    var diseaseId = subContents.find("#disease_selected_Id").val();

                    insertRow(diseaseId);
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#selcet_disease_dlg").dialog('close');
                }
            }]
        };

        $("#selcet_disease_dlg").show();
        $("#selcet_disease_dlg").dialog(opt);

        return false;

    });
    
};

