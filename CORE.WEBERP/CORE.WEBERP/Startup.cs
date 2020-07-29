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
    /// �������������ļ�
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ���������캯��
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// ����
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ע�����
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region �Զ�ע��ӵ��ITransientDependency,IScopeDependency��ISingletonDependency����
            services.AddFxServices();
            #endregion
            #region �Զ�ӳ��ӵ��MapAttribute����
            services.AddAutoMapper();
            #endregion
            #region ���ݿ��ʼ��
            services.AddEFCoreSharding(config =>
            {
                string conName = Configuration["ConnectionName"];
                if (Configuration["LogicDelete"].ToBool())
                    config.UseLogicDelete();
                config.UseDatabase(Configuration.GetConnectionString(conName), Configuration["DatabaseType"].ToEnum<DatabaseType>());
                config.SetEntityAssembly(GlobalData.FXASSEMBLY_PATTERN);
            });
            #endregion
            #region �����Զ���ģ����֤
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            #endregion
            #region ���ȫ���쳣�������Ͳ���У�������
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidFilterAttribute>();
                options.Filters.Add<GlobalExceptionFilter>();
            })
            #endregion
            #region ʹ��NewtonsoftJson�滻��Ĭ�ϵ�json���л����
            .AddNewtonsoftJson(options =>
            {
                ////�޸��������Ƶ����л���ʽ������ĸСд
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                ////�޸�ʱ������л���ʽ
                //options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
                ////���������һ������
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.GetType().GetProperties().ForEach(aProperty =>
                {
                    var value = aProperty.GetValue(JsonExtention.DefaultJsonSetting);
                    aProperty.SetValue(options.SerializerSettings, value);
                });
            });
            #endregion
            #region ����IHttpContextAccessor
            services.AddHttpContextAccessor();
            #endregion
            #region ���Swagger�ӿ�
            services.AddOpenApiDocument(settings =>
            {
                settings.AddSecurity("�����֤Token", Enumerable.Empty<string>(), new OpenApiSecurityScheme()
                {
                    Scheme = "bearer",
                    Description = "Authorization:Bearer {your JWT token}<br/><b>��Ȩ��ַ:/Base_Manage/Home/SubmitLogin</b>",
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.Http
                });
            });
            #endregion
            services.AddControllers();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region ����body����
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            })
            #endregion
            #region �������
            .UseMiddleware<CorsMiddleware>();
            #endregion
            app.UseHttpsRedirection();
            #region ʹ�þ�̬�ļ�
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,//֧��δ֪�ļ�����
                DefaultContentType = "application/octet-stream"//Ĭ�ϵ��ļ�����
            });
            #endregion
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #region ʹ��Swagger
            app.UseOpenApi(); //���swagger����api�ĵ���Ĭ��·���ĵ� /swagger/v1/swagger.json��
            app.UseSwaggerUi3();//���Swagger UI������ܵ���(Ĭ��·��: /swagger).
            #endregion
        }
    }
}
