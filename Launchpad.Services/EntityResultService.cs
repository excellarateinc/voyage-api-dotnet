using AutoMapper;
using Launchpad.Core;
using Launchpad.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Launchpad.Services
{
    /// <summary>
    /// Base class that helps create EntityResults
    /// </summary>
    public abstract class EntityResultService
    {
        protected readonly IMapper Mapper;

        public EntityResultService(IMapper mapper)
        {
            Mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        /// <summary>
        /// Creates a failure as a result of a missing entity
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>EntityResult</returns>
        protected EntityResult NotFound(object id)
        {
            return new EntityResult(false, true)
              .WithEntityNotFound(id);
        }

        /// <summary>
        /// Creates a failure as a result of a missing entity
        /// </summary>
        /// <typeparam name="TModel">Type of the model</typeparam>
        /// <param name="id">Entity ID</param>
        /// <returns><![CDATA[EntityResult<TModel>]]></returns>
        protected EntityResult<TModel> NotFound<TModel>(object id)
            where TModel : class
        {
            return new EntityResult<TModel>(null, false, true)
              .WithEntityNotFound(id);
        }

        /// <summary>
        /// Creates a successful result with the specified model
        /// </summary>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <param name="model">Model</param>
        /// <returns><![CDATA[EntityResult<TModel>]]></returns>
        protected EntityResult<TModel> Success<TModel>(TModel model)
            where TModel : class
        {
            return new EntityResult<TModel>(model, true, false);
        }

        /// <summary>
        /// Craetes a successful result 
        /// </summary>
        /// <returns>EntityResult</returns>
        protected EntityResult Success()
        {
            return new EntityResult(true, false);
        }

        /// <summary>
        /// Converts IdentityResult to an EntityResult 
        /// </summary>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <param name="result">IdentityResult</param>
        /// <param name="model">Model</param>
        /// <returns><![CDATA[EntityResult<TModel>]]></returns>
        protected EntityResult<TModel> FromIdentityResult<TModel>(IdentityResult result, TModel model)
             where TModel : class
        {
            return new EntityResult<TModel>(model, result.Succeeded, false, result.Errors.ToArray());
        }

        /// <summary>
        /// Converts IdentityResult to an EntityResult
        /// </summary>
        /// <param name="result">The result to convert.</param>
        /// <returns>The converted EntityResult.</returns>
        protected EntityResult FromIdentityResult(IdentityResult result)
        {
            return new EntityResult(result.Succeeded, false, result.Errors.ToArray());
        }

        /// <summary>
        /// Merges two collections:
        ///     - New items in source are added to destination
        ///     - Updated items in source are updated in destination
        ///     - Items in destination that are not in source are removed
        /// </summary>
        /// <typeparam name="TSource">Source collection type</typeparam>
        /// <typeparam name="TDest">Destination collection type</typeparam>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        /// <param name="predicate">Predicate for matching items between collections</param>
        /// <param name="deleteAction">Action for deleting items</param>
        protected void MergeCollection<TSource, TDest>(
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
                    Mapper.Map<TSource, TDest>(model, entity);
                }
                else
                {
                    // Add new item in destination
                    entity = Mapper.Map<TSource, TDest>(model);
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
