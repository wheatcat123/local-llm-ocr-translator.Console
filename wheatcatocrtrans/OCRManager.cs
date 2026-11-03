using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

public static class OCRManager
{
    public static void CloseAllOverlays()
    {
        foreach (TranslationOverlayForm overlay in Overlays)
        {
            try
            {
                if (!overlay.IsDisposed)
                {
                    overlay.Close();
                }
            }
            catch
            {
            }
        }

        Overlays.Clear();
    }
    private static readonly List<TranslationOverlayForm> Overlays =
    new List<TranslationOverlayForm>();
    public static bool IsEnabled
    {
        get;
        private set;
    }

    private static bool IsStarting;


    public static async Task<bool> StartAsync()
    {
        if (IsEnabled)
        {
            return true;
        }

        if (IsStarting)
        {
            return false;
        }

        IsStarting = true;

        try
        {
            AppSettings settings =
                SettingsManager.LoadIn();


            Console.WriteLine();
            Console.WriteLine("正在启动翻译服务...");


            bool started =
                LlamaManager.LlamaStart(
                    settings);

            if (!started)
            {
                Console.WriteLine("LLM 启动失败。");

                return false;
            }


            bool online =
                await LlamaManager.LlamaWait();

            if (!online)
            {
                Console.WriteLine("LLM 加载失败。");

                LlamaManager.LlamaStop();

                return false;
            }


            IsEnabled = true;

            Console.WriteLine("OCR 翻译服务已经启动。");

            return true;
        }
        finally
        {
            IsStarting = false;
        }
    }


    public static void Stop()
    {
        if (!IsEnabled &&
            !IsStarting)
        {
            return;
        }


        IsEnabled = false;


        LlamaManager.LlamaStop();


        Console.WriteLine("OCR 翻译服务已经关闭。");
    }


    public static async Task TranslateOnceAsync()
    {
        if (!IsEnabled)
        {
            return;
        }


        using RegionSelectorForm selector =
            new RegionSelectorForm();


        DialogResult result =
            selector.ShowDialog();


        if (result != DialogResult.OK)
        {
            return;
        }


        Rectangle region =
            selector.SelectedRegion;


        using Bitmap screenshot =
            ScreenCaptureService.Capture(
                region);


        AppSettings settings =
            SettingsManager.LoadIn();


        OcrService ocr =
            new OcrService();


        string text =
            ocr.Recognize(
                screenshot,
                "eng+chi_sim");


        MenuManager.SetLastOcrResult(
            text);


        if (string.IsNullOrWhiteSpace(text))
        {
            MessageBox.Show(
                "没有识别到文字。");

            return;
        }


        string translation =
            await LlamaManager.Translate(
                text,
                settings);


        if (string.IsNullOrWhiteSpace(
            translation))
        {
            MessageBox.Show(
                "LLM 没有返回翻译结果。");

            return;
        }


        TranslationOverlayForm overlay =
        new TranslationOverlayForm(
            region,
            translation,
            settings.FontSize);

        Overlays.Add(overlay);

        overlay.FormClosed +=(sender, e) =>
        {
            Overlays.Remove(overlay);
        };

        overlay.Show();
    }
}
