//这是菜单树创建界面，这里面可以用模板快速地创建新的菜单树，MenuNode是一个可以嵌套
//的数据类型，它的定义被单独写在一个文件里，有子菜单的menu和有功能的menu目前是互斥
//的，它要么打开自己的子menu，要么执行Action里的东西

using System;

public static class MenuBuilder
{
    
    public static MenuNode Build()
{
    MenuNode ocrSwitch =new MenuNode{ Title = "开启OCR"};

   ocrSwitch.Function =
    () =>
    {
        if (OCRManager.IsEnabled)
        {
            OCRManager.Stop();

            ocrSwitch.Title =
                "开启OCR";
        }
        else
        {
            bool started =
                OCRManager.StartAsync()
                    .GetAwaiter()
                    .GetResult();

            if (started)
            {
                ocrSwitch.Title =
                    "关闭OCR";
            }
            else
            {
                ocrSwitch.Title =
                    "开启OCR";
            }
        }
    };
    
    return new MenuNode
        {
            Title = "Wheat_Cat OCR Translator",

            Children =
            {
                ocrSwitch,

                new MenuNode//设置
                {
                    Title = "设置",

                    Children =
                    {
                        new MenuNode//初始语言
                        {
                            Title = "初始语言",
                            Function = () =>//功能写入
                            {
                                MenuKits.GeneralBack(MenuKits.ChangeSettings("SourceLanguage","原文语言的设定"),"设置原文语言");
                            }
                        },
                        new MenuNode//目标语言
                        {
                            Title = "目标语言",
                            Function = () =>//功能写入
                            {
                                MenuKits.GeneralBack(MenuKits.ChangeSettings("TargetLanguage","目标语言的设定"),"设置目标语言");
                            }
                        },
                        new MenuNode//模型
                        {
                            Title = "模型",
                            Children =
                            {
                               new MenuNode//模型路径
                                {
                                    Title = "模型路径",
                                    
                                    Function = () =>//功能写入
                                    {
                                        MenuKits.GeneralBack(MenuKits.ChangeSettings("ModelPath","使用的llm模型的路径(仅支持gguf格式)"),"修改模型路径");
                                    }
                                }, 
                                new MenuNode//启动参数
                                {
                                    Title = "启动参数",
                                    Function = () =>//功能写入
                                    {
                                        MenuKits.GeneralBack(MenuKits.ChangeSettings("StartupParameters","llama.cpp的启动参数"),"修改启动参数");
                                    }
                                },
                            }
                        },
                        new MenuNode//快捷键设置
                        {
                            Title = "快捷键",
                            Function = () =>//功能写入
                            {
                                MenuKits.GeneralBack(MenuKits.ChangeSettings("Hotkey","快捷键的设置"),"修改快捷键设置");
                            }
                        },
                        new MenuNode//默认设置
                        {
                        Title = "恢复默认设置",
                        Function = () =>//功能写入
                            {
                             MenuKits.GeneralBack(MenuKits.Repair(),"恢复默认设置");
                            }
                        },
                    }
                },

                new MenuNode//关于制作者
                {
                    Title = "关于制作者",

                    Function = () =>
                    {
                        Console.Clear();
                        Console.WriteLine("OCR Translator");
                        Console.WriteLine("OCR 翻译软件---由 Wheat_Cat 制作");
                        Console.WriteLine(@"██╗    ██╗██╗  ██╗███████╗ █████╗ ████████╗         ██████╗ █████╗ ████████╗
██║    ██║██║  ██║██╔════╝██╔══██╗╚══██╔══╝        ██╔════╝██╔══██╗╚══██╔══╝
██║ █╗ ██║███████║█████╗  ███████║   ██║           ██║     ███████║   ██║   
██║███╗██║██╔══██║██╔══╝  ██╔══██║   ██║           ██║     ██╔══██║   ██║   
╚███╔███╔╝██║  ██║███████╗██║  ██║   ██║   ███████╗╚██████╗██║  ██║   ██║   
 ╚══╝╚══╝ ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝ ╚═════╝╚═╝  ╚═╝   ╚═╝   
                                                                                ");
                        Console.WriteLine("按任意键返回...");
                        Console.ReadKey(true);
                    }
                },

                new MenuNode//退出
                {
                    Title = "退出",
                    IsExit = true
                }
            }
        };
    }
}
/*这里存放添加目录实例的模板，从#后开始复制直到#前可以成功插入
注意！截止到10/2的版本，Children和Function的使用是互斥的
因为enter键只有当这个目录是终目录时才会尝试去调用它的功能，这他妈太弱智了
#
new MenuNode//速记名称
           {
                   Title = "标题",
                   Children = {}
                   Function = () =>//功能写入
                    {
                    这里写功能函数
                    }
               },
#
*/