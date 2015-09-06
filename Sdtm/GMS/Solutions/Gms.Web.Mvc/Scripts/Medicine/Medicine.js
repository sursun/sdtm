
var Medicine;
if (!Medicine) {
    Medicine = {};
}

Medicine.Init = function () {

    $('#medicine_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [
            [
                { field: 'MedicineTypeString', title: '药物类型', width: 100 },
                { field: 'NormalName', title: '常用名称', width: 100 },
                { field: 'ChemicalName', title: '化学名称', width: 100 },
                { field: 'PinYin', title: '简拼', width: 100 },
                {
                    field: 'Model',
                    title: '规格',
                    width: 100
                },
                { field: 'EnabledString', title: '状态', width: 50 },
                { field: 'Note', title: '备注', width: 120 },
                {
                    field: 'IsRecommend', title: '是否推荐', width: 100, formatter: function (value, row, index) {

                        if (value == 1) {
                            return '<span style="color:red;">已推荐</span>';
                        }
                        else {
                            return ' ';
                        }
                    }
                },
            {
                field: 'Caozuo', title: '操作', width: 100, formatter: function (value, row, index) {

                    if (row.IsRecommend == 0) {
                        return '<a href="#" class="btn-small">推荐</a>';
                    }
                    else {
                        return '<a href="#" class="btn-small">取消推荐</a>';
                    }
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

            $.post("/Medicine/UpdateRecommend", { id: row.Id }, function (data) {
                if (data.success) {
                    $('#medicine_search_list').datagridEx('reload');
                } else {
                    $.messager.alert("提示", data.data);
                }
            }, "json");

            return true;
        },
        onSelect: function (rowIndex, rowData) {
            if (rowData != null) {
                $('#medicine_selected_Id').val(rowData.Id);
                $('#medicine_selected_Name').val(rowData.NormalName);
            }
        }
    });

    //----------------------------------------- 选择类型 -------------------------------------//
    $("#selectType").live("click", function () {

        var url = this.hash.substr(1);

        $("#selcet_type_dlg_content").attr("src", url);

        var opt = {
            title: '选择类别',
            width: 500,
            height: 400,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var deptContents = $("#selcet_type_dlg_content").contents();

                    $("#Type_Id").val(deptContents.find("#commoncode_selected_Id").val());
                    $("#Type_Name").val(deptContents.find("#commoncode_selected_Name").val());
                 
                    $("#selcet_type_dlg").dialog('close');
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#selcet_type_dlg").dialog('close');
                }
            }]
        };

        $("#selcet_type_dlg").show();
        $("#selcet_type_dlg").dialog(opt);

        return false;

    });

  
};

