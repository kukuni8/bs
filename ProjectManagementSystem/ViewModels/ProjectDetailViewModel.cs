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

        public ProjectAddMissionViewModel AddMission { get; set; }

        public ProjectEditMissionViewModel EditMission { get; set; }

        public ProjectDeleteMissionViewModel DeleteMission { get; set; }

        public IEnumerable<RiskEditViewModel> RiskEditViewModels { get; set; }

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

        [Display(Name = "截止日期")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [Display(Name = "优先级")]
        public MissionPriority Priority { get; set; }

        [Display(Name = "状态")]
        public MissionStatus Status { get; set; }

        public int ProjectId { get; set; }

        [Display(Name = "所属项目")]
        public string ProjectName { get; set; }
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

        [Display(Name = "更新日期")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }
        [Display(Name = "截止日期")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

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
    }

}
