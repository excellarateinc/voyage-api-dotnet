using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Voyage.Core
{
    public static class CollectionHelpers
    {
        /// <summary>
        /// Merges two collections:
        ///     - New items in source are added to destination
        ///     - Updated items in source are updated in destination
        ///     - Items in destination that are not in source are removed
        /// </summary>
        /// <typeparam name="TSource">Source collection type</typeparam>
        /// <typeparam name="TDest">Destination collection type</typeparam>
        /// <param name="mapper">Mapping instance</param>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        /// <param name="predicate">Predicate for matching items between collections</param>
        /// <param name="deleteAction">Action for deleting items</param>
        public static void MergeCollection<TSource, TDest>(
            IMapper mapper,
            ICollection<TSource> source,
            ICollection<TDest> destination,
            Func<TSource, TDest, bool> predicate,
            Action<TDest> deleteAction)
        {
            foreach (var model in source)
            {
                // Attempt to find match in destination
                var entity = destination.FirstOrDefault(_ => predicate(model, _));

                if (entity != null)
                {
                    // Update item in destination
                    mapper.Map<TSource, TDest>(model, entity);
                }
                else
                {
                    // Add new item in destination
                    entity = mapper.Map<TSource, TDest>(model);
                    destination.Add(entity);
                }
            }

            // Remove items that exist in destination but not in the source
            List<TDest> removed = destination
                .Where(_ => !source.Any(s => predicate(s, _)))
                .ToList();

            removed.ForEach(_ =>
            {
                // This will unwire the relationship in the context
                destination.Remove(_);

                // This will actually delete the object
                deleteAction(_);
            });
        }
    }
}
