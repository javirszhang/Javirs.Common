# Javirs.Common
v2.8.0  2019年7月18日
1. 删除一些无用的类
2. 增加netstandard2.0框架

v2.7.0  2019年6月8日
1. 优化HttpHelper类
2. 增加一个人民币CNY结构（struct)
3. 增加一个对象转字典的扩展方法（obj.ToDictionry()）

v2.6.28 2019年5月22日
1. 新增一个CustomizeException类，所有自定义异常类都从此类派生
2. 新增AES加解密类
3. 新增以字符串为键按asc值升序排列的字典类
4. 新增安全随机Key生成类，对应Java语言的SecureRandomKey类

v2.6.27 2019年5月6日
1. 修复RSA使用PEM密钥字符串无法初始化的BUG
2. 修复JsonObject对象在Json字符串包含转义符无法解析的BUG

v2.6.23 2018年6月19日
1. 解决PEM密钥文件解析ASN.1编码的密钥问题

v2.6.14 2017年5月15日
1. 增强DataTable转换dynamic类型功能
2. 修正Json序列化的时间格式问题
3. 修复时间戳BUG（时区不同的问题）

v2.6.13 2016年12月11日
1. 完善JsonObject对象，解析复杂Json格式

v2.6.12 2016年12月08日
1. 增加SignProvider模块，加密、签名、验签模块
2. 删除Log4Javirs模块
3. 增加DataTable转换为dynamic动态对象扩展方法
4. RSA证书对象增加获取证书ID[GetCertificateID()]的方法

v2.6.7  2015年12月29日
1. 增加百度eCharts图表支持

v2.6.6  2015年12月25日
1. 工具类增加一个将枚举项做HTML标签返回的方法

v2.6.5  2015年11月11日
1. 枚举生产select控件增加html属性参数

v2.6.4  2015年11月11日
1. 补全NameValuePair对象
2. 身份证验证对象修复一个BUG（当输入错误的身份证号码时出现字典异常）

v2.6.3  2015年11月07日
1. 扩展方法类增加一个html文本中包含的远程图片保存到本地的功能

v2.6.2  2015年11月01日
1. 导出EXCEL增加NPOIExcelBookAttribute特性，包含属性的表头定义以及排序

v2.6.0  2015年9月23日
1. 增加mvc下Html扩展方法生成Select，增加默认Option
2. security命名空间增加一个Pem证书解析类，对接其他语言OpenSSL的PEM证书RSA加解密

v2.5.7  2015年8月3日
1. 删除Javirs.Common.Json的过期方法与特性类


v2.5.6  2015年7月24日
1. Javirs.Common.Utils工具类增加获取本机IP方法

v2.5.5  2015年7月23日
1. Javirs.Common.Net.HttpHelper增加对https的访问功能

v2.5.4  2015年7月1日
1. 增加一个转换Nullable<DateTime>的string类型扩展方法

v2.5.3  2015年6月8日
1. HTTP POST对象时签名对ISignatureAttribute字段不排序

v2.5.2  2015年5月30日
1. 增加一个WebService基类
2. 增加一个AppRequestHeader的SoapHeader类

v2.5.1  2015年5月21日
1. 增加MVC的多按钮提交表单的选择器特性

v2.5.0  2015年5月21日
1. 增加导出Excel的功能，使用NPOI生成excel文件
2. 对HtmlHelper增加多一个按枚举生成Select的重载

v2.4.0  2015年5月16日
1. 删除对Common.dll程序集的引用，删除WDatePicker的扩展方法
2. Xml序列化增加Standalone的文档声明
3. xml增加反序列方法

v2.3.5  2015年5月4日
1. util工具类增加获取枚举的显示文本方法

v2.3.4  2015年4月29日
1. 增加API请求返回model类（ApiResult，ApiResult<T>,PaginateApiResult<T>）

v2.3.3  2015年4月8日 
1. 修复LOG日志的BUG
2. LOG日志增加按文件大小分割日志功能

v2.3.0  2015年3月10日
1. 增加一个时间戳对象 TimeStamp

v2.2 2015年1月31日
1. 完善LOG功能，WEB下的IOC问题未解决
2. MVC的权限增加接口，实现多种登录切换功能

v2.1
1. 增加MVC4的基类，登录验证与权限控制

v2.0
1. XML的命名空间进行调整，不再兼容v1版本