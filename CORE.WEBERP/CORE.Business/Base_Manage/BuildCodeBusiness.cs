using CORE.Entity.Base_Manage;
using CORE.Util;
using EFCore.Sharding;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CORE.Business.Base_Manage
{
    public class BuildCodeBusiness : BaseBusiness<Base_DbLink>, IBuildCodeBusiness, ITransientDependency
    {
        public BuildCodeBusiness(IDbAccessor db, IHostingEnvironment evn)
            : base(db)
        {
            var projectPath = evn.ContentRootPath;
            _solutionPath = Directory.GetParent(projectPath).ToString();
        }

        private static readonly List<string> ignoreProperties =
            new List<string> { "Id", "CreateTime", "CreatorId", "CreatorRealName", "Deleted" };

        #region 外部接口

        /// <summary>
        /// 获取所有数据库连接
        /// </summary>
        /// <returns></returns>
        public List<Base_DbLink> GetAllDbLink()
        {
            return GetList();
        }

        /// <summary>
        /// 获取数据库所有表
        /// </summary>
        /// <param name="linkId">数据库连接Id</param>
        /// <returns></returns>
        public List<DbTableInfo> GetDbTableList(string linkId)
        {
            if (linkId.IsNullOrEmpty())
                return new List<DbTableInfo>();
            else
                return GetTheDbHelper(linkId).GetDbAllTables();
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="linkId">数据库连接Id</param>
        /// <returns></returns>
        public void Build(BuildInputDTO input)
        {
            string linkId = input.linkId;
            string areaName = input.areaName;
            List<string> tables = input.tables;
            List<int> buildTypes = input.buildTypes;
            _areaName = areaName;
            //获取相关数据库的dbHelper
            _dbHelper = GetTheDbHelper(linkId);
            //获取数据库所有的表添加到字典里
            GetDbTableList(linkId).ForEach(aTable =>
            {
                _dbTableInfoDic.Add(aTable.TableName, aTable);
            });

            tables.ForEach(aTable =>
            {
                //获取表字段信息
                var tableFieldInfo = _dbHelper.GetDbTableInfo(aTable);
                //表实体名
                string entityName = aTable;
                //业务逻辑参数名
                string busName = $"{entityName.ToFirstLowerStr()}Bus";
                //业务逻辑变量名
                string _busName = $"_{busName}";
                List<string> selectOptionsList = new List<string>();
                List<string> listColumnsList = new List<string>();
                List<string> formColumnsList = new List<string>();
                //提取忽略字段的所有信息
                tableFieldInfo.Where(x => !ignoreProperties.Contains(x.Name)).ToList().ForEach(aField =>
                {
              
                    Dictionary<string, string> renderParamters = new Dictionary<string, string>
                    {
                        {$"%{nameof(areaName)}%",areaName },
                        {$"%{nameof(entityName)}%",entityName },
                        {$"%{nameof(busName)}%",busName },
                        {$"%{nameof(_busName)}%",_busName },
                    };

                    //buildTypes,实体层=0,业务层=1,接口层=2,页面层=3
                    //实体层
                    if (buildTypes.Contains(0))
                    {
                        BuildEntity(tableFieldInfo, aTable);
                    }
                    string tmpFileName = string.Empty;
                    string savePath = string.Empty;
                    //业务层
                    if (buildTypes.Contains(1))
                    {
                        //接口
                        tmpFileName = "IBusiness.txt";
                        savePath = Path.Combine(
                            _solutionPath,
                            "CORE.IBusiness",
                            areaName,
                            $"I{entityName}Business.cs");
                        WriteCode(renderParamters, tmpFileName, savePath);

                        //实现
                        tmpFileName = "Business.txt";
                        savePath = Path.Combine(
                            _solutionPath,
                            "CORE.Business",
                            areaName,
                            $"{entityName}Business.cs");
                        WriteCode(renderParamters, tmpFileName, savePath);
                    }
                    //接口层
                    if (buildTypes.Contains(2))
                    {
                        tmpFileName = "Controller.txt";
                        savePath = Path.Combine(
                            _solutionPath,
                            "CORE.WEBERP",
                            "Controllers",
                            areaName,
                            $"{entityName}Controller.cs");
                        WriteCode(renderParamters, tmpFileName, savePath);
                    }
                });
            });
        }

        #endregion

        #region 私有成员

        readonly string _solutionPath;
        private string _areaName { get; set; }
        private void BuildEntity(List<TableInfo> tableInfo, string tableName)
        {
            //区域名
            string nameSpace = $@"CORE.Entity.{_areaName}";
            //生成全路径
            string filePath = Path.Combine(_solutionPath, "CORE.Entity", _areaName, $"{tableName}.cs");

            _dbHelper.SaveEntityToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, filePath, nameSpace);
        }
        private DbHelper GetTheDbHelper(string linkId)
        {
            var theLink = GetTheLink(linkId);
            DbHelper dbHelper = DbHelperFactory.GetDbHelper(theLink.DbType.ToEnum<DatabaseType>(), theLink.ConnectionStr);

            return dbHelper;
        }
        private Base_DbLink GetTheLink(string linkId)
        {
            Base_DbLink resObj = new Base_DbLink();
            var theModule = GetIQueryable().Where(x => x.Id == linkId).FirstOrDefault();
            resObj = theModule ?? resObj;

            return resObj;
        }
        private DbHelper _dbHelper { get; set; }
        private Dictionary<string, DbTableInfo> _dbTableInfoDic { get; set; } = new Dictionary<string, DbTableInfo>();
        private void WriteCode(Dictionary<string, string> paramters, string templateFileName, string savePath)
        {
            string tmpFileText = File.ReadAllText(Path.Combine(_solutionPath, "CORE.WEBERP", "BuildCodeTemplate", templateFileName));
            paramters.ForEach(aParamter =>
            {
                tmpFileText = tmpFileText.Replace(aParamter.Key, aParamter.Value);
            });
            FileHelper.WriteTxt(tmpFileText, savePath, Encoding.UTF8, FileMode.Create);
        }

        #endregion

        #region 数据模型

        #endregion
    }
}
