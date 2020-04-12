$(function () {
    var w = get_top_window();
    if (w != window) {
        w.location.href = 'login';
        return;
    }

    var layer;
    layui.use(['layer', 'form'], function () {
        layer = layui.layer, form = layui.form;
    });

    var btn_login = $('#btn-login');
    btn_login.click(() => {
        login(w, btn_login);
    });
    $('input').keydown(e => {
        if (e.keyCode == 13 || e.charCode == 13)
            login(w, btn_login);
    });

    $('#login-out').click(() => {
        post({
            url: api_host + 'user/loginout',
            success: o => {
                w.location.href = 'login';
            },
            e: o => {
                w.location.href = 'login';
            }
        });
        token('');
    })
});

function login(w, btn) {
    var user = $('#in_account');
    var pwd = $('#in_pwd');

    if (user.val() == '') {
        layer.msg("请输入用户名");
        return false;
    }
    if (pwd.val() == '') {
        layer.msg("请输入密码");
        return false;
    }

    post({
        url: api_host + 'user/login',
        data: {
            'user_name': user.val(),
            'password': pwd.val()
        },
        success: o => {
            layer.msg(o['msg']);
            if (o['status'] != 200) {
                return;
            }
            token(o['data']['token']);
            setInterval(() => {
                w.location.href = 'index';
            }, 2000);
        }, error: o => {
            layer.msg("网络繁忙 请稍后再试！");
        }
    })
}