﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CalendarSkillTest.Flow.Utterances;
using Luis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;

namespace CalendarSkillTest.Flow.Fakes
{
    public class MockLuisRecognizer : ITelemetryRecognizer
    {
        private BaseTestUtterances utterancesManager;
        private GeneralTestUtterances generalUtterancesManager;

        public MockLuisRecognizer(BaseTestUtterances utterancesManager)
        {
            this.utterancesManager = utterancesManager;
        }

        public MockLuisRecognizer(params BaseTestUtterances[] utterancesManagers)
        {
            this.utterancesManager = new BaseTestUtterances();

            foreach (var manager in utterancesManagers)
            {
                foreach (var pair in manager)
                {
                    this.utterancesManager.TryAdd(pair.Key, pair.Value);
                }
            }
        }

        public MockLuisRecognizer()
        {
            this.generalUtterancesManager = new GeneralTestUtterances();
        }

        public bool LogPersonalInformation { get; set; } = false;

        public IBotTelemetryClient TelemetryClient { get; set; } = new NullBotTelemetryClient();

        public Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken)
            where T : IRecognizerConvert, new()
        {
            var mockResult = new T();

            var t = typeof(T);
            var text = turnContext.Activity.Text;
            if (t.Name.Equals(typeof(calendarLuis).Name))
            {
                calendarLuis mockCalendar = utterancesManager.GetValueOrDefault(text, utterancesManager.GetBaseNoneIntent());

                var test = mockCalendar as object;
                mockResult = (T)test;
            }
            else if (t.Name.Equals(typeof(General).Name))
            {
                var mockGeneralIntent = generalUtterancesManager.GetValueOrDefault(text, generalUtterancesManager.GetBaseNoneIntent());

                var test = mockGeneralIntent as object;
                mockResult = (T)test;
            }

            return await Task.FromResult(mockResult);
        }

        public Task<T> RecognizeAsync<T>(DialogContext dialogContext, CancellationToken cancellationToken = default(CancellationToken))
            where T : IRecognizerConvert, new()
        {
            throw new NotImplementedException();
        }

        public Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, Dictionary<string, string> telemetryProperties, Dictionary<string, double> telemetryMetrics, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<T> RecognizeAsync<T>(ITurnContext turnContext, Dictionary<string, string> telemetryProperties, Dictionary<string, double> telemetryMetrics, CancellationToken cancellationToken = default(CancellationToken))
            where T : IRecognizerConvert, new()
        {
            throw new NotImplementedException();
        }
    }
}