$(function () {
    $('#open_ware_add').click(open_ware_add);
    $('#open_stockin_pre').click(open_stockin_pre);
    get_ware_list();
});

/**
 * 安置产品
 */
function open_stockin_pre() {
    var w = get_top_window();
    if (w.open_tab) {
        w.open_tab('安置产品', 'ware/stockin_pre', 'stockin_pre');
    } else {
        w.open('stockin_pre', '_blank');
    }
}

/**
 * 添加仓库
 * */
function open_ware_add() {
    layer.open({
        type: 2,
        title: '添加仓库',
        area: ['500px', '300px'],
        content: ['ware_add', 'no']
    });
}

function get_ware_list() {
    get({
        url: api_host + 'ware/getallwares',
        success: o => {
            if (o['status'] != 200) {
                return;
            }
            render_ware(o['data']);
        },
        error: o => {
            layer.msg('网络错误');
        }
    })
}

function render_ware(o) {
    var dom = ``;
    for (var i in o) {
        var item = o[i];
        var status = ``;
        var btn_status = ``;
        if (item['status'] == 1) {
            status = `<span style='color:#5FB878'>正常</span>`;
            btn_status = `<button type="button" class="layui-btn layui-btn-sm layui-btn-fluid layui-btn-warm btn-disabled-product">禁用</button>`;
        } else {
            status = `<span style='color:#FF5722'>禁用</span>`;
            btn_status = `<button type="button" class="layui-btn layui-btn-sm layui-btn-fluid layui-btn-warm btn-disabled-product">启用</button>`;
        }

        var temp = `
                <tr>
                    <td>仓库名称：${item['name']}</td>
                    <td>${item['location']}</td>
                    <td>${item['remark']}</td>
                    <td style='text-align:center;'>${status}</td>
                    <td>
                        <div class="layui-btn-container">
                            <button type="button" class="layui-btn layui-btn-sm layui-btn-fluid btn-edit-product">编辑</button>
                            ${btn_status}
                            <button type="button" class="layui-btn layui-btn-sm layui-btn-fluid layui-bg-red btn-rm-product">删除</button>
                        </div>
                    </td>
                </tr>`;
        dom += temp;
    }

    $('#ware-list').html($(dom));
}