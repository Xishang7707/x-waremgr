var id = getQuery('id');
$(function () {
    $('#btn-deposit-stock').click(deposit_stock);
    layui.use(['form'], () => {
        form = layui.form;
        get({
            url: api_host + 'ware/getallwaresdrop',
            success: o => {
                if (o['status'] != 200) {
                    return;
                }
                var ware_list = o['data'];
                var ware_select_item = '';
                for (var k in ware_list) {
                    var op_temp = `<option value="${ware_list[k]['key']}">${ware_list[k]['value']}</option>`;
                    ware_select_item += op_temp;
                }

                if (ware_select_item.length == 0) {
                    ware_select_item = `<option disabled>无可用供货商</option>`;
                } else {
                    ware_select_item = `<option></option>` + ware_select_item;
                }
                $('#ware_name').html($(ware_select_item));
                form.render('select');
                form.on(`select(ware_name)`, function (data) {
                    $(`#ware_id`).val(data.value);
                });
            }
        });
    });
});

function deposit_stock() {
    var data = get_data();
    var w = get_top_window();
    if (!data['ware_id']) {
        return w.layer.msg('请选择仓库');
    }
    if (data['quantity'] < 0) {
        return w.layer.msg('安置数量必须大于0');
    }
    if (!data['location']) {
        return w.layer.msg('请填写安置位置');
    }
    post({
        url: api_host + 'ware/depositstock',
        data: data,
        success: o => {
            w.layer.msg(o['msg']);
            if (o['status'] != 200) {
                return;
            }
            var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
            parent.layer.close(index);
        },
        error: o => {
            w.layer.msg('网络错误');
        }
    })
}

function get_data() {
    var fm = $('#stock-deposit-form');

    var data = {
        "id": id,
        "ware_id": fm.find('input[name=ware_id]').val(),
        "quantity": fm.find('input[name=quantity]').val(),
        "location": fm.find('input[name=location]').val(),
        "remark": fm.find('input[name=remark]').val(),
    };
    if (!data['id']) {
        data['id'] = 0;
    } else {
        data['id'] = Number.parseFloat(data['id']);
    }
    if (!data['quantity']) {
        data['quantity'] = 0;
    } else {
        data['quantity'] = Number.parseFloat(data['quantity']);
    }
    if (!data['ware_id']) {
        data['ware_id'] = 0;
    } else {
        data['ware_id'] = Number.parseFloat(data['ware_id']);
    }

    return data;
}