$(function () {
    search_stockin_pre();
});

function search_stockin_pre() {
    get({
        url: api_host + 'stock/searchstockprepaginer',
        success: o => {
            if (o['status'] != 200) {
                layer.msg(o['msg']);
            }
            render_stockin_pre(o['data']['Data']);
        },
        error: o => {
            layer.msg('网络错误');
        }
    })
}

function render_stockin_pre(o) {
    var dom = ``;
    for (var i in o) {
        var item = o[i];
        var temp = `<tr>
                        <td>${item['product_name']}</td>
                        <td>${item['quantity']}</td>
                        <td><button type="button" class="layui-btn layui-btn-sm layui-btn-fluid layui-bg-blue btn-stock-deposit" data-id='${item['id']}'>安置</button></td>
                    <tr>`;
        dom += temp;
    }

    $('#stockin-pre-stocks').html($(dom));
    $('#stockin-pre-stocks .btn-stock-deposit').click(open_stock_deposit);
}

function open_stock_deposit(e) {
    layer.open({
        type: 2,
        title: '安置产品',
        area: ['500px', '400px'],
        content: ['stock_deposit?id=' + $(e.target).data('id'), 'no']
    });
}