public class MenuManager
{
    public static event Action? ExitRequested;
    public static string LastOcrResult = "暂无OCR结果";

    public static void SetLastOcrResult(string text)
    {
        LastOcrResult = text
            .Replace("\r", " ")
            .Replace("\n", " ")
            .Trim();
    }

    public static void Load(MenuNode rootmenu)
    {
        MenuNode menus = rootmenu;
        string notice = "";
        int pointer = 0;
        Stack<MenuNode> history = new();
        bool running = true;

        //定义menus作为内部变量，
        // notice为空，
        // 光标位置为0,
        // 新建用于存放MenuNode的栈，
        // 运行为是

        while (running)
        {
            
            // 主页面标题
            Console.Clear();
            Console.WriteLine("Wheat_Cat OCR Translator");
            Console.WriteLine("==============================");
            if (OCRManager.IsEnabled)
            {
                Console.WriteLine("OCR状态：已开启");
            }
            else
            {
                Console.WriteLine("OCR状态：未开启");
            }
            Console.WriteLine();

            //光标位置显示
            for (int i = 0; i < menus.Children.Count; i++)
            {
                MenuNode child = menus.Children[i];
                if (pointer == i){Console.WriteLine(">{0}",child.Title);}
                else Console.WriteLine(" {0}",child.Title);
            }

            //下菜单栏
            Console.WriteLine();
            Console.WriteLine("[↑ ↓]移动 [Enter]确认 [Esc]返回上一级");
            Console.WriteLine("==============================");
            Console.WriteLine("提示：{0}",notice);
            Console.WriteLine("最近OCR：{0}", LastOcrResult);
            // 按键和操作
            ConsoleKeyInfo key = Console.ReadKey(true); 
            
            //向上箭头使光标值降低
            if (key.Key == ConsoleKey.UpArrow)
            {
                notice = "";
                pointer--;

                if (pointer < 0)
                {
                    pointer = menus.Children.Count - 1;
                }
            }
            //向下箭头使光标值升高
            else if (key.Key == ConsoleKey.DownArrow)
            {
                notice = "";
                pointer++;

                if (pointer >= menus.Children.Count)
                {
                    pointer = 0;
                }
            }
            //Enter时，先检查目前的目录是否为空
            //选中当前光标所在的子目录
            //如果是叶子目录，执行它的功能或者退出
            //如果它有子目录，把当前的目录压进栈里，将选中的子目录赋给menus
            else if (key.Key == ConsoleKey.Enter)
            {
               if (menus.Children.Count == 0){notice = "当前的目录为空且没有功能，检查编写！";continue;}
               if (menus.Children.Count > 0)
                {
                    notice = "";
                    MenuNode selected = menus.Children[pointer];

                    if (selected.IsLeaf)
                    {
                        if (selected.IsExit)
                        {
                            running = false;
                            ExitRequested?.Invoke();
                            Console.Clear();
                            Console.WriteLine("Wheat_Cat OCR Translator");
                            Console.WriteLine("==============================");
                            Console.WriteLine("OCR Translator 已经退出");
                            Console.WriteLine("==============================");
                        }
                        else {selected.Function?.Invoke();}
                    }
                    else
                    {
                        history.Push(menus);
                        menus = selected;
                        pointer = 0;
                    }
                }
            }
            // 返回上一级
            else if (key.Key == ConsoleKey.Escape)
            {
            
                if (history.Count > 0)
                {
                    notice = "";
                    menus = history.Pop();
                    pointer = 0;
                }
                else
                {
                    notice = "已经是根目录！";
                }
            }
        }




    
    }


}
