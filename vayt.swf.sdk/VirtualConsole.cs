﻿/*******************************************************************************
* Copyright 2009-2013 Amazon.com, Inc. or its affiliates. All Rights Reserved.
* 
* Licensed under the Apache License, Version 2.0 (the "License"). You may
* not use this file except in compliance with the License. A copy of the
* License is located at
* 
* http://aws.amazon.com/apache2.0/
* 
* or in the "license" file accompanying this file. This file is
* distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the specific
* language governing permissions and limitations under the License.
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;

namespace vayt.swf.sdk
{
    public class VirtualConsole
    {
        TextBox _window;

        public VirtualConsole(TextBox window)
        {
            this._window = window;
        }

        public void WriteLine(string message)
        {
            this._window.Dispatcher.BeginInvoke((Action)(() =>
            {
                this._window.Text += message + "\r\n";
                this._window.ScrollToEnd();
            }));
        }
    }
}
