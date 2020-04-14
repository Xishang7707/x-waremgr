$(function () {
    var order_sn = getQuery('order_sn');
    get_stockin_order(order_sn);
});

function get_stockin_order(order_sn) {
    get({
        url: api_host + `stock/getstockindetail?order_sn=${order_sn}`,
        success: o => {
            if (o['status'] != 200) {
                layer.msg(o['msg']);
                return;
            }
            render_stockin(o['data']);
        },
        error: o => {
            layer.msg(o['msg']);
        }
    })
}

/**
 * 渲染入库单
 * @param {any} o
 */
function render_stockin(o) {
    //基础信息
    $('#u_depart_name').text(o['depart_name']);
    $('#u_jobcardid').text(o['job_number']);
    $('#u_real_name').text(o['applyer']);
    $('#o_order_sn').text(o['order_sn']);
    $('#o_apply_time').text(o['add_time']);

    var apply_status_temp = ``;
    if (o['apply_status'] == 0) {
        apply_status_temp = `<p class='g-color-orange'><i class="layui-icon layui-icon-loading-1 layui-anim layui-anim-rotate layui-anim-loop" style="font-size: 30px; color: #1E9FFF;"></i>${o['apply_status_desc']}</p>`;
    } else if (o['apply_status'] == 1) {
        apply_status_temp = `<p class='g-color-green'><i class='layui-icon'>&#x1005;</i>${o['apply_status_desc']}</p>`;
    } else if (o['apply_status'] == 2) {
        apply_status_temp = `<p class='g-color-red'><i class='layui-icon'>&#x1007;</i>${o['apply_status_desc']}</p>`;
    }
    $('#o_apply_status').html(apply_status_temp);

    //审批者信息
    var apply_t = ``;
    for (var i = 0; i < o['audit_list'].length; i++) {
        var item = o['audit_list'][i];
        var item_apply = '';
        if (o['apply_status'] == 0) {
            item_apply = `<p class='g-color-red'><i class='layui-icon'>&#x1007;</i>${o['apply_status_desc']}</p>`;
        } else if (o['apply_status'] == 1) {
            item_apply = `<p class='g-color-green'><i class='layui-icon'>&#x1005;</i>${o['apply_status_desc']}</p>`;
        } else {//if (o['apply_status'] == null)
            item_apply = `<p class='g-color-orange'><i class="layui-icon layui-icon-loading-1 layui-anim layui-anim-rotate layui-anim-loop" style="font-size: 30px; color: #1E9FFF;"></i>未处理</p>`;
        }
        var tr = `<tr>
                    <td>
                        <p>职位：${item['position_name']}</p>
                        <p>审批者：${item['auditer']}</p>
                    </td>
                    <td>
                        ${item_apply}
                    </td>
                    <td>${item['audit_time']}</td>
                </tr>`;
        apply_t += tr;
    }
    var temp = `
    <div class="order-item">
        <div id="${o['order_sn']}"></div>
        <table class="layui-table">
            <thead>
                <tr>
                    <th>审批者信息</th>
                    <th>审批结果</th>
                    <th>审批时间</th>
                </tr>
            </thead>
            <tbody>
                ${apply_t}
            </tbody>
        </table>
    </div>`;
    $('#stockin-apply-log').html($(temp));
}

/**
 * 解析审批流程
 * @param {any} o
 */
function stock_item_apply_proc(o) {
    var step_proc = new Array();
    for (var i = 0; i < o['audit_list'].length; i++) {
        var cur_step = o['audit_list'][i];
        step_proc.push({ title: cur_step['position_name'], description: cur_step['remark'] ? cur_step['remark'] : '' });
    }
    steps({
        el: `#${o['order_sn']}`,
        data: step_proc,
        active: o['audit_step_index']
    });
}
