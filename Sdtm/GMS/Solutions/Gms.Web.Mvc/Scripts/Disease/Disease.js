
var Disease;
if (!Disease) {
    Disease = {};
}

Disease.Init = function () {

    $('#disease_search_list').datagridEx({
        toolbar: '#toolbar',
        pagination: true,
        singleSelect: true,
        columns: [[
            { field: 'CodeNo', title: '国际码', width: 100 },
            { field: 'Name', title: '名称', width: 100 },
            { field: 'TypeString', title: '类型', width: 100 },
            { field: 'PinYin', title: '简拼', width: 100 }
        ]],
        onSelect: function (rowIndex, rowData) {
            if (rowData != null) {
                $('#disease_selected_Id').val(rowData.Id);
                $('#disease_selected_Name').val(rowData.Name);
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

