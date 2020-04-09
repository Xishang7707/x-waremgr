$(function () {
    var user_info;
    var layer;
    layui.use(['layer', 'form'], function () {
        layer = layui.layer, form = layui.form;
    });

    get({
        url: api_host + 'user/getuserinfo',
        success: o => {
            if (o['status'] == 401) {
                location.href = 'login';
                return;
            }
            user_info = o['data'];

            $('#info_real_name').text(user_info['real_name']);
        },
        error: o => {
            location.href = 'login';
        }
    })

    $('#login-out').click(() => {
        post({
            url: api_host + 'user/loginout',
            success: o => {
                location.href = 'login';
            },
            e: o => {
                location.href = 'login';
            }
        });
        token('');
    })
});
