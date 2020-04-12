$(function () {
    $('#open_stockin_apply').click(open_stockin_apply);
    get_stock_order_list();

});
/**
 * 获取表格入库订单
 * */
function get_stock_order_list() {
    get({
        url: api_host + 'stock/searchstockinpaginer',
        success: o => {
            if (o['status'] == 401) {
                layer.msg(o['msg']);
                token();
                location.href = 'login';
                return;
            } else if (o['status'] != 200) {
                layer.msg(o['msg']);
                return;
            }
            console.log(o);
            render_stockin('#stockin-body', o['data']['Data']);
        },
        error: o => {
            layer.msg('加载失败');
        }
    })
}

/**
 * 解析入库信息
 * @param {any} e
 * @param {any} o
 */
function render_stockin(e, o) {
    var el = $(e);
    for (var i = 0; i < o.length; i++) {
        var cur_item = o[i];
        var temp = stock_item_temp(cur_item);
        el.append($(temp));
        stock_item_apply_proc(cur_item);
    }

    $('.btn-audit-agree').click(audit_agree);
    $('.btn-audit-reject').click(audit_reject);
}

/**
 * 解析入库单
 * @param {any} o
 */
function stock_item_temp(o) {
    var apply_status_temp = ``;
    if (o['apply_status'] == 0) {
        apply_status_temp = `<p class='g-color-orange'><i class="layui-icon layui-icon-loading-1 layui-anim layui-anim-rotate layui-anim-loop" style="font-size: 30px; color: #1E9FFF;"></i>${o['apply_status_desc']}</p>`;
    } else if (o['apply_status'] == 1) {
        apply_status_temp = `<p class='g-color-green'><i class='layui-icon'>&#x1005;</i>${o['apply_status_desc']}</p>`;
    } else if (o['apply_status'] == 2) {
        apply_status_temp = `<p class='g-color-red'><i class='layui-icon'>&#x1007;</i>${o['apply_status_desc']}</p>`;
    }

    var op_audit_temp = `
                <button type="button" class="layui-btn layui-btn-sm layui-btn-fluid layui-bg-blue btn-audit-agree" data-order_sn='${o['order_sn']}'>同意申请</button>
                <button type="button" class="layui-btn layui-btn-sm layui-btn-fluid layui-bg-red btn-audit-reject" data-order_sn='${o['order_sn']}'>驳回申请</button>`;
    var temp = `
    <div class="order-item">
        <div id="${o['order_sn']}"></div>
        <table class="layui-table">
            <colgroup>
                <col width="350">
                <col>
                <col width="200">
                <col width="200">
                <col width="150">
            </colgroup>
            <thead>
                <tr>
                    <th>订单信息</th>
                    <th>申请信息</th>
                    <th>申请状态</th>
                    <th>申请时间</th>
                    <th style='text-align:center'>操作</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <p>订单号：${o['order_sn']}</p>
                    </td>
                    <td>
                        <p>部门：${o['depart_name']}</p>
                        <p>申请人：${o['applyer']}</p>
                    </td>
                    <td>
                        ${apply_status_temp}
                    </td>
                    <td>${o['add_time']}</td>
                    <td>
                        <div class="layui-btn-container">
                            <button type="button" class="layui-btn layui-btn-sm layui-btn-fluid">详情</button>
                            ${o['op_audit'] == true ? op_audit_temp : ''}
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>`;

    return temp;
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

/**
 * 打开入库申请
 * */
function open_stockin_apply() {
    var w = get_top_window();
    if (w.open_tab) {
        w.open_tab('入库申请', 'stock/stockin_apply', 'stockin_apply');
    } else {
        w.open('stockin_apply', '_blank');
    }
}

/**
 * 同意申请
 * @param {any} e 元素
 */
function audit_agree(e) {

    post({
        url: api_host + 'stock/stockinaudit',
        data: {
            "order_sn": $(e.target).data('order_sn'),
            "act": 1
        },
        success: o => {
            var w = get_top_window();
            w.layer.msg(o['msg']);
            if (o['status'] != 200) {
                return;
            }
            location.reload();
        },
        error: o => {
            var w = get_top_window();
            w.layer.msg(o['msg']);
        }
    })
}

/**
 * 拒绝申请
 * @param {any} e 元素
 */
function audit_reject(e) {
    post({
        url: api_host + 'stock/stockinaudit',
        data: {
            "order_sn": $(e.target).data('order_sn'),
            "act": 0
        },
        success: o => {
            var w = get_top_window();
            w.layer.msg(o['msg']);
            if (o['status'] != 200) {
                return;
            }
            location.reload();
        },
        error: o => {
            var w = get_top_window();
            w.layer.msg(o['msg']);
        }
    })
}