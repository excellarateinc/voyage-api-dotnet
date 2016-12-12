using FluentValidation.Attributes;
using Launchpad.Models.Validators;

namespace Launchpad.Models
{
    [Validator(typeof(WidgetModelValidator))]
    public class WidgetModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}
