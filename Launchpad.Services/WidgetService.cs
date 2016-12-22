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
        private readonly IWidgetRepository _widgetRepository;
        private readonly IMapper _mapper;

        public WidgetService(IWidgetRepository widgetRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
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
            Widget result;
            using (var scope = UnitOfWork.Begin())
            {
                result = _widgetRepository.Add(target);
                UnitOfWork.SaveChanges();
                scope.Commit();
            }

            return Success(_mapper.Map<WidgetModel>(result));
        }

        public EntityResult<WidgetModel> UpdateWidget(int id, WidgetModel widget)
        {
            var target = _widgetRepository.Get(id);
            if (target == null)
                return NotFound<WidgetModel>(id);

            _mapper.Map<WidgetModel, Widget>(widget, target, opts => { });
            using (var scope = UnitOfWork.Begin())
            {
                _widgetRepository.Update(target);
                UnitOfWork.SaveChanges();
                scope.Commit();
            }

            var model = _mapper.Map<WidgetModel>(target);
            return Success(model);
        }

        public EntityResult DeleteWidget(int id)
        {
            using (var scope = UnitOfWork.Begin())
            {
                _widgetRepository.Delete(id);
                UnitOfWork.SaveChanges();
                scope.Commit();
                return Success();
            }
        }
    }
}
