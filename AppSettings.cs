//这里存放的是这个软件的几乎所有可以定制的数据，它会被实例成一个文件。但并不意味着它们都有渠道在软件内部
//更改，做菜单树虽然不算麻烦，但是之后会有很多不同部件分别的配置信息，我不可能
//让他们全都可以自定义

public class AppSettings
{
    public string LlamaPath {get;set;}="";//目前没有打包所以LLaMA在我自己电脑上
    public string ModelPath { get; set; } = "";
    public string StartupParameters { get; set; }= "";
    public string SourceLanguage { get; set; } = "English";
    public string TargetLanguage { get; set; } = "Chinese";
    public string Hotkey { get; set; } = "Ctrl+Shift+T";
    public string ForegroundColor { get; set; } = "White";
    public string BackgroundColor { get; set; } = "Black";
    public int FontSize { get; set; } = 16;
    public bool StartupWithWindows { get; set; } = false;
}