$(function () {
    $('#btn-factory-add').click(() => {
        factory_add();
    });
});

/**
 * 获取供应商数据
 * */
function get_add_factory_data() {
    var item = $('#factory-add-form');
    var data = {
        'factory_name': item.find('input[name=factory_name]').val(),
        'factory_tel': item.find('input[name=factory_tel]').val(),
        'factory_person_name': item.find('input[name=factory_person_name]').val()
    };
    return data;
}

function reset_add_factory_data() {
    $('#factory-add-form').find('input').val('');
}

function factory_add() {
    var w = get_top_window();
    post({
        url: api_host + 'factory/addfactory',
        data: get_add_factory_data(),
        success: o => {
            w.layer.msg(o['msg']);
            if (o['status'] != 200) {
                return;
            }

            var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
            if (index) {
                parent.layer.close(index);
            } else {
                reset_add_factory_data();
            }
        },
        error: o => {
            w.layer.msg(o['msg']);
        }
    })
}