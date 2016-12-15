## Pixeez使用说明

### 简介
 Pixeez是Pixiv相关API的第三方.NET封装，用以实现与Pixiv服务互换信息。

### 对象
 这里介绍此API封装涉及到的大部分数据类型，这些类型的定义位于Pixeez.Objects名称空间下。
 - Authorize类：公开的，用于储存已经被解析的Json授权结果；
 - Feed类：公开的，用于储存新动态数据；
 - Pagination类：公开的，用于储存当前页面的页码信息；
 - ProfileImageUrls类：公开的，用于储存不同尺寸头像的URL；
 - Rank类和RankWork类：公开的，用于储存排名信息；
 - UserStats类：公开的，用于储存用户的身份；
 - Contacts类：公开的，用于储存用户联系方式；
 - Workspace类：公开的，用于储存工作环境信息；
 - Profile类：公开的，用于储存用户信息；
 - User类：公开的，用于储存用户；
 - UsersFavoriteWork类：公开的，用于储存最喜爱的作品信息；
 - UsersWork类：公开的，用于储存用户作品信息；
 - Work.cs内部定义的类：全部都是公开的，用于储存作品相关信息；

### 授权API封装
 - Auth.AuthorizeAsync：异步，公开，返回Task<Token>，用于登录并获取Token；
 - Auth.AuthorizeWithAccessToken：单步，公开，返回Token，用于恢复登录状态并重新获取Token；

### 主要API封装
 - Token.AccessToken：属性，不公开可写，公开可读，用于储存Token的字符串表达；
 - Token.SendRequestAsync：公开，异步，用于发送自定义请求；
 - Token.GetWorksAsync：公开，异步，返回Task<List<Work>>，用于获取作品列表；
 - Token.GetUsersAsync：公开，异步，返回Task<List<User>>，用于获取用户；
 - Token.GetMyFeedsAsync：公开，异步，返回Task<List<Feed>>，用于获取对应Token的动态；
 - Token.GetMyFavoriteWorksAsync：公开，异步，返回Task<Paginated<UsersFavoriteWork>>，用于获取按页导航的对应Token的最喜爱的作品；
 - Token.AddMyFavoriteWorksAsync：公开，异步，返回Task<List<UsersFavoriteWork>>，用于添加最喜爱的作品；
 - Token.DeleteMyFavoriteWorksAsync：公开，异步，返回Task<List<UsersFavoriteWork>>，用于删除最喜爱的作品；
 - Token.GetMyFollowingWorksAsync：公开，异步，返回Task<Paginated<UsersWork>>，用于获取对应Token处于Following状态的作品；
 - Token.GetUsersWorksAsync：公开，异步，返回Task<Paginated<UsersWork>>，用于获取用户作品分页导航的列表；
 - Token.GetUsersFavoriteWorksAsync：公开，异步，返回Task<Paginated<UsersFavoriteWork>>，用于获取用户最喜爱的作品分页导航的列表；
 - Token.GetUsersFeedsAsync：公开，异步，返回Task<List<Feed>>，用于获取用户动态；
 - Token.GetRankingAllAsync：公开，异步，返回Task<Paginated<Rank>>，用于获取热度排行的作品分页导航的列表；
 - Token.SearchWorksAsync：公开，异步，返回Task<Paginated<Work>>，用于获取搜索结果分页导航的列表；
 - Token.GetLatestWorksAsync：公开，异步，返回Task<Paginated<Work>>，用于获取最近的作品分页导航的列表；

### 实例
~~~cs
class Program
{
    static void Main(string[] args)
    {
        Task.Run(async () => await Program.PixivDemo()).Wait();
    }

    static async Task PixivDemo()
    {
        // 请求授权
        var tokens = await Pixeez.Auth.AuthorizeAsync("username", "password");

        var work = await tokens.GetWorksAsync(51796422);
        var user = await tokens.GetUsersAsync(11972);
        var myfeeds = await tokens.GetMyFeedsAsync(true);
        var usersWorks = await tokens.GetUsersWorksAsync(11972);
        var usersFavoriteWorks = await tokens.GetUsersFavoriteWorksAsync(11972);
        var ranking = await tokens.GetRankingAllAsync();
        var search = await tokens.SearchWorksAsync("フランドール・スカーレット", mode: "tag");
    }
}
~~~

### 说明
 本API的C#封装基于原工程Pixeez改进，感谢原作者cucmberium（日本）对该工程的贡献以及对我们的授权。

### 许可
 本项目遵从MIT许可协议开源：

 Copyright (C) (2016 Project-PixivUniversal)

 Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
