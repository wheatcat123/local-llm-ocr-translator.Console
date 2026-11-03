//这里是对于menu来说可能用到的所有功能接口，之后会拓展如何和ai与ocr联结在一起

using System.Runtime.CompilerServices;

public class MenuKits
    {
        public static void GeneralBack(bool ifSucceeded,string operationname = "")
        {
            Console.Clear();
            Console.WriteLine("Wheat_Cat OCR Translator-操作回执");
            Console.WriteLine("==============================");
            if (ifSucceeded)
            {Console.WriteLine("操作{0}成功！点击任意键回到上级目录", operationname);}//这个{0}填什么好呢？
            else{Console.WriteLine("操作{0}失败或已取消。点击任意键回到上级目录", operationname);}
            Console.ReadKey(true);
        }
        public static bool ChangeSettings(string location, string discription ="")
        {
            
            string initialvalue = SettingsManager.Read(location);
            Console.Clear();
            Console.WriteLine("Wheat_Cat OCR Translator");
            Console.WriteLine("你正在查看的是配置{0}，其描述是“{1}”",location,discription);
            Console.WriteLine("输入值在下方再按Enter可以修改，不输入值直接按Enter可以取消修改并退出");
            Console.WriteLine("==============================");
            Console.WriteLine("这个配置的初始值是{0}",initialvalue);
            Console.WriteLine("请在下方输入新的值：");
            string? next = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(next)){return false;}
            else SettingsManager.Write(location,next);return true;
        }
        public static void StartNewProcess(){}
        public static bool Repair()
        {
            Console.Clear();
            Console.WriteLine("Wheat_Cat OCR Translator");
            Console.WriteLine("==============================");
            Console.WriteLine("这个功能会覆盖你更改过的所有软件配置，这个操作不可撤销");
            Console.WriteLine(@"建议在这之前保存你的配置文件的副本，
你想好了吗？（按Y确认，按其他按键取消）");
            ConsoleKeyInfo key= Console.ReadKey();
            if (key.Key == ConsoleKey.Y)
            {AppSettings defaultSettings = new AppSettings();
            SettingsManager.Save(defaultSettings);return true;}
            else{return false;}
            
        }









    }