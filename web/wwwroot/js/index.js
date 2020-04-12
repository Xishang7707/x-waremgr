var element;
$(function () {
    var user_info;

    layui.use('element', () => {
        element = layui.element;
        element.on('nav(top-nav)', function (elem) {
            var el = $(elem);
            if (!el.attr('nav-page')) {
                return;
            }
            var layid = el.attr('lay-id');
            open_tab(el.html(), el.attr('page-link'), layid);
        });

    });

    var w = get_top_window();
    get({
        url: api_host + 'user/getuserinfo',
        success: o => {
            if (o['status'] == 401) {
                w.location.href = 'login';
                return;
            }
            user_info = o['data'];
            w._cache_data['user_info'] = user_info;
            $('#info_real_name').text(`${user_info['real_name']}(${user_info['department_name']})`);
        },
        error: o => {
            w.location.href = 'login';
        }
    });

    $('#login-out').click(login_out_tologin)
});

/**
 * 添加tab
 * @param title 标题
 * @param url 链接地址
 * @param id 唯一标识
 */
function open_tab(title, url, id) {
    var layid = id;
    var lis = $('ul.layui-tab-title li');
    for (var i = 0; i < lis.length; i++) {
        var t = lis.eq(i);
        if (t.attr('lay-id') == layid) {
            element.tabChange('main-tab', layid);
            return;
        }
    }

    element.tabAdd('main-tab', {
        title: title
        , content: `<iframe src='${url}' style="width:100%;height:100%;border:none;"></iframe>`
        , id: layid
    });
    element.tabChange('main-tab', layid);
}