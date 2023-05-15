namespace ProjectManagementSystem.ViewModels
{
    public class DefectIndexViewModel
    {
        public string CurProjectName { get; set; }
        public int CurProjectId { get; set; }
        public IEnumerable<DefectEditViewModel> DefectEditViewModels { get; set; }
    }
}
