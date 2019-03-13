/*

The MIT License (MIT)

Copyright (c) 2016 andrewlock

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;

namespace FibraClickSocial.Configuration
{
    /// <summary>
    /// A utility <see cref="Exception"/> that indicates a strong typed configuration model was not configured correctly
    /// </summary>
    public class SettingsValidationException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SettingsValidationException"/>
        /// </summary>
        /// <param name="className">The name of the class being validated</param>
        /// <param name="propertyName">The property of the instance that was invalid</param>
        /// <param name="error">A description of the configuration error</param>
        public SettingsValidationException(string className, string propertyName, string error)
            : this(GetMessage(className, propertyName, error))
        {
        }

        private static string GetMessage(string className, string propertyName, string message)
        {
            return $@"Invalid appsettings: {className}.{propertyName} {message}";
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SettingsValidationException"/>
        /// </summary>
        /// <param name="errorMessage">A description of the configuration error</param>
        public SettingsValidationException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
