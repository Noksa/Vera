﻿using System;
using System.Collections.Generic;
using System.Reflection;
using PageTitleAttribute = AT_Core_Specflow.CustomElements.Attributes.PageTitleAttribute;

namespace AT_Core_Specflow.Core
{
    public class PageContext
    {
        private Dictionary<object, string> _elements;
        private Dictionary<string, Dictionary<object, string>> _blocksElements;
        public Dictionary<object, string> Elements => _elements ?? (_elements = new Dictionary<object, string>());

        public Dictionary<string, Dictionary<object, string>> BlocksElements =>
            _blocksElements ?? (_blocksElements = new Dictionary<string, Dictionary<object, string>>());

        public BasePage CurrentPage { get; private set; }
        public object DecoratingBlock { get; set; }
        public object DecoratingPage { get; set; }
        public BasePage OpenPage(string title)
        {
            foreach (var page in PageManager.PagesTypes)
            {
                if (!(page.GetCustomAttribute(typeof(PageTitleAttribute)) is PageTitleAttribute attr) ||
                    attr.Title != title) continue;
                var newPage = (BasePage)Activator.CreateInstance(page);
                CurrentPage = newPage;
                return CurrentPage;
            }
            throw new NullReferenceException($"Cant initialize page with title '{title}'.");
        }

    }
}