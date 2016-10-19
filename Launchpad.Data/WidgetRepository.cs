using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using System.Linq;
using System;

namespace Launchpad.Data
{
    public class WidgetRepository : BaseRepository<Widget>, IWidgetRepository
    {
        public WidgetRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public override Widget Add(Widget model)
        {
            var addedModel = Context.Widgets.Add(model);
            Context.SaveChanges();
            return addedModel;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if(entity != null)
            {
                Context.Widgets.Remove(entity);
                Context.SaveChanges();
            }
        }

        public override Widget Get(object id)
        {
            return Context.Widgets.Find(id);
        }

        public override IQueryable<Widget> GetAll()
        {
            return Context.Widgets;
        }

        public override Widget Update(Widget model)
        {
            Context.SaveChanges();
            return model;
        }
    }
}
