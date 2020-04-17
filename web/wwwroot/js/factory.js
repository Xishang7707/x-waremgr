$(function () {

    search_factory();
    $('#open_factory_add').click(() => {
        layer.open({
            type: 2,
            title: '添加供应商',
            area: ['500px', '350px'],
            content: ['factory_add', 'no']
        });
    });
});

function search_factory() {
    var w = get_top_window();
    get({
        url: api_host + 'factory/searchfactorybypaginer',
        success: o => {
            if (o['status'] != 200) {
                w.layer.msg(o['msg']);
                return;
            }
            render_table(o['data']['Data']);
        },
        error: o => {
            w.layer.msg(o['msg']);
        }
    })
}

function render_table(o) {
    var dom = ``;
    for (var i in o) {
        var item = `<tr>
                        <td><span>${o[i]['factory_name']}</span></td>
                        <td>
                            <p>联系人：${o[i]['factory_person_name']}</p>
                            <p>联系电话：${o[i]['factory_tel']}</p>
                        </td>
                        <td>
                            <div class="layui-btn-container">
                                <button type="button" class="layui-btn layui-btn-sm layui-btn-fluid" onclick="open_factory_edit('${o[i]['id']}')">编辑</button>
                            </div>
                        </td>
                    </tr>`;
        dom += item;
    }
    $('#factory_table tbody').html($(dom));
}