using System;
using System.IO;
using System.IO.Pipelines;
using System.Text.Json;
using System.Text.Json.Nodes;

public static class SettingsManager
{
    
    private static string SettingsFilePath =Path.Combine(AppContext.BaseDirectory,"settings.json");
    
    public static AppSettings LoadIn()
    {
        
        if ( ! File.Exists(SettingsFilePath))
        {
            Console.WriteLine("Wheat_Cat OCR Translator");
            Console.WriteLine("==============================");
            Console.WriteLine("没有找到 settings.json 这可能是第一次启动或用户配置丢失");
            Console.WriteLine("选择前进将重新创建一个默认的settings.json");
            Console.WriteLine("按任意键前进");
            Console.ReadKey();
            AppSettings defaultSettings = new AppSettings();

            Save(defaultSettings);

            return defaultSettings;
        }
        try
        {
            string json = File.ReadAllText(SettingsFilePath);

            AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(json);

            if (settings == null)
            {
                Console.WriteLine("Wheat_Cat OCR Translator");
                 Console.WriteLine("==============================");
                Console.WriteLine("设置文件为空！");
                Console.WriteLine("你正在使用的是默认设置");
                Console.WriteLine("按任意键前进");
                Console.ReadKey();
                return new AppSettings();
            }

            return settings;
        }
        catch (Exception ex)
        {
            Console.WriteLine("读取 settings.json 时发生错误：");
            Console.WriteLine(ex.Message);

            Console.WriteLine();
            Console.WriteLine("你正在使用的是默认设置");
            Console.WriteLine("按任意键前进");
            Console.ReadKey();
            return new AppSettings();
        }
    }
    // 保存设置
    public static void Save(AppSettings settings)
    {
        string json =
            JsonSerializer.Serialize(settings,new JsonSerializerOptions{ WriteIndented = true});

        File.WriteAllText(SettingsFilePath,json);
    }

    public static void Write(string location, string input)
    {
        JsonObject root;

        if (File.Exists(SettingsFilePath))
        {
            string json = File.ReadAllText(SettingsFilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                root = new JsonObject();
            }
            else
            {
                JsonNode? node = JsonNode.Parse(json);

                if (node is JsonObject obj)
                {
                    root = obj;
                }
                else
                {
                    throw new InvalidDataException("配置文件根节点不是 JSON 对象,这可能意味着配置文件中出现无效语法");
                }
            }
        }
        else
        {
            root = new JsonObject();
        }

        // 如果 location 已存在，就覆盖它的值
        // 如果 location 不存在，就新增这个属性
        root[location] = input;

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string newJson = root.ToJsonString(options);

        File.WriteAllText(SettingsFilePath, newJson);
    }
    public static string Read(string location)
    {
        string json = File.ReadAllText(SettingsFilePath);

        using JsonDocument doc = JsonDocument.Parse(json);

              JsonElement root = doc.RootElement;

        if (root.TryGetProperty(location, out JsonElement value))
            {
                return value.ToString();
            }

        return "【数据为空】"; 
    }
}