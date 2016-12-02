using AutoMapper;
using Launchpad.Core;
using Launchpad.Data.Interfaces;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;
using Launchpad.Services.Interfaces;
using System.Collections.Generic;

namespace Launchpad.Services
{
    public class WidgetService : EntityResultService, IWidgetService
    {
        private IWidgetRepository _widgetRepository;
        private IMapper _mapper;

        public WidgetService(IWidgetRepository widgetRepository, IMapper mapper)
        {
            _widgetRepository = widgetRepository.ThrowIfNull(nameof(widgetRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public EntityResult<IEnumerable<WidgetModel>> GetWidgets()
        {
            var widgets = _widgetRepository.GetAll();
            return Success(_mapper.Map<IEnumerable<Widget>, IEnumerable<WidgetModel>>(widgets));
        }

        public EntityResult<WidgetModel> GetWidget(int id)
        {
            var widget = _widgetRepository.Get(id);
            return widget == null
                    ? NotFound<WidgetModel>(id)
                    : Success(_mapper.Map<WidgetModel>(widget));
        }

        public EntityResult<WidgetModel> AddWidget(WidgetModel widget)
        {
            var target = _mapper.Map<Widget>(widget);
            var result = _widgetRepository.Add(target);
            return Success(_mapper.Map<WidgetModel>(result));
        }

        public EntityResult<WidgetModel> UpdateWidget(int id, WidgetModel widget)
        {
            WidgetModel model = null;
            var target = _widgetRepository.Get(id); //Mapping in the services comes back to bite me, alternative is to attach the entity
            if (target == null)
                return NotFound<WidgetModel>(id);


            _mapper.Map<WidgetModel, Widget>(widget, target, opts => { });
            _widgetRepository.Update(target);
            model = _mapper.Map<WidgetModel>(target);

            return Success(model);
        }

        public EntityResult DeleteWidget(int id)
        {
            _widgetRepository.Delete(id);
            return Success();
        }
    }
}
