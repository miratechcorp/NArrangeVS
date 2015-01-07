using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NArrangeVS.Events
{
    internal class RunningDocTableEvents : IVsRunningDocTableEvents3
    {
        private NArrangeVSPackage package;

        public RunningDocTableEvents(NArrangeVSPackage package)
        {
            this.package = package;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeSave(uint docCookie)
        {
            if (package.options.Value.ArrangeFileOnSave) {
                RunningDocumentInfo runningDocumentInfo = package.rdt.Value.GetDocumentInfo(docCookie);
                EnvDTE.Document document = package.dte.Value.Documents.OfType<EnvDTE.Document>().SingleOrDefault(x => x.FullName == runningDocumentInfo.Moniker);
                if ((document != null) && package.fileRegex.Value.IsMatch(document.FullName)) {
                    package.NArrangeDocument(document);
                }
            }
            return VSConstants.S_OK;
        }
    }
}