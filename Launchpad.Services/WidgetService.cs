using Launchpad.Services.Interfaces;
using Launchpad.Models;
using Launchpad.Data.Interfaces;
using AutoMapper;
using Launchpad.Models.EntityFramework;
using System.Collections.Generic;
using Launchpad.Core;
using System;

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

        public WidgetModel AddWidget(WidgetModel widget)
        {
            var target = _mapper.Map<Widget>(widget);
            var result = _widgetRepository.Add(target);
            return _mapper.Map<WidgetModel>(result);
        }

        public WidgetModel UpdateWidget(WidgetModel widget)
        {
            WidgetModel result = null;
            var target = _widgetRepository.Get(widget.Id); //Mapping in the services comes back to bite me, alternative is to attach the entity
            if (target != null)
            {
                _mapper.Map<WidgetModel, Widget>(widget, target, opts => { });
                _widgetRepository.Update(target);
                result = _mapper.Map<WidgetModel>(target);
            }
            return result;
        }

        public void DeleteWidget(int id)
        {
            _widgetRepository.Delete(id);
        }
    }
}
