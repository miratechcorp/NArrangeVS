// Copyright 2015 MIRATECH
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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using NArrange.Core;
using NArrangeVS.Events;

namespace NArrangeVS
{
    [Guid(GuidList.guidNArrangeVSPkgString)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(NArrangeVSOptions), "NArrangeVS", "General", 0, 0, true, new[] { "NArrange", })]
    public sealed class NArrangeVSPackage : Package
    {
        internal readonly Lazy<DTE2> dte;
        internal readonly Lazy<Regex> fileRegex;
        internal readonly Lazy<NArrangeVSOptions> options;
        internal readonly Lazy<RunningDocumentTable> rdt;
        internal readonly Lazy<Microsoft.VisualStudio.OLE.Interop.IServiceProvider> sp;

        public NArrangeVSPackage()
        {
            options = new Lazy<NArrangeVSOptions>(() => GetDialogPage(typeof(NArrangeVSOptions)) as NArrangeVSOptions, true);
            dte = new Lazy<DTE2>(() => ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as DTE2);
            sp = new Lazy<Microsoft.VisualStudio.OLE.Interop.IServiceProvider>(() => Package.GetGlobalService(typeof(Microsoft.VisualStudio.OLE.Interop.IServiceProvider)) as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            rdt = new Lazy<RunningDocumentTable>(() => new RunningDocumentTable(new ServiceProvider(sp.Value)));
            fileRegex = new Lazy<Regex>(() => new Regex(options.Value.ArrangeFileRegex));
        }

        internal void NArrangeDocument(EnvDTE.Document document)
        {
            // Get the text document.
            TextDocument textDocument = document.Object("TextDocument") as TextDocument;
            // Get the full document text.
            EditPoint editStart = textDocument.StartPoint.CreateEditPoint();
            string inputFileText = editStart.GetText(textDocument.EndPoint);
            // Arrange the text.
            string outputFileText;
            StringArranger stringArranger = new StringArranger(string.IsNullOrWhiteSpace(options.Value.NArrangeConfigLocation) ? null : options.Value.NArrangeConfigLocation, new OutputPaneLogger(this));
            bool success = stringArranger.Arrange(document.FullName, inputFileText, out outputFileText);
            if (success) {
                // Overwrite the file.
                ArrangeFileOptions arrangeFileOptions =
                    (options.Value.ArrangeFileFormat ? ArrangeFileOptions.Format : 0) |
                    (options.Value.ArrangeFileKeepMarkers ? ArrangeFileOptions.KeepMarkers : 0) |
                    (options.Value.ArrangeFileNormalizeNewlines ? ArrangeFileOptions.NormalizeNewlines : 0) |
                    (options.Value.ArrangeFileTabsSpaces ? ArrangeFileOptions.TabsSpaces : 0)
                    ;
                editStart.ReplaceText(textDocument.EndPoint, outputFileText, (int)arrangeFileOptions);
            }
        }

        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();
            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs) {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidNArrangeVSCmdSet, (int)PkgCmdIDList.cmdidArrangeFile);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }
            // Add events
            rdt.Value.Advise(new RunningDocTableEvents(this));
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            NArrangeDocument(dte.Value.ActiveDocument);
        }
    }
}