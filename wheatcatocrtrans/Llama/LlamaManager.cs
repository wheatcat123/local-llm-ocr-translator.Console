//这里存放的是和llama进程有关的管理器，它提供打开关闭llama，打开或者关闭模型
//，以及从jason传入启动参数，将输入链接至模型，和将输出链接至客户端的功能
//这本质上是一个菜单的功能吧？我会把它传到MenuKits里面
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

public class LlamaManager

{
    
    private static Process? Llamaserver;
    private static readonly HttpClient LlamaClient = new HttpClient();
    
    public static bool LlamaStart(AppSettings settings)
    {
        if (!File.Exists(settings.LlamaPath))
        {
            Console.WriteLine("找不到 llama：");

            Console.WriteLine(settings.LlamaPath);

            Console.WriteLine("按任意键继续");

            Console.ReadKey();

            return false;
        }


        if (!File.Exists(settings.ModelPath))
        {
            Console.WriteLine("找不到 GGUF 模型：");

            Console.WriteLine(settings.ModelPath);

            Console.WriteLine( "按任意键继续");

            Console.ReadKey();

            return false;
        }


        // 防止重复启动
        if (Llamaserver != null &&!Llamaserver.HasExited)
        {
            return true;
        }


        string arguments =
            "-m \"" +
            settings.ModelPath +
            "\" " +
            settings.StartupParameters;


        ProcessStartInfo startInfo =
            new ProcessStartInfo();


        startInfo.FileName =
            settings.LlamaPath;


        startInfo.Arguments =
            arguments;


        startInfo.UseShellExecute =
            false;


        // 不让 llama 日志进入主 Console
        startInfo.CreateNoWindow =
            true;


        // 接管 stdout
        startInfo.RedirectStandardOutput =
            true;


        // 接管 stderr
        startInfo.RedirectStandardError =
            true;


        Llamaserver =
            Process.Start(startInfo);


        if (Llamaserver == null)
        {
            Console.WriteLine("llama-server 启动失败！");

            return false;
        }


        // 持续读取 llama 日志
        // 第一版直接丢弃
        Llamaserver.OutputDataReceived +=
            (sender, e) =>
            {};


        Llamaserver.ErrorDataReceived +=
            (sender, e) =>
            {};


        Llamaserver.BeginOutputReadLine();

        Llamaserver.BeginErrorReadLine();


        return true;
    }
    
    public static bool LlamaStop()
    {
        if (Llamaserver == null)
        {
            return true;
        }


        try
        {
            if (!Llamaserver.HasExited)
            {
                Llamaserver.Kill();

                Llamaserver.WaitForExit(5000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("关闭 llama 时发生错误：");

            Console.WriteLine(ex.Message);

            return false;
        }
        finally
        {
            Llamaserver.Dispose();
            Llamaserver = null;
        }
        return true;
    }
    public static async Task<bool> LlamaIsOnline()
    {
        try
        {
            HttpResponseMessage response = await LlamaClient.GetAsync("http://127.0.0.1:8080/health");

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
    public static async Task<bool> LlamaWait()
    {
        Console.WriteLine("正在等待模型加载...");

        for (int i = 0; i < 120; i++)
        {
            bool online = await LlamaIsOnline();
            Console.Clear();
            Console.WriteLine("正在请求模型 {0}/120",i);
            if (online)
            {
                Console.WriteLine("模型已经上线！");
                return true;
            }

            Console.WriteLine("模型还没有准备好，1秒后重新检查...");

            await Task.Delay(1000);
        }

        Console.WriteLine("等待模型超时。");

        return false;
    }
    public static async Task<string> Translate(string ocrText,AppSettings settings)

    {
        string systemPrompt =
            "You are a professional translation engine.\n" +
            "Translate the user's text to" +
            settings.TargetLanguage +
            ".\n" +
            "Output only the translation.\n" +
            "Do not explain anything.\n" +
            "Do not add quotation marks.";


        var requestObject = new
        {
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = systemPrompt
                },

                new
                {
                    role = "user",
                    content = ocrText
                }
            },

            stream = false,

            cache_prompt = false,

            temperature = 0.2,

            max_tokens = 512
        };


        string json = JsonSerializer.Serialize(requestObject);

        StringContent content = new StringContent(json,Encoding.UTF8,"application/json");

        HttpResponseMessage response =await LlamaClient.PostAsync("http://127.0.0.1:8080/v1/chat/completions",content);


        response.EnsureSuccessStatusCode();


        string responseJson = await response.Content.ReadAsStringAsync();


        using JsonDocument document = JsonDocument.Parse(responseJson);


        string result =
            document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()
                ?? "";


        return result.Trim();
    }

    
 }







