namespace OnlineTSCS.Models
{
    using System.Data.Entity;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using OnlineTSCS.Models.EnumClass;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ///取消级联删除 2017-09-15 重要程度5 保证图状数据库外键引用不会出现级联删除的多路径错误
            modelBuilder.Entity<VideoCommentModel>().HasRequired(a => a.Account).WithMany(u => u.VideoComments).WillCascadeOnDelete(false);
            modelBuilder.Entity<ForumCommentModel>().HasRequired(a => a.Account).WithMany(u => u.ForumComments).WillCascadeOnDelete(false);
            modelBuilder.Entity<PersonalMsgModel>().HasRequired(a => a.ToAccount).WithMany(u => u.ReceivePMs).WillCascadeOnDelete(false);

        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<AccountModel> AccountModels { get; set; }
        public virtual DbSet<LogModel> LogModels { get; set; }
        public virtual DbSet<CourseModel> CourseModels { get; set; }
        public virtual DbSet<ACMappingModel> ACMappingModels { get; set; }
        public virtual DbSet<CourseCommentModel> CourseCommentModels { get; set; }
        public virtual DbSet<VideoModel> VideoModels { get; set; }

        public virtual DbSet<VideoCommentModel> VideoCommentModels { get; set; }
        public virtual DbSet<ForumModel> ForumModels { get; set; }

        public virtual DbSet<ForumCommentModel> ForumCommentModels { get; set; }
        public virtual DbSet<PersonalMsgModel> PersonalMsgModels { get; set; }

        public virtual DbSet<HomeworkModel> HomeworkModels { get; set; }

        public virtual DbSet<HomeworkCommentModel> HomeworkCommentModels { get; set; }

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

        [DisplayName("选课信息")]
        public virtual ICollection<ACMappingModel> Mapping { get; set; }

        [DisplayName("课程评论信息")]
        public virtual ICollection<CourseCommentModel> CourseComments { get; set; }

        [DisplayName("视频评论信息")]
        public virtual ICollection<VideoCommentModel> VideoComments { get; set; }

        [DisplayName("上传视频信息")]
        public virtual ICollection<VideoModel> Videos { get; set; }

        [DisplayName("论坛发贴信息")]
        public virtual ICollection<ForumModel> ForumTitles { get; set; }

        [DisplayName("论坛回帖信息")]
        public virtual ICollection<ForumCommentModel> ForumComments { get; set; }

        [DisplayName("发出的PM")]
        public virtual ICollection<PersonalMsgModel> SendPMs { get; set; }

        [DisplayName("收到的的PM")]
        public virtual ICollection<PersonalMsgModel> ReceivePMs { get; set; }

        [DisplayName("提交的作业")]
        public virtual ICollection<HomeworkCommentModel> HomeworkComments { get; set; }
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

    public class CourseModel
    {
        [Key]
        [DisplayName("CID")]
        public int CourseId { get; set; }

        [DisplayName("课程名称")]
        [Required]
        public string CourseName { get; set; }

        [DisplayName("课程介绍")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("课程类别")]
        [Required]
        public EnumCourseType Type { get; set; }

        [DisplayName("课程容量")]
        [Required]
        [Range(0, 200, ErrorMessage = "课程容量必须在0到200之间")]
        public int Capacity { get; set; }

        [DisplayName("课时")]
        [Required]
        [Range(0, 64, ErrorMessage = "课程容量必须在0到64之间")]
        public int Lesson { get; set; }

        [DisplayName("开课时间")]
        [Required]
        public System.DateTime StartDate { get; set; }

        [DisplayName("通过审核")]
        public bool Checked { get; set; }

        [DisplayName("冻结课程")]
        public bool Frozen { get; set; }

        [DisplayName("提交人员")]
        public int UploadId { get; set; }

        [DisplayName("审核人员")]
        public int JudgeId { get; set; }

        [DisplayName("选课信息")]
        public virtual ICollection<ACMappingModel> Mapping { get; set; }

        [DisplayName("课程评论信息")]
        public virtual ICollection<CourseCommentModel> CourseComments { get; set; }

        [DisplayName("上传视频信息")]
        public virtual ICollection<VideoModel> Videos { get; set; }

        [DisplayName("课程作业信息")]
        public virtual ICollection<HomeworkModel> Homeworks { get; set; }
    }

    public class ACMappingModel
    {
        [Key]
        [DisplayName("MappingID")]
        public int MappingId { get; set; }

        public int CourseId { get; set; }
        public virtual CourseModel Course { get; set; }

        public int Id { get; set; }
        public virtual AccountModel Account { get; set; }
    }

    public class CourseCommentModel
    {
        [Key]
        public int CourseCommentId { get; set; }

        public int CourseId { get; set; }
        public virtual CourseModel Course { get; set; }

        public int Id { get; set; }
        public virtual AccountModel Account { get; set; }

        [DisplayName("评论时间")]
        public System.DateTime CommentDate { get; set; }

        [DisplayName("评论内容")]
        [Required]
        [StringLength(256, MinimumLength = 2, ErrorMessage = "评论过短是没有意义的")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [DisplayName("是否冻结")]
        public bool Frozen { get; set; }

        [DisplayName("是否被举报")]
        public bool Reported { get; set; }
    }

    public class VideoModel
    {
        [Key]
        public int VideoId { get; set; }

        public int CourseId { get; set; }
        public virtual CourseModel Course { get; set; }

        public int Id { get; set; }
        public virtual AccountModel Account { get; set; }

        [DisplayName("上传时间")]
        public System.DateTime UpLoadDate { get; set; }

        [DisplayName("视频名称")]
        [Required]
        public string VideoName { get; set; }

        [DisplayName("视频介绍")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("视频aid")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string BiliAid { get; set; }

        [DisplayName("视频cid")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string BiliCid { get; set; }

        [DisplayName("播放次数")]
        [DataType(DataType.PhoneNumber)]
        public long Times { get; set; }

        [DisplayName("是否冻结")]
        public bool Frozen { get; set; }

        [DisplayName("是否被举报")]
        public bool Reported { get; set; }

        [DisplayName("视频评论信息")]
        public virtual ICollection<VideoCommentModel> VideoComments { get; set; }
    }

    public class VideoCommentModel
    {
        [Key]
        public int VideoCommentId { get; set; }

        public int VideoId { get; set; }
        public virtual VideoModel Video { get; set; }

        public int Id { get; set; }
        public virtual AccountModel Account { get; set; }

        [DisplayName("评论时间")]
        public System.DateTime CommentDate { get; set; }

        [DisplayName("评论内容")]
        [Required]
        [StringLength(256, MinimumLength = 2, ErrorMessage = "评论过短是没有意义的")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [DisplayName("是否冻结")]
        public bool Frozen { get; set; }

        [DisplayName("是否被举报")]
        public bool Reported { get; set; }
    }

    public class ForumModel
    {
        [Key]
        public int ForumItemId { get; set; }

        public int Id { get; set; }
        public virtual AccountModel Account { get; set; }

        [DisplayName("发帖时间")]
        public System.DateTime CommentDate { get; set; }

        [DisplayName("标题")]
        [Required]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "标题必须在8~32个字之间")]
        [DataType(DataType.MultilineText)]
        public string Title { get; set; }

        [DisplayName("内容")]
        [Required]
        [StringLength(256, ErrorMessage = "内容过短是没有意义的")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [DisplayName("是否冻结")]
        public bool Frozen { get; set; }

        [DisplayName("是否被举报")]
        public bool Reported { get; set; }

        [DisplayName("本帖回复信息")]
        public virtual ICollection<ForumCommentModel> ForumComments { get; set; }
    }

    public class ForumCommentModel
    {
        [Key]
        public int ForumCommentId { get; set; }

        public int Id { get; set; }
        public virtual AccountModel Account { get; set; }

        public int ForumItemId { get; set; }
        public virtual ForumModel ForumItems { get; set; }

        [DisplayName("回复时间")]
        public System.DateTime CommentDate { get; set; }

        [DisplayName("内容")]
        [Required]
        [StringLength(256, ErrorMessage = "内容过短是没有意义的")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [DisplayName("是否冻结")]
        public bool Frozen { get; set; }

        [DisplayName("是否被举报")]
        public bool Reported { get; set; }
    }

    public class PersonalMsgModel
    {
        [Key]
        public int PersonalMsgId { get; set; }

        public int FromId { get; set; }
        [ForeignKey("FromId")]
        public virtual AccountModel FromAccount { get; set; }

        public int ToId { get; set; }
        [ForeignKey("ToId")]
        public virtual AccountModel ToAccount { get; set; }

        [DisplayName("发送时间")]
        public System.DateTime MessageDate { get; set; }

        [DisplayName("内容")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [DisplayName("是否冻结")]
        public bool Frozen { get; set; }

        [DisplayName("是否被举报")]
        public bool Reported { get; set; }
    }

    public class HomeworkModel
    {
        [Key]
        public int HomeworkId { get; set; }

        public int CourseId { get; set; }
        public virtual CourseModel Course { get; set; }

        [DisplayName("开放时间")]
        [DataType(DataType.DateTime)]
        public System.DateTime StartDate { get; set; }

        [DisplayName("关闭时间")]
        [DataType(DataType.DateTime)]
        public System.DateTime EndDate { get; set; }

        [DisplayName("作业要求")]
        [DataType(DataType.MultilineText)]
        public string Claim { get; set; }

        [DisplayName("是否冻结")]
        public bool Frozen { get; set; }

        [DisplayName("收到的作业")]
        public virtual ICollection<HomeworkCommentModel> HomeworkComments { get; set; }
    }

    public class HomeworkCommentModel
    {
        [Key]
        public int HomeworkCommentId { get; set; }

        public int Id { get; set; }
        public virtual AccountModel Account { get; set; }

        public int HomeworkId { get; set; }
        public virtual HomeworkModel Homework { get; set; }

        [DisplayName("提交时间")]
        [DataType(DataType.DateTime)]
        public System.DateTime PostDate { get; set; }

        [DisplayName("作业分数")]
        [Range(0, 100, ErrorMessage = "分数必须在0~100之间")]
        public double Score { get; set; }

        [DisplayName("作业内容")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [DisplayName("是否冻结")]
        public bool Frozen { get; set; }
    }
}