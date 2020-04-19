$(function () {
    $('#btn-ware-add').click(ware_add);
});

function ware_add() {
    var data = get_add_data();
    if (!data['name']) {
        return layer.msg('请输入仓库名称');
    }
    if (!data['location']) {
        return layer.msg('请输入仓库位置');
    }
    var w = get_top_window();
    post({
        url: api_host + 'ware/addware',
        data: data,
        success: o => {
            if (o['status'] != 200) {
                return w.layer.msg(o['msg']);
            }
            var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
            parent.layer.close(index);
        },
        error: o => {
            w.layer.msg(o['msg']);
        }
    })
}

function get_add_data() {
    var fm = $('#ware-form');

    var data = {
        "name": fm.find('input[name=ware_name]').val(),
        "location": fm.find('input[name=location]').val(),
        "remark": fm.find('input[name=remark]').val(),
    };

    return data;
}