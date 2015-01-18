using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace NArrangeVS
{
    [Flags]
    public enum ArrangeFileOptions
    {
        KeepMarkers = vsEPReplaceTextOptions.vsEPReplaceTextKeepMarkers,
        NormalizeNewlines = vsEPReplaceTextOptions.vsEPReplaceTextNormalizeNewlines,
        TabsSpaces = vsEPReplaceTextOptions.vsEPReplaceTextTabsSpaces,
        Format = vsEPReplaceTextOptions.vsEPReplaceTextAutoformat,
    }
}