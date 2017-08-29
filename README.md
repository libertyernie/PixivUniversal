## Pixiv通用客户端

### 简介
 Pixiv通用客户端旨在构建在WindowsPC、Windows移动端（UWP）、安卓系统设备和传统Win32平台上的统一的Pixiv服务浏览体验；该项目分成五个部分：PixivUWP、PixivAndroid、PixivWPF、Pixeez（Pixiv第三方API封装）和TileBackground（动态磁贴后台组件）。

### 仓库规范
 非项目组成员将无法编辑任何内容；对于开发人员的仓库规范如下：
 - 任何形式的修改必须以建立issue开始，并关联相关的分支；
 - 任何形式的修改不得直接进行在master分支；
 - 任何新特性分支以feature_开头；
 - 任何Bug修复分支以bugfix_开头；
 - 归并请求建议指定测试人员；
 - 规定非强制性，但是建议的，请自由进行贡献；


### 编译

使用 Visual Studio 2017 或更高版本打开该项目的sln文件

如果无法编译，您可能需要安装15063版本的 UWP SDK 以及 [Microsoft Store Services SDK](http://aka.ms/store-em-sdk)

### 许可

 本项目遵从 [![GPLv2](https://img.shields.io/badge/license-GPLv2-blue.svg?style=flat)](LICENSE.md) 许可协议开源