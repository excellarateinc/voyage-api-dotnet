using Launchpad.Services.Interfaces;
using Launchpad.Models;
using Launchpad.Data.Interfaces;
using AutoMapper;
using Launchpad.Models.EntityFramework;
using System.Collections.Generic;
using System;
using Launchpad.Core;

namespace Launchpad.Services
{
    public class WidgetService : IWidgetService
    {
        private IWidgetRepository _widgetRepository;
        private IMapper _mapper;

        public WidgetService(IWidgetRepository widgetRepository, IMapper mapper)
        {
            _widgetRepository = widgetRepository.ThrowIfNull(nameof(widgetRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public IEnumerable<WidgetModel> GetWidgets()
        {
            var widgets = _widgetRepository.GetAll();
            return _mapper.Map<IEnumerable<Widget>, IEnumerable<WidgetModel>>(widgets);
        }

        public WidgetModel GetWidget(int id)
        {
            var widget = _widgetRepository.Get(id);
            return _mapper.Map<WidgetModel>(widget);
        }
    }
}
