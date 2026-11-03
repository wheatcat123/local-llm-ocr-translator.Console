//这里按理来说应该存放一个有关OCR引擎应用有关的管理器，用来接受配置，传递截图信息
//，而且这里应该有个和截图操作有关的东西，但是我不知道那是什么玩意儿😭
// 以后的开发全都不能只用控制台，只能含泪vibecoding了这下
using System;
using System.IO;
using TesseractOCR;
using TesseractOCR.Enums;

public class OcrService
{
    private readonly string TessDataPath;

    public OcrService()
    {
        TessDataPath = Path.Combine(
            AppContext.BaseDirectory,
            "tessdata"
        );
    }

   public string Recognize(
    Bitmap bitmap,
    string language)
{
    using MemoryStream stream =
        new MemoryStream();


    bitmap.Save(
        stream,
        System.Drawing.Imaging.ImageFormat.Png);


    byte[] bytes =
        stream.ToArray();


    using TesseractOCR.Pix.Image image =
        TesseractOCR.Pix.Image.LoadFromMemory(
            bytes);


    using Engine engine =
        new Engine(
            TessDataPath,
            language,
            EngineMode.Default);


    using Page page =
        engine.Process(image);


    return page.Text.Trim();
}
    
}
