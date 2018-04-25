﻿using System;
using System.IO;
using System.Threading;
using Allure.Commons;
using IML_AT_Core.Core.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace IML_AT_Core.Core
{
    public static class StepRunner
    {
        public static string StepName { get; set; }
        private static readonly ThreadLocal<string> Uuid = new ThreadLocal<string>();
        private static string pathToFile;

        public static void Run(string stepName, Action stepBody)
        {
            StepName = stepName;
            Uuid.Value = Guid.NewGuid().ToString("N");
            var stepResult = new StepResult
            {
                name = stepName,
                start = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
            AllureLifecycle.Instance.StartStep(Uuid.Value, stepResult);

            try
            {
                stepBody();
                AllureLifecycle.Instance.UpdateStep(Uuid.Value, result => { result.status = Status.passed; });
            }
            catch (Exception)
            {
                var timestamp = DateTime.Now.ToString("dd-MM-yyyy-hhmmss");
                pathToFile = Path.Combine(TestContext.CurrentContext.TestDirectory,
                    TestContext.CurrentContext.Test.ID + "-" + timestamp + ".png");
                DriverFactory.GetDriver().TakeScreenshot()
                    .SaveAsFile(pathToFile, ScreenshotImageFormat.Png);
                var attachment = new Attachment
                {
                    type = "image/png",
                    source = pathToFile
                };
                AllureLifecycle.Instance.UpdateStep(Uuid.Value, result => { result.attachments.Add(attachment); });
                throw;
            }
            finally
            {
                if (File.Exists(pathToFile)) File.Delete(pathToFile);
                AllureLifecycle.Instance.StopStep(Uuid.Value);
            }
        }

    }
}


