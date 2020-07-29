using System.Linq;
using CORE.Util;
using CORE.WEBERP.Filter;
using CORE.WEBERP.Middleware;
using EFCore.Sharding;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;

namespace CORE.WEBERP
{
    /// <summary>
    /// 启动服务配置文件
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 启动服务构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region 自动注入拥有ITransientDependency,IScopeDependency或ISingletonDependency的类
            services.AddFxServices();
            #endregion
            #region 自动映射拥有MapAttribute的类
            services.AddAutoMapper();
            #endregion
            #region 数据库初始化
            services.AddEFCoreSharding(config =>
            {
                string conName = Configuration["ConnectionName"];
                if (Configuration["LogicDelete"].ToBool())
                    config.UseLogicDelete();
                config.UseDatabase(Configuration.GetConnectionString(conName), Configuration["DatabaseType"].ToEnum<DatabaseType>());
                config.SetEntityAssembly(GlobalData.FXASSEMBLY_PATTERN);
            });
            #endregion
            #region 禁用自定义模型验证
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            #endregion
            #region 添加全局异常过滤器和参数校验过滤器
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidFilterAttribute>();
                options.Filters.Add<GlobalExceptionFilter>();
            })
            #endregion
            #region 使用NewtonsoftJson替换掉默认的json序列化组件
            .AddNewtonsoftJson(options =>
            {
                ////修改属性名称的序列化方式，首字母小写
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                ////修改时间的序列化方式
                //options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
                ////解决命名不一致问题
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.GetType().GetProperties().ForEach(aProperty =>
                {
                    var value = aProperty.GetValue(JsonExtention.DefaultJsonSetting);
                    aProperty.SetValue(options.SerializerSettings, value);
                });
            });
            #endregion
            #region 启用IHttpContextAccessor
            services.AddHttpContextAccessor();
            #endregion
            #region 添加Swagger接口
            services.AddOpenApiDocument(settings =>
            {
                settings.AddSecurity("身份认证Token", Enumerable.Empty<string>(), new OpenApiSecurityScheme()
                {
                    Scheme = "bearer",
                    Description = "Authorization:Bearer {your JWT token}<br/><b>授权地址:/Base_Manage/Home/SubmitLogin</b>",
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.Http
                });
            });
            #endregion
            services.AddControllers();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region 允许body重用
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            })
            #endregion
            #region 允许跨域
            .UseMiddleware<CorsMiddleware>();
            #endregion
            app.UseHttpsRedirection();
            #region 使用静态文件
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,//支持未知文件类型
                DefaultContentType = "application/octet-stream"//默认的文件编码
            });
            #endregion
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #region 使用Swagger
            app.UseOpenApi(); //添加swagger生成api文档（默认路由文档 /swagger/v1/swagger.json）
            app.UseSwaggerUi3();//添加Swagger UI到请求管道中(默认路由: /swagger).
            #endregion
        }
    }
}
