using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
    public class ProjectDetailViewModel
    {
        public ProjectEditViewModel ProjectEditViewModel { get; set; }
        public ProjectMissionIndexViewModel ProjectMissionIndexViewModel { get; set; }

        public Project CurProject { get; set; }

        public int CurProjectId { get; set; }

        public ProjectAddMissionViewModel AddMission { get; set; }

        public ProjectEditMissionViewModel EditMission { get; set; }

        public ProjectDeleteMissionViewModel DeleteMission { get; set; }

        public IEnumerable<RiskEditViewModel> RiskEditViewModels { get; set; }

        public IEnumerable<DefectEditViewModel> DefectEditViewModels { get; set; }

        public IEnumerable<ProjectUserIndexViewModel> ProjectUserIndexViewModels { get; set; }

        public List<ProjectUserNotInProjectModel> UsersNotInThisProject { get; set; }

        public IEnumerable<BookIndexViewModel> BookIndexViewModels { get; set; }

        public IEnumerable<ResourceIndexViewModel> ResourceIndexViewModels { get; set; }

        public ResourceAddViewModel ResourceAddViewModel { get; set; }

        public ResourceChangeViewModel UseResourceViewModel { get; set; }

        public ResourceChangeViewModel AddResourceViewModel { get; set; }
    }
    public class ProjectMissionIndexViewModel
    {
        public IEnumerable<ProjectEditMissionViewModel> FinishedMissions { get; set; }
        public IEnumerable<ProjectEditMissionViewModel> UntreatedMissions { get; set; }
        public IEnumerable<ProjectEditMissionViewModel> ProcessOnMissions { get; set; }
    }

    public class ProjectDeleteMissionViewModel
    {
        public int MissionId { get; set; }
        public int ProjectId { get; set; }
    }
    public class ProjectAddMissionViewModel
    {
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "创建日期")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "开始日期")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "截止日期")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [Display(Name = "完成时间")]
        [DataType(DataType.Date)]
        public DateTime FinishedTime { get; set; }

        [Display(Name = "优先级")]
        public MissionPriority Priority { get; set; }

        [Display(Name = "状态")]
        public MissionStatus Status { get; set; }

        [Display(Name = "所属项目")]
        public string ProjectName { get; set; }

        public int ProjectId { get; set; }

        [Display(Name = "执行者")]
        public List<string> Executors { get; set; }
    }
    public class ProjectEditMissionViewModel
    {
        [Display(Name = "编号")]
        public int Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }


        [Display(Name = "创建日期")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "开始日期")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "截止日期")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [Display(Name = "完成时间")]
        [DataType(DataType.Date)]
        public DateTime FinishedTime { get; set; }

        [Display(Name = "优先级")]
        public MissionPriority Priority { get; set; }

        [Display(Name = "状态")]
        public MissionStatus Status { get; set; }

        [Display(Name = "所属项目")]
        public int ProjectId { get; set; }

        [Display(Name = "所属项目")]
        public string ProjectName { get; set; }

        [Display(Name = "执行者")]
        public List<string> Executors { get; set; }

        public List<MissionDialogue> Dialogues { get; set; }

        [Display(Name = "记录")]
        public string Content { get; set; }
    }

    public class ProjectUserIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }

    }

    public class ProjectUserNotInProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
        public bool IsSelected { get; set; }
    }

}
