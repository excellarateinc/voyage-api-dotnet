using System.ComponentModel.DataAnnotations;

namespace Launchpad.Models
{
    public class WidgetModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="A widget must have a name")]
        public string Name { get; set; }
    }
}
