
//正式脚本移除注释
function 工作区域() {
    return '';//全屏模式
}

function 屏幕() {
    return -1;//-1是所有桌面
}
//JS入口
function 运行() {
    //添加控制逻辑

    //不是备案网站环境
    if (!区域特征_是备案网站()) {

        代理.系统.打开('C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe', 'C:\\Users\\59545\\Desktop\\自动机器人');
    }

    if (区域特征_备案网站特征1() && 区域特征_备案网站特征2()) {
        脚本任务();
    }
    else {
        Console.WriteLine('备案网站已发生变化，脚本失效！');
    }


    return '';
}


//活动脚本
function 脚本任务() {

    // 代理.鼠标.移动(47, 143);
    // 代理.延迟(300);
    var y = 145;
    代理.鼠标.单击(47, y);
    代理.延迟(300);
    键盘.输入('房间1001');
    代理.延迟(300);

    y += 20;
    代理.鼠标.单击(47, y);
    代理.延迟(300);
    键盘.输入('13256000');
    代理.延迟(300);

    y += 20;
    代理.鼠标.单击(47, y);
    代理.延迟(300);
    键盘.输入('1233');
    代理.延迟(300);

    y += 20;
    代理.鼠标.单击(47, y);
    代理.延迟(300);
    键盘.输入('龚磊');
    代理.延迟(300);

    代理.系统.退出();
}

function 区域特征_是备案网站() {
    return 代理.系统.图片特征('182,68,353,25') == '34612A148521CB303D1A57EAAFF64DF4';
}

function 区域特征_备案网站特征1() {
    return 代理.系统.图片特征('5,142,41,100') == '163A770F91D5F2F26B322355636F4398';
}
function 区域特征_备案网站特征2() {
    return 代理.系统.图片特征('1,241,56,28') == '2F0F0A33ECF5D6538C17AD8B185E86F6';
}