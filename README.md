# SimpleFrameWork
+ 简单的快速开发框架，UI采用了**FairyGUI**，包含**DoTween**插件
+ 读配置表使用了Excel转CSV格式，项目中包含了一个读CSV的插件
+ 跟unity版本无关，遇到版本可以忽略（也不要提交版本信息）

## Factory
```C#
public T GetFactory<T>() where T : FactoryBase//获取工厂
```
## UI
```C#
public void OpenPanel(string panelName,UIType type,UIMode mode,object data = null);//打开ui界面
public void ClosePanel(string panelName,object data = null);//关闭界面（返回上个界面）
```
