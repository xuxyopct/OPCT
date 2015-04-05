环境：工具在Oracle 10g、11G、12C上使用测试通过； 
使用步骤：
1.软件基于.Net Framework 3.5开发和编译，如没有安装，请到微软网站下载安装.Net Framework 3.5(http://www.microsoft.com/zh-cn/download/details.aspx?id=21)或安装更高版本。
2.打开app.config配置连接Oracle数据库的服务名(需要安装Oracle 8.1以上的Oracle客户端，配置TNS，服务名与Oracle配置的TNS名称一致)及登录用户名、密码。如下：
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <appSettings>
        <add name="ConnectionString" connectionString="Data Source=orcl;User ID=system;Password=oracle;Unicode=True" providerName="System.Data.OracleClient"  />
        <add name="ConnectionString" connectionString="Data Source=ora10g;User ID=system;Password=oracle123;Unicode=True" providerName="System.Data.OracleClient"  />
        <add name="ConnectionString" connectionString="Data Source=racdb1;User ID=system;Password=oracle;Unicode=True" providerName="System.Data.OracleClient"  />
        <add name="ConnectionString" connectionString="Data Source=racdb2;User ID=system;Password=oracle;Unicode=True" providerName="System.Data.OracleClient"  />                                                                                     
    </appSettings>
</configuration>
3.选择数据源
 
4.选择需要获取的信息，单击“获取信息”
 
软件功能：
1.查询Oracle实例名、状态、版本等
2.Oracle连接数
3.SGA信息
4.PGA信息
5.数据库使用
6.表空间使用情况
7.临时文件信息
8.临时文件使用情况
9.表死锁情况
10.锁表数量
11.长时间使用的SQL语句
12.事例的等待
13.回滚段的争用情况
14.表空间I/O比例
15.文件系统I/O比例
16.用户下所有索引
17.SGA命中率
18.SGA字典缓冲区命中率
19.SGA共享缓冲区命中率
20.SGA重做日志缓冲区命中率
21.内存和硬盘排序比率
22.正在运行的SQL语句
23.字符集
24.MTS
25.碎片程度高的表
26.使用CPU多的用户
27.KILL用户会话
28.一次性改变和获取统计
29.重做日志使用统计
30.系统统计设置
31.网络I/O活动情况
32.进程使用CPU统计
33.控制文件放置
34.数据库属性
35.常用参数
36.SGA共享池自由内存数量
37.SGA共享池效率检查
38.SGA内存单元大小
39.SGA内存分配大小
40.还原操作用户
41.异步I/O开启
42.压缩表
43.系统状态
44.日志组信息
45.系统Parameter
