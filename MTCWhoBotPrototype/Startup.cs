﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.10.3

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.ApplicationInsights;
using Microsoft.Bot.Builder.Integration.ApplicationInsights.Core;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTCRequestBot;
using MTCWhoBotPrototype.Bots;
using MTCWhoBotPrototype.Dialogs;
using MTCWhoBotPrototype.Translation;

namespace MTCWhoBotPrototype
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            #region Telemetry
            // Add Application Insights services into service collection
            services.AddApplicationInsightsTelemetry();

            // Create the telemetry client.
            services.AddSingleton<IBotTelemetryClient, BotTelemetryClient>();

            // Add telemetry initializer that will set the correlation context for all telemetry items.
            services.AddSingleton<ITelemetryInitializer, OperationCorrelationTelemetryInitializer>();

            // Add telemetry initializer that sets the user ID and session ID (in addition to other bot-specific properties such as activity ID)
            services.AddSingleton<ITelemetryInitializer, TelemetryBotIdInitializer>();

            // Create the telemetry middleware to initialize telemetry gathering
            services.AddSingleton<TelemetryInitializerMiddleware>();

            // Create the telemetry middleware (used by the telemetry initializer) to track conversation events
           // services.AddSingleton<TelemetryLoggerMiddleware>();
            // PII now collected
            services.AddSingleton<TelemetryLoggerMiddleware>(s => new TelemetryLoggerMiddleware(s.GetService<IBotTelemetryClient>(), true));

            #endregion

            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            services.AddSingleton<IStorage, MemoryStorage>();

            // Create the User state. (Used in this bot's Dialog implementation.)
            services.AddSingleton<UserState>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            services.AddSingleton<ConversationState>();

            // Register LUIS recognizer
            services.AddSingleton<MTCRequestRecognizer>();

            // Register the BookingDialog.
            services.AddSingleton<MTCRequestDialog>();

            // Register LUIS recognizer
            //services.AddSingleton<FlightBookingRecognizerOld>();

            //// Register the BookingDialog.
            //services.AddSingleton<BookingDialog>();

            // The MainDialog that will be run by the bot.
            services.AddSingleton<MainDialog>();

            // Create the Microsoft Translator responsible for making calls to the Cognitive Services translation service
            services.AddSingleton<MicrosoftTranslator>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, DialogAndWelcomeBot<MainDialog>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
