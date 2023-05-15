using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class ResourceViewModel
    {
        public int CurProjectId { get; set; }
        public IEnumerable<ResourceIndexViewModel> ResourceIndexViewModels { get; set; }

        public ResourceAddViewModel ResourceAddViewModel { get; set; }

        public ResourceChangeViewModel UseResourceViewModel { get; set; }

        public ResourceChangeViewModel AddResourceViewModel { get; set; }
        public FundIndexViewModel FundIndexViewModel { get; set; }

        public FundChange AddFundViewModel { get; set; }

        public FundChange UseFundViewModel { get; set; }
    }
}
