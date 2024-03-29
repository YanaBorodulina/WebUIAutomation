﻿using System;

namespace Sample.Web.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UrlPartAttribute : Attribute
    {
        public UrlPartAttribute(string urlPart)
        {
            UrlPart = urlPart;
        }

        public string UrlPart { get; }
    }
}