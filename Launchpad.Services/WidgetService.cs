using Launchpad.Services.Interfaces;
using System.Linq;
using Launchpad.Models;
using Launchpad.Data.Interfaces;
using AutoMapper;
using Launchpad.Models.EntityFramework;
using System.Collections.Generic;

namespace Launchpad.Services
{
    public class WidgetService : IWidgetService
    {
        private IWidgetRepository _widgetRepository;
        private IMapper _mapper;

        public WidgetService(IWidgetRepository widgetRepository, IMapper mapper)
        {
            _widgetRepository = widgetRepository;
            _mapper = mapper;
        }

        public IEnumerable<WidgetModel> GetWidgets()
        {
            var widgets = _widgetRepository.GetAll();
            return _mapper.Map<IEnumerable<Widget>, IEnumerable<WidgetModel>>(widgets);
        }
    }
}
