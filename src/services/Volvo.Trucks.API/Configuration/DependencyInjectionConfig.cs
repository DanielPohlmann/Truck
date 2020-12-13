using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volvo.Core.Mediator;
using Volvo.Trucks.API.Application.Commands;
using Volvo.Trucks.API.Application.Events;
using Volvo.Trucks.API.Application.Queries;
using Volvo.Trucks.API.Application.Queries.Interfaces;
using Volvo.Trucks.API.Data;
using Volvo.Trucks.API.Data.Repository;
using Volvo.Trucks.API.Models.Interfaces;

namespace Volvo.Trucks.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IRequestHandler<AddTruckCommand, ValidationResult>, TruckCommandHandler>();
            services.AddScoped<IRequestHandler<EditTruckCommand, ValidationResult>, TruckCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveTruckCommand, ValidationResult>, TruckCommandHandler>();

            services.AddScoped<INotificationHandler<TruckRegistrationEvent>, TruckEventHandler>();

            services.AddScoped<IModelServiceQuery, ModelServiceQuery>();
            services.AddScoped<ITruckServiceQuery, TruckServiceQuery>();

            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<ITruckRepository, TruckRepository>();
            services.AddScoped<TruckContext>();
        }
    }
}
