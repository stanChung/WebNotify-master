var wnHub = $.connection.notifyHub;
var uid = '';
$(function () {

    uid = prompt('請輸入您的ID', 'stan');
    document.title = '目前使用者：' + uid;

    $('#dg').datagrid({
        idField: 'DataID',
        columns: [[
            { field: 'ReportName', title: '報表名稱', width: 100 },
            { field: 'ReportStatus', title: '報表描述', width: 150 },
            {
                field: 'IsComplete', title: '已完成', formatter: function (v, r, i) {
                    return r.IsComplete ? 'Yes' : 'No';
                }
            }
        ]]

    });

    wnHub.client.showNotify = function (msg) {
        $.messager.show({
            timeout: 0,
            msg: msg,
            title: 'Information'
        });
    };

    wnHub.client.displayStatus = function () {

        GetData();

    };



    //開啟連線
    $.connection.hub.start().done(function () {
        wnHub.server.userConnected(uid);
        GetData();
    });



});


function GetData() {
    var setting = {
        url: '/Data',
        type: 'GET',
        datatype: 'json',
        data: { uid: uid },
        success: function (data) {
            var dg = $('#dg');
            var oDatas = dg.datagrid('getData');
            if (oDatas.rows.length > 0) {
                oDatas.rows.forEach(function (e, i, a) {
                    var tmp = $.grep(data, function (n) { return (n.DataID == e.DataID && n.ReportOwner == uid); });//先以DataID找出資料
                    if (tmp.length > 0) {   //如果有相對應的資料
                        if (e.IsComplete != tmp[0].IsComplete && tmp[0].IsComplete===true)  //是否已完成的狀態不一樣且狀態是true，就表示工作完成了
                            wnHub.client.showNotify('親愛的' + uid + '，您的報表「' + e.ReportName + '」已經完成了');
                    }
                });
            }

            dg.datagrid('loadData', data);
        },
        error: function (a, b, c) {
            alert(b);
        }

    };

    $.ajax(setting);
}