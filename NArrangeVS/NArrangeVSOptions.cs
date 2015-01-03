﻿// Copyright 2015 MIRATECH
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace NArrangeVS
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false)]
    [ComVisible(true)]
    public class NArrangeVSOptions : DialogPage
    {
        private static readonly string DefaultNArrangeFolderPath = @"C:\Program Files (x86)\NArrange 0.2.9";

        [Category("Integration")]
        [DisplayName("Arrange File On Save")]
        [Description("When enabled, all files which match the arrange file mask will be arranged.")]
        public bool ArrangeFileOnSave
        {
            get;
            set;
        }

        [Category("Integration")]
        [DisplayName("Arrange File Mask")]
        [Description("A regular expression to match files to automatically arrange.")]
        public string ArrangeFileRegex
        {
            get;
            set;
        }

        [Category("NArrange Information")]
        [DisplayName("NArrange Config Location")]
        [Description("The location of the NArrange XML configuration file to be used for arranging the files.")]
        public string NArrangeConfigLocation
        {
            get;
            set;
        }

        [Category("NArrange Information")]
        [DisplayName("NArrange Console Location")]
        [Description("The location of the narrange-console.exe file.")]
        public string NArrangeConsoleLocation
        {
            get;
            set;
        }

        public override void LoadSettingsFromStorage()
        {
            ArrangeFileOnSave = true;
            ArrangeFileRegex = "\\.cs$";
            NArrangeConsoleLocation = Path.Combine(DefaultNArrangeFolderPath, "narrange-console.exe");
            NArrangeConfigLocation = Path.Combine(DefaultNArrangeFolderPath, "DefaultConfig.xml");
            base.LoadSettingsFromStorage();
        }
    }
}