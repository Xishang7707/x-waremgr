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
            render_stockin_base_info(o['data']);
            render_audit_list(o['data']);
            render_product_list(o['data']);
            render_audit_operator(o['data']);
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
function render_stockin_base_info(o) {
    //基础信息
    $('#u_depart_name').text(o['depart_name']);
    $('#u_jobcardid').text(o['job_number']);
    $('#u_real_name').text(o['applyer']);
    $('#o_order_sn').text(o['order_sn']);
    $('#o_apply_time').text(o['add_time']);
}

/**
 * 渲染入库审批列表
 * @param {any} o
 */
function render_audit_list(o) {
    var apply_status_temp = ``;
    if (o['apply_status'] == 0) {
        apply_status_temp = `<span class='g-color-orange'><i class="layui-icon layui-icon-loading-1 layui-anim layui-anim-rotate layui-anim-loop" style="font-size: 30px; color: #1E9FFF;"></i>${o['apply_status_desc']}</span>`;
    } else if (o['apply_status'] == 1) {
        apply_status_temp = `<span class='g-color-green'><i class='layui-icon'>&#x1005;</i>${o['apply_status_desc']}</span>`;
    } else if (o['apply_status'] == 2) {
        apply_status_temp = `<span class='g-color-red'><i class='layui-icon'>&#x1007;</i>${o['apply_status_desc']}</span>`;
    }
    $('#o_apply_status').html(apply_status_temp);

    //审批者信息
    var apply_t = ``;
    for (var i = 0; i < o['audit_list'].length; i++) {
        var item = o['audit_list'][i];
        var item_apply = '';
        if (item['audit_status'] == 0) {
            item_apply = `<span class='g-color-red'><i class='layui-icon'>&#x1007;</i>${item['audit_status_desc']}</span>`;
        } else if (item['audit_status'] == 1) {
            item_apply = `<span class='g-color-green'><i class='layui-icon'>&#x1005;</i>${item['audit_status_desc']}</span>`;
        } else {//if (o['apply_status'] == null)
            item_apply = `<span class='g-color-orange'><i class="layui-icon layui-icon-loading-1 layui-anim layui-anim-rotate layui-anim-loop" style="font-size: 30px; color: #1E9FFF;"></i>未处理</span>`;
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
 * 渲染入库产品
 * @param {any} o
 */
function render_product_list(o) {
    var hr = `<hr class="layui-bg-green">`;
    var dom = hr;
    for (var index in o['products']) {
        var item = o['products'][index];
        var t = + new Date();
        var temp = `
<div class="stockin-apply-product-item" data-item-key=${t}>
                    <fieldset class="layui-elem-field">
                        <form class="layui-form" action="">
                            <div class="layui-field-box">
                                <div class="layui-form-item">
                                    <div class="layui-inline">
                                        <label class="layui-form-label">产品名称</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <span>${item['product_name']}</span>
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">供货商</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <span>${item['factory_name']}</span>
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">物资编号</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <span>${item['material_number']}</span>
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">批号</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <span>${item['batch_number']}</span>
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">型号</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <span>${item['model_number']}</span>
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">规格</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <input type="text" name="package_size" autocomplete="off" class="layui-input" maxlength="30">
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">数量</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <input type="text" name="quantity" autocomplete="off" class="layui-input" maxlength="30">
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">报告单</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <input type="text" name="report_card_url" autocomplete="off" class="layui-input" maxlength="30">
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">有效期</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <input type="text" name="expiration_date" autocomplete="off" class="layui-input" maxlength="30">
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">复验期</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <input type="text" name="retest_date" autocomplete="off" class="layui-input" maxlength="30">
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">使用说明</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <input type="text" name="instructions" autocomplete="off" class="layui-input" maxlength="30">
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">配件</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <input type="text" name="spare_parts" autocomplete="off" class="layui-input" maxlength="30">
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">单价</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <input type="text" name="unit_price" autocomplete="off" class="layui-input" maxlength="30">
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <label class="layui-form-label">单位</label>
                                        <div class="layui-input-inline" style="width: 225px;">
                                            <input type="text" name="unit_name" autocomplete="off" class="layui-input" maxlength="30">
                                        </div>
                                    </div>
                                    <div class="layui-inline">
                                        <div class="layui-btn-container">
                                            <button type="button" class="layui-btn layui-bg-red btn-rm-product">移除产品</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </fieldset>
                </div>
`;

        var temp2 = `
<div class="layui-row">
                <div class="layui-col-xs2">
                    产品名称： <span>${item['product_name']}</span>
                </div>
                <div class="layui-col-xs2">
                    供货商： <span>${item['factory_name']}</span>
                </div>
                <div class="layui-col-xs2">
                    物资编号： <span>${item['material_number']}</span>
                </div>
                <div class="layui-col-xs2">
                    批号： <span>${item['batch_number']}</span>
                </div>
                <div class="layui-col-xs2">
                    型号： <span>${item['model_number']}</span>
                </div>
                <div class="layui-col-xs2">
                    规格： <span>${item['package_size']}</span>
                </div>
                <div class="layui-col-xs2">
                    数量： <span>${item['quantity']}</span>
                </div>
                <div class="layui-col-xs2">
                    报告单： <span>${item['report_card_url']}</span>
                </div>
                <div class="layui-col-xs2">
                    有效期： <span>${item['expiration_date']}</span>
                </div>
                <div class="layui-col-xs2">
                    复验期： <span>${item['retest_date']}</span>
                </div>
                <div class="layui-col-xs2">
                    使用说明： <span>${item['instructions']}</span>
                </div>
                <div class="layui-col-xs2">
                    配件： <span>${item['spare_parts']}</span>
                </div>
                <div class="layui-col-xs2">
                    单价： <span>${item['unit_price']}</span>
                </div>
                <div class="layui-col-xs2">
                    单位： <span>${item['unit_name']}</span>
                </div>
            </div>
`;
        dom += temp2;
        dom += hr;
    }
    $('#stockin-apply-body').html($(dom));
}

/**
 * 渲染审批操作
 * @param {any} o
 */
function render_audit_operator(o) {
    var op_audit_temp = `
            <div class="layui-btn-container">
                <button type="button" class="layui-btn layui-bg-blue btn-audit-agree" data-order_sn='${o['order_sn']}'>同意申请</button>
                <button type="button" class="layui-btn layui-bg-red btn-audit-reject" data-order_sn='${o['order_sn']}'>驳回申请</button>
            </div>`;
    if (o['op_audit'] == true) {
        $('#operator-containt').html($(op_audit_temp));
        $('.btn-audit-agree').click(audit_agree);
        $('.btn-audit-reject').click(audit_reject);
    }
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