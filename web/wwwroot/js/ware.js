$(function () {
    $('#open_ware_add').click(open_ware_add);
    $('#open_stockin_pre').click(open_stockin_pre);
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