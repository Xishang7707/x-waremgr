﻿var api_host = 'http://127.0.0.1:7001/api/'

/**
 * GET请求
 * @param {any} param0
 */
function get({ url, data, async = true, success, error }) {
    $.ajax({
        url: url,
        data: data,
        headers: {
            'token': token(),
            'lang': lang()
        },
        async: async,
        type: 'get',
        success: o => {
            success && success(o);
        },
        error: o => {
            error && error(o);
        }
    })
}

/**
 * POST请求
 * @param {any} param0
 */
function post({ url, data, async = true, success, error }) {
    $.ajax({
        url: url,
        headers: {
            'token': token(),
            'lang': lang()
        },
        contentType: 'application/json',
        data: JSON.stringify(data),
        type: 'post',
        processData: false,
        async: async,
        success: o => {
            success && success(o);
        },
        error: o => {
            if (o.responseJSON['status'] != 500)
                success && success(o.responseJSON);
            else
                error && error(o);
        }
    })
}

/**
 * token
 * @param {any} v
 */
function token(v) {
    if (v) {
        Cookies.set('x-access-s', v);
    }
    return Cookies.get('x-access-s');
}

/**
 * lang
 * @param {any} v
 */
function lang(v) {
    if (v) {
        Cookies.set('lang', v);
    }
    return Cookies.get('lang');
}

/**
 * 获取顶层window
 */
function get_top_window() {
    var p = window.parent;
    while (p != p.window.parent) {
        p = p.window.parent;
    } return p;
}