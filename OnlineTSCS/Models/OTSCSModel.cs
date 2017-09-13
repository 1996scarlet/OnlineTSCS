namespace OnlineTSCS.Models
{
    using System.Data.Entity;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using OnlineTSCS.Models.EnumClass;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OTSCSModel : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“OTSCSModel”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“OnlineTSCS.Models.OTSCSModel”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“OTSCSModel”
        //连接字符串。
        public OTSCSModel()
            : base("name=OTSCSModel")
        {
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<AccountModel> AccountModels { get; set; }
        public virtual DbSet<LogModel> LogModels { get; set; }
    }

    public class AccountModel
    {
        [Key]
        [DisplayName("UID")]
        public int Id { get; set; }

        [DisplayName("用户账号")]
        [Required]
        [StringLength(32, ErrorMessage = "账号长度必须在6到32之间", MinimumLength = 6)]
        public string AccountName { get; set; }

        [DisplayName("用户密码")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "密码长度必须在8到32之间", MinimumLength = 8)]
        [Required]
        public string Password { get; set; }

        [DisplayName("用户类别")]
        [Required]
        public EnumAccountType Type { get; set; }

        [DisplayName("密码确认")]
        [NotMapped]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "密码长度必须在8到32之间", MinimumLength = 8)]
        public string ConfirmPassword { get; set; }

        //------------Other Infoes-------------//

        [DisplayName("信用卡号")]
        [DataType(DataType.CreditCard)]
        public string Credit { get; set; }

        [DisplayName("单位（学校）")]
        [StringLength(16, ErrorMessage = "学校名称长度必须位于4~16之间", MinimumLength = 4)]
        public string School { get; set; }

        [DisplayName("手机号码")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        [DisplayName("电子邮箱")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class LogModel
    {
        [Key]
        [DisplayName("LOGID")]
        public int Id { get; set; }

        [DisplayName("调用Action")]
        public string ActionName { get; set; }

        [DisplayName("调用控制器")]
        public string ControllerName { get; set; }

        [DisplayName("附加参数")]
        public string ActionParameters { get; set; }

        [DisplayName("记录时间")]
        public System.DateTime AccessDate { get; set; }

        [DisplayName("操作账号")]
        public string ActionAccount { get; set; }
    }
}