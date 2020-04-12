var form;
var factory_data = [];
$(function () {
    var w = get_top_window();
    if (w._cache_data['user_info']) {
        set_stockin_apply_info(w._cache_data['user_info']);
    } else {
        get({
            url: api_host + 'user/getuserinfo',
            success: o => {
                if (o['status'] == 401) {
                    w.location.href = 'login';
                    return;
                }
                _cache_data['user_info'] = o['data'];
                set_stockin_apply_info(w._cache_data['user_info']);
            },
            error: o => {
                w.location.href = 'login';
            }
        });
    }

    //供货商
    get({
        url: api_host + `factory/searchfactorydrop`,
        async: false,
        success: o => {
            factory_data = o['data'];
        },
        error: o => {

        }
    })

    layui.use(['form'], () => {
        form = layui.form;
        $('#btn-stockin-apply').click(stockin_apply);
        $('#btn-add-product').click(add_product);
        add_product();
    });
});

function set_stockin_apply_info(info) {
    $('#u_depart_name').text(info['department_name']);
    $('#u_jobcardid').text(info['department_name']);
    $('#u_real_name').text(info['real_name']);
}

function add_product() {
    var t = +new Date();
    var factory_select_item = '';
    for (var k in factory_data) {
        var op_temp = `<option value="${factory_data[k]['key']}">${factory_data[k]['value']}</option>`;
        factory_select_item += op_temp;
    }

    if (factory_data.length == 0) {
        factory_select_item = `<option disabled>无可用供货商</option>`;
    } else {
        factory_select_item = `<option></option>` + factory_select_item;
    }

    var temp = `
            <div class="stockin-apply-product-item" data-item-key=${t}>
                <fieldset class="layui-elem-field">
                    <form class="layui-form" action="">
                    <div class="layui-field-box">
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">产品名称</label>
                                <div class="layui-input-inline" style="width: 225px;">
                                    <input type="hidden" name='product_id' class="product_id">
                                    <input type="text" name="product_name" autocomplete="off" class="layui-input" maxlength="30">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">供货商</label>
                                <div class="layui-input-inline" style="width: 225px;">
                                    <input type="hidden" name='factory_id' class="factory_id" id='factory_id-${t}'>
                                    <select id='factory_name-${t}' name="factory_name" lay-verify="" lay-filter='factory_name-${t}' lay-search>
                                      ${factory_select_item}
                                    </select>
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">物资编号</label>
                                <div class="layui-input-inline" style="width: 225px;">
                                    <input type="text" name="material_number" autocomplete="off" class="layui-input" maxlength="30">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">批号</label>
                                <div class="layui-input-inline" style="width: 225px;">
                                    <input type="text" name="batch_number" autocomplete="off" class="layui-input" maxlength="30">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">型号</label>
                                <div class="layui-input-inline" style="width: 225px;">
                                    <input type="text" name="model_number" autocomplete="off" class="layui-input" maxlength="30">
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
            </div>`;

    $('#stockin-apply-body').append($(temp));
    $('.stockin-apply-product-item .btn-rm-product').click(remove_product);
    form.render(`select`);
    form.on(`select(factory_name-${t})`, function (data) {
        //console.log(data.elem); //得到select原始DOM对象
        //console.log(data.value); //得到被选中的值
        //console.log(data.othis); //得到美化后的DOM对象
        var key = $(data.elem).parents('.stockin-apply-product-item').data('item-key');
        $(`#factory_id-${key}`).val(data.value);
    });
    //set_select_search(`#factory_name-${t}`);
}

function remove_product(e) {
    $(e.target).parents('.stockin-apply-product-item').remove();
}

function remove_all_product() {
    $('#stockin-apply-body').children().remove();
}

/**
 * 入库申请
 * */
function stockin_apply() {
    post({
        url: api_host + 'stock/stockinapply',
        data: { 'products': get_stockin_apply_data() },
        success: o => {
            layer.msg(o['msg']);
            if (o['status'] != 200) {
                return;
            }
            remove_all_product();
            add_product();
        }, error: o => {
            console.log(o);
            layer.msg(o['msg']);
        }
    })
}

/**
 * 获取入库的数据
 * */
function get_stockin_apply_data() {
    var products = $('#stockin-apply-body').find('.stockin-apply-product-item');
    var data_list = new Array();
    for (var i = 0; i < products.length; i++) {
        var item = products.eq(i);

        var data = {
            'id': item.find('input.product_id').val(),
            'product_name': item.find('input[name=product_name]').val(),
            'factory_id': item.find('input[name=factory_id]').val(),
            'material_number': item.find('input[name=material_number]').val(),
            'batch_number': item.find('input[name=batch_number]').val(),
            'model_number': item.find('input[name=model_number]').val(),
            'package_size': item.find('input[name=package_size]').val(),
            'quantity': item.find('input[name=quantity]').val(),
            'report_card_url': item.find('input[name=report_card_url]').val(),
            'expiration_date': item.find('input[name=expiration_date]').val(),
            'retest_date': item.find('input[name=retest_date]').val(),
            'instructions': item.find('input[name=instructions]').val(),
            'spare_parts': item.find('input[name=spare_parts]').val(),
            'unit_price': item.find('input[name=unit_price]').val(),
            'unit_name': item.find('input[name=unit_name]').val(),
        }
        var result_data = {};
        for (var k in data) {
            if (data[k]) {
                result_data[k] = data[k];
            }
        }
        if (result_data['quantity'] || result_data['quantity'] == 0)
            result_data['quantity'] = Number.parseFloat(result_data['quantity']);
        else {
            result_data['quantity'] = 0;
        }
        if (result_data['unit_price'] || result_data['unit_price'] == 0)
            result_data['unit_price'] = Number.parseFloat(result_data['unit_price']);
        else {
            result_data['unit_price'] = 0;
        }
        if (result_data['id'] || result_data['id'] == 0)
            result_data['id'] = Number.parseFloat(result_data['id']);
        else {
            result_data['id'] = 0;
        }
        if (result_data['factory_id'] || result_data['factory_id'] == 0)
            result_data['factory_id'] = Number.parseFloat(result_data['factory_id']);
        else {
            result_data['factory_id'] = 0;
        }
        data_list.push(result_data);
    }
    return data_list;
}

function set_select_search(elem) {
    var el = $(elem);
    ((el) => {
        $('.stockin-apply-product-item .layui-form-select').on('input', function func(e) {
            var _t = $(e.target);
            _t.parents('.stockin-apply-product-item').find('input.factory_id').val('');
            get({
                url: api_host + `factory/searchfactorydrop?name=${_t.val()}`,
                success: o => {
                    var _old_val = _t.val();
                    el.children().remove();
                    var op_temp = `<option>${_old_val}</option>`;
                    el.append(op_temp);
                    for (var k in o['data']) {
                        console.log(o['data'][k]);
                        var op_temp = `<option value="${k}">${o['data'][k]['value']}</option>`;
                        el.append(op_temp);
                    }
                    form.render(`select`);
                    _t.val(_old_val);
                    $('.stockin-apply-product-item .layui-form-select').on('input', func);
                },
                error: o => {

                }
            })
        })
    })(el);
}

function factory_name_selected(e, o) {
    var _t = $(e.target);
    _t.val(o['value']);
    _t.parents('.stockin-apply-product-item').find('.factory_id').val(o['key']);
}