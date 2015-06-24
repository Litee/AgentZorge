using System;
using AgentZorge.DaemonStage.Highlights;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;

namespace AgentZorge.DaemonStage
{
    public class MoqDaemonStageProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _daemonProcess;

        public MoqDaemonStageProcess(IDaemonProcess daemonProcess)
        {
            _daemonProcess = daemonProcess;
        }

        public IDaemonProcess DaemonProcess
        {
            get { return _daemonProcess; }
        }

        public void Execute(Action<DaemonStageResult> committer)
        {
            // Getting PSI (AST) for the file being highlighted
            var sourceFile = _daemonProcess.SourceFile;
            var file = sourceFile.GetPsiServices().Files.GetDominantPsiFile<CSharpLanguage>(sourceFile) as ICSharpFile;
            if (file == null)
                return;

            // Running visitor against the PSI
            var elementProcessor = new MoqIncompatibleCallbackParametersAnalysis(_daemonProcess);
            file.ProcessDescendants(elementProcessor);

            // Checking if the daemon is interrupted by user activity
            if (_daemonProcess.InterruptFlag)
                throw new ProcessCancelledException();

            // Commit the result into document
            committer(new DaemonStageResult(elementProcessor.Highlights));
        }
    }
}