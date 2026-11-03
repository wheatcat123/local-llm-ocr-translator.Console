<div align="center">

# local-llm-ocr-translator.Console

> 【截止到26/11/1这个项目还没有完工】用于配合本地部署的llama.cpp模型的极简截图翻译软件。

[![Stars](https://img.shields.io/github/stars/wheatcat123/local-llm-ocr-translator.Console?style=social)](https://github.com/wheatcat123/local-llm-ocr-translator.Console/stargazers)
[![Forks](https://img.shields.io/github/forks/wheatcat123/local-llm-ocr-translator.Console?style=social)](https://github.com/wheatcat123/local-llm-ocr-translator.Console/network/members)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/wheatcat123/local-llm-ocr-translator.Console/pulls)

[项目简介](#项目简介) · [快速开始](#快速开始) · [使用说明](#使用说明) · [配置文件](#配置文件) · [报告问题](https://github.com/wheatcat123/local-llm-ocr-translator.Console/issues)

</div>

---

## 项目简介

该项目的主程序为Windows控制台应用，属初学者自学实验目的，目前功能完备，但用户体验设计仍在开发阶段

- **功能流程**：按下全局热键，在屏幕上框选一块区域，程序截图后交给Tesseract识别文字，文字发送到本机的 llama-server 翻译，译文显示在选区位置的浮窗中
- **程序形态**：控制台窗口负责菜单操作，托盘图标负责显示或隐藏控制台，两者与浮窗在同一个进程里运行
- **适用场景**：在能部署本地LLM的条件下，有以最低的性能开销实现OCR本地截图翻译需求时
- **不适用场景**：不能本地运行LLM的条件下，不支持.NET运行时的条件下。
- **当前版本**：`v0.0.1`
- **作者**：Wheat_Cat（[@wheatcat123](https://github.com/wheatcat123)）

---

## 特性

- **控制台菜单**：方向键移动光标，`Enter` 确认，`Esc` 返回上一级，菜单树定义在 `ConsoleCore/MenuBuilder.cs`
- **全局热键**：默认 `Ctrl+Shift+T`，由一个隐藏的WinForms窗口通过 `RegisterHotKey` 注册
- **框选截图**：按下热键后全屏出现半透明遮罩，按住左键拖出矩形，松开后按屏幕坐标截图，按 `Esc` 或选区不足 5×5 像素时取消
- **本地OCR**：使用 Tesseract 5.5.2，识别语言为 `eng+chi_sim`，语言包放在输出目录的 `tessdata` 中
- **本地翻译**：由程序启动 `llama-server.exe`，轮询 `http://127.0.0.1:8080/health` 直到模型上线，再向 `/v1/chat/completions` 提交翻译请求
- **译文浮窗**：浮窗覆盖在选区位置上，黑底白字，可以移动、调整大小、最大化，按 `Esc` 关闭
- **托盘控制**：托盘菜单可以显示控制台、隐藏控制台、退出程序，双击托盘图标显示控制台
- **配置文件**：`settings.json` 与可执行文件放在同一目录，首次启动时自动创建
- **离线运行**：OCR与翻译都在本机完成，程序只访问 `127.0.0.1`

---

## 技术栈

- **语言与运行时**：C#，.NET 10（`net10.0-windows`），启用可空引用与隐式 using
- **界面**：Windows Forms，高DPI模式为 `PerMonitorV2`
- **OCR**：`TesseractOCR 5.5.2` NuGet包，随包携带 `tesseract55.dll` 与 `leptonica-1.85.0.dll`
- **翻译**：llama.cpp 的 `llama-server.exe`，通过HTTP访问 `127.0.0.1:8080`
- **配置**：`System.Text.Json`
- **系统调用**：`user32.dll`（热键注册、窗口显示、系统菜单项删除）、`kernel32.dll`（获取控制台窗口句柄）

---

## 环境要求

- Windows 10 或更高版本，程序中包含WinForms界面，未做跨平台处理
- .NET 10 SDK，或者运行已发布程序时使用对应的桌面运行时
- llama.cpp 的Windows版本，包含 `llama-server.exe`
- 一个GGUF格式的模型文件
- `tessdata` 目录中的 `eng.traineddata` 与 `chi_sim.traineddata`，仓库中已经包含这两个文件

---

## 快速开始

```bash
git clone https://github.com/wheatcat123/local-llm-ocr-translator.Console.git
cd local-llm-ocr-translator.Console
dotnet build
```

启动程序：

```bash
dotnet run --project wheatcatocrtrans
```

也可以直接运行 `wheatcatocrtrans\bin\Debug\net10.0-windows\wheatcatocrtrans.exe`。

首次启动时程序在可执行文件所在目录创建 `settings.json`，其中 `LlamaPath` 与 `ModelPath` 默认为空，需要先填写：

1. `LlamaPath` 没有菜单入口，直接编辑 `settings.json`，填入 `llama-server.exe` 的完整路径
2. 在控制台菜单中进入 `设置 → 模型 → 模型路径`，填入GGUF模型文件的完整路径
3. 需要卸载层数或其他启动参数时，进入 `设置 → 模型 → 启动参数` 填写，例如 `-ngl 99 -c 4096`

配置完成后在控制台菜单选择 `开启OCR`，程序启动 llama-server 并等待模型上线，最长等待120秒。菜单项变成 `关闭OCR` 之后，按下 `Ctrl+Shift+T` 框选屏幕区域即可得到译文。

---

## 使用说明

控制台窗口：

- `↑` `↓` 移动光标，`Enter` 确认，`Esc` 返回上一级
- 顶部显示当前的OCR状态，底部显示提示信息与最近一次OCR的文字
- `设置` 下可以修改原文语言、目标语言、模型路径、启动参数与快捷键，`恢复默认设置` 会用默认配置覆盖当前的 `settings.json`
- `关于制作者` 显示作者信息，`退出` 结束程序
- 控制台的关闭按钮已被移除，退出程序需要通过控制台菜单或托盘菜单

托盘图标：

- `显示控制台`、`隐藏控制台`、`退出程序`
- 双击图标显示控制台

浮窗：

- 译文出现在选区的位置，可以拖动标题栏移动、拖动边缘调整大小
- 按 `Esc` 关闭浮窗

---

## 配置文件

`settings.json` 与可执行文件放在同一目录，程序使用 `System.Text.Json` 反序列化，缺少字段时使用下表的默认值。

| 字段 | 默认值 | 说明 |
| --- | --- | --- |
| `LlamaPath` | 空 | `llama-server.exe` 的完整路径，没有菜单入口 |
| `ModelPath` | 空 | GGUF模型文件的完整路径 |
| `StartupParameters` | 空 | 追加在 `-m "<模型路径>"` 之后的启动参数 |
| `SourceLanguage` | `English` | 原文语言，当前只保存在配置中，识别语言固定为 `eng+chi_sim` |
| `TargetLanguage` | `Chinese` | 目标语言，写入翻译请求的系统提示词 |
| `Hotkey` | `Ctrl+Shift+T` | 快捷键，当前热键窗口固定注册 `Ctrl+Shift+T` |
| `ForegroundColor` | `White` | 译文浮窗前景色，当前浮窗固定使用白色 |
| `BackgroundColor` | `Black` | 译文浮窗背景色，当前浮窗固定使用黑色 |
| `FontSize` | `16` | 译文浮窗字号 |
| `StartupWithWindows` | `false` | 开机自启，尚未实现 |

翻译请求的参数在 `Llama/LlamaManager.cs` 中固定为 `stream = false`、`cache_prompt = false`、`temperature = 0.2`、`max_tokens = 512`，系统提示词要求模型只输出译文，不解释、不加引号。

---

## 项目结构

```text
Wheat_Cat_OCR_Translator
├─ wheatcatocrtrans.slnx              解决方案文件
├─ .vscode/settings.json              指定默认解决方案
└─ wheatcatocrtrans
   ├─ Program.cs                      程序入口，启动控制台菜单线程与WinForms消息循环
   ├─ AppSettings.cs                  配置数据类
   ├─ SettingsManager.cs              settings.json 的读取、写入与默认配置创建
   ├─ AppShutdown.cs                  统一退出入口，先停OCR再结束应用
   ├─ OCRManager.cs                   OCR开关状态、单次翻译流程与浮窗列表
   ├─ OCRservice.cs                   Tesseract引擎封装
   ├─ ScreenCaptureService.cs         按屏幕矩形截图
   ├─ RegionSelectorForm.cs           全屏框选窗口
   ├─ TranslationOverlayForm.cs       译文浮窗
   ├─ HotkeyReceiverForm.cs           隐藏窗口，注册与接收全局热键
   ├─ TrayApplicationContext.cs       托盘图标、托盘菜单与退出逻辑
   ├─ ConsoleWindowController.cs      控制台窗口的显示、隐藏与关闭按钮移除
   ├─ ConsoleCore
   │  ├─ MenuManager.cs               菜单渲染与按键循环
   │  ├─ MenuBuilder.cs               菜单树的定义
   │  ├─ MenuNode.cs                  菜单节点的数据结构
   │  └─ MenuKits.cs                  菜单可以调用的通用操作
   ├─ Llama
   │  └─ LlamaManager.cs              启动与关闭 llama-server、健康检查、翻译请求
   └─ tessdata                        Tesseract语言包
```

---

## 已知限制

- 全局热键固定为 `Ctrl+Shift+T`，配置项 `Hotkey` 还没有接入注册逻辑，热键被其他程序占用时程序会提示注册失败
- 识别语言固定为 `eng+chi_sim`，配置项 `SourceLanguage` 还没有参与识别
- 配置项 `ForegroundColor` 与 `BackgroundColor` 还没有生效
- 配置项 `StartupWithWindows` 还没有实现
- 菜单节点不能同时拥有子菜单和功能，带子菜单的节点只能用于进入下一级
- llama-server 的地址与端口固定为 `127.0.0.1:8080`
- 翻译请求没有设置超时，llama-server 无响应时界面会一直等待
- 等待模型上线最多120次，每次间隔1秒，超过后判定启动失败并关闭 llama-server

---

## 🗺️ 路线图

- [x] v0.0.0 核心功能
- [x] v0.0.1 可运行的第一个版本
- [ ] v0.0.2 完整用户界面和配置功能
- [ ] v0.1.0 TUI用户界面适配

欢迎在 [Issues](https://github.com/wheatcat123/local-llm-ocr-translator.Console/issues) 中讨论新特性。

---

## 🤝 贡献指南

我们欢迎任何形式的贡献，包括但不限于：报告 Bug、提出建议、完善文档、提交代码。

1. **Fork** 本仓库并克隆到本地
2. 创建分支：`git checkout -b feat/your-feature`
3. 提交改动：`git commit -m "feat: add some feature"`（遵循 [Conventional Commits](https://www.conventionalcommits.org/zh-hans/)）
4. 推送分支：`git push origin feat/your-feature`
5. 提交 **Pull Request**

提交前请确保构建通过：

```bash
dotnet build
```

### 开发环境

```bash
git clone https://github.com/wheatcat123/local-llm-ocr-translator.Console.git
cd local-llm-ocr-translator.Console
dotnet build
dotnet run --project wheatcatocrtrans
```

程序需要本机已经准备好 `llama-server.exe` 与GGUF模型，编译阶段不检查这两个路径。

---

## 📜 行为准则

本项目采用 [Contributor Covenant](https://www.contributor-covenant.org/zh-cn/) 行为准则。参与本项目即表示你同意遵守其条款。

---

## 📄 许可证

本项目基于 **MIT License** 开源发布，允许商业使用、修改、分发，但需保留版权声明与许可声明。

```text
MIT License

Copyright (c) 2026 Wheat_Cat

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
