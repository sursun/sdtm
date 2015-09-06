
var Zlfa;
if (!Zlfa) {
    Zlfa = {};
}

Zlfa.Init = function () {


    $('#medicate_list').datagrid({
        toolbar: '#toolbar',
        singleSelect: true,
        columns: [
            [
            { field: 'MedicineTypeString', title: '药物类型', width: 80 },
            {
                field: 'NormalName', title: '药物名称', width: 150, formatter: function (value, row, index) {
                    return row.NormalName + "[" + row.ChemicalName + "]";

                } },
            {
                field: 'Model', title: '规格', width: 80
            },
            {
                field: 'DoseTypeName', title: '给药途径', width: 100, editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'DoseTypeValue',
                        textField: 'DoseTypeName',
                        method: 'get',
                        url: '/Health/GetDoseType'
                    }
                }
            },
            {
                field: 'UsageName', title: '用法', width: 100, editor: {
                type: 'combobox',
                options: {
                    valueField: 'UsageValue',
                    textField: 'UsageName',
                    method: 'get',
                    url: '/Medicine/GetUsage'
                }
            } },
            { field: 'Dosage', title: '用量', width: 100, editor: 'text' },
            { field: 'Note', title: '备注', width: 200, editor: 'textarea' },
            //{ field: 'StartDateTimeString', title: '开始用药时间', width: 100, editor: "datebox" },

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

            if (endEditing()) {
                $(this).datagrid('deleteRow', index);
                //event.preventDefault();
            }
   
            return true;
        },
        onDblClickRow: onDblClickRow
    });

    function onDblClickRow(index,row) {
        if (editIndex != index) {
            if (endEditing()) {
                $('#medicate_list').datagrid('selectRow', index)
                .datagrid('beginEdit', index);
                editIndex = index;
            } else {
                $('#medicate_list').datagrid('selectRow', editIndex);
            }
        } else {
            endEditing();
        }
    };

    var editIndex = undefined;
    function endEditing() {
        if (editIndex == undefined) {
            return true;
        }
        if ($('#medicate_list').datagrid('validateRow', editIndex)) {
            var ed = $('#medicate_list').datagrid('getEditor', { index: editIndex, field: 'UsageName' });
            var usageString = $(ed.target).combobox('getText');
            $('#medicate_list').datagrid('getRows')[editIndex]['UsageName'] = usageString;

            ed = $('#medicate_list').datagrid('getEditor', { index: editIndex, field: 'DoseTypeName' });
            var doseString = $(ed.target).combobox('getText');
            $('#medicate_list').datagrid('getRows')[editIndex]['DoseTypeName'] = doseString;
            
            $('#medicate_list').datagrid('endEdit', editIndex);
            editIndex = undefined;
            return true;
        } else {
            return false;
        }
    };

    $('#medicatetag').combobox({
        valueField: 'value',
        textField: 'label',
        mode: "remote",
        url:"/Medicine/AutocompleteGetMedicine",
        onSelect: function (record) {
            insertRow(record.value);
        }
    });
  
    //$("#medicatetag").autocomplete({
    //    source: function (request, response) {

    //        var url = '/Medicine/AutocompleteGetMedicine?q=' + request.term;

    //        $.ajax({
    //            'url': url,
    //            dataType: 'json',
    //            success: function (dataObj) {
    //                response(dataObj); //将数据交给autocomplete去展示
    //            }
    //        });

    //    },
    //    select: function(event, ui) {

    //        insertRow(ui.item.value);

    //        event.preventDefault();
    //    }
    //});

    function insertRow(medicineid) {
        
        if (!endEditing()) {
            return false;
        }

        var rows = $('#medicate_list').datagrid('getData').rows;
        for (var index in rows) {
            if (medicineid == rows[index].Id) {
                alert("此药品已经存在！");
                return false;
            }
        }

        $.post("/Medicine/GetMedicine", { id: medicineid }, function(data) {
            if (data.success) {
                
                $('#medicate_list').datagrid('insertRow', {
                    index: 0, // index start with 0
                    row: data.data
                });
            }
        }, "json");

    };

  
    $('#btnSave').click(function () {
        
        if (!endEditing()) {
            return false;
        }

        var datagridData = $('#medicate_list').datagrid('getData');
   
        var aToStr = JSON.stringify(datagridData.rows);

        var other = $("#Other").val();
        var sport = $("#Sport").val();
        var note = $("#Note").val();
        var id = $("#treatmentId").val();
        

        $.post("/Health/SaveOrUpdateTreatment", {
            treatmentid: id,
            medicates: aToStr,
            other: other,
            sport: sport,
            note: note
        }, function(data) {
            if (data.success) {

                alert("保存成功！");
       
            } else {
                alert(data.data);
            }
        }, "json");

    });

    $("a", ".recommend-list").live("click", function() {
        var mId = this.hash.substr(1);
        insertRow(mId);
    });
    
    //添加药品
    $("#btnAdd").click(function () {

        $("#selcet_medicine_dlg_content").attr("src", "/Medicine");

        var opt = {
            title: '选择药品',
            width: 500,
            height: 400,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var subContents = $("#selcet_medicine_dlg_content").contents();

                    $("#selcet_medicine_dlg").dialog('close');

                    var medicineId = subContents.find("#medicine_selected_Id").val();

                    insertRow(medicineId);
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#selcet_medicine_dlg").dialog('close');
                }
            }]
        };

        $("#selcet_medicine_dlg").show();
        $("#selcet_medicine_dlg").dialog(opt);

        return false;

    });
};

