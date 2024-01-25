using Microsoft.Build.Framework;

namespace Bilet15Mamba.Areas.Admin.ViewModels
{
    public class UpdateSettingVm
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
