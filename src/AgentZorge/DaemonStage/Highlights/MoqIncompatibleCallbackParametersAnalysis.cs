using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentZorge.DaemonStage.Highlights
{
    public class MoqIncompatibleCallbackParametersAnalysis : IRecursiveElementProcessor
    {
        private readonly IDaemonProcess _daemonProcess;
        private readonly List<HighlightingInfo> _highlights = new List<HighlightingInfo>();

        public MoqIncompatibleCallbackParametersAnalysis(IDaemonProcess daemonProcess)
        {
            _daemonProcess = daemonProcess;
        }

        public bool InteriorShouldBeProcessed(JetBrains.ReSharper.Psi.Tree.ITreeNode element)
        {
            return true;
        }

        public void ProcessAfterInterior(JetBrains.ReSharper.Psi.Tree.ITreeNode element)
        {
            var callbackInvocationExpression = element as IInvocationExpression;
            if (callbackInvocationExpression == null)
                return;
            var callbackArguments = callbackInvocationExpression.ArgumentList.Arguments;
            if (callbackArguments.Count != 1)
                return;
            var callbackLambdaExpression = callbackArguments[0].Value as ILambdaExpression;
            if (callbackLambdaExpression == null)
                return;
            var callbackLambdaParameterDeclarations = callbackLambdaExpression.ParameterDeclarations;
            if (callbackInvocationExpression.Reference == null)
                return;
            var callbackResolveResult = callbackInvocationExpression.Reference.Resolve();
            var callbackMethodDeclaration = callbackResolveResult.DeclaredElement as IMethod;
            if (callbackMethodDeclaration == null || callbackMethodDeclaration.ShortName != "Callback")
                return;
            var callbackMethodReturnType = callbackMethodDeclaration.ReturnType.GetScalarType();
            if (callbackMethodReturnType == null)
                return;
            if (callbackMethodReturnType.GetClrName().FullName != "Moq.Language.Flow.ICallbackResult")
                return;
            var invokedExpression = callbackInvocationExpression.InvokedExpression as IReferenceExpression;
            if (invokedExpression == null)
                return;
            var setupInvocationExpression = invokedExpression.QualifierExpression as IInvocationExpression;
            if (setupInvocationExpression == null)
                return;
            if (setupInvocationExpression.Reference == null)
                return;
            var setupResolveResult = setupInvocationExpression.Reference.Resolve();
            var setupMethodDeclaration = setupResolveResult.DeclaredElement as IMethod;
            if (setupMethodDeclaration == null || setupMethodDeclaration.ShortName != "Setup")
                return;
            var setupMethodReturnType = setupMethodDeclaration.ReturnType.GetScalarType();
            if (setupMethodReturnType == null)
                return;
            if (setupMethodReturnType.GetClrName().FullName != "Moq.Language.Flow.ISetup`1")
                return;
            var setupArguments = setupInvocationExpression.ArgumentList.Arguments;
            if (setupArguments.Count != 1)
                return;
            var setupLambdaExpression = setupArguments[0].Value as ILambdaExpression;
            if (setupLambdaExpression == null)
                return;
            var mockMethodInvocationExpression = setupLambdaExpression.BodyExpression as IInvocationExpression;
            if (mockMethodInvocationExpression == null || mockMethodInvocationExpression.Reference == null)
                return;
            var targetMethodResolveResult = mockMethodInvocationExpression.Reference.Resolve();
            var targetMethod = targetMethodResolveResult.DeclaredElement as IMethod;
            if (targetMethod == null)
                return;
            if (targetMethod.Parameters.Count != callbackLambdaParameterDeclarations.Count)
            {
                _highlights.Add(new HighlightingInfo(callbackInvocationExpression.ArgumentList.GetHighlightingRange(), new AgentZorgeHighlighting(string.Format("Invalid number of parameters in Callback method. Expected: {0}. Found: {1}.", targetMethod.Parameters.Count, callbackLambdaParameterDeclarations.Count))));
            }
            else
            {
                for (int i = 0; i < targetMethod.Parameters.Count; i++)
                {
                    var targetParameter = targetMethod.Parameters[i];
                    var targetParameterTypeName = targetParameter.Type.GetLongPresentableName(CSharpLanguage.Instance);
                    var callbackLambdaParameterTypeName = callbackLambdaParameterDeclarations[i].DeclaredElement.Type.GetLongPresentableName(CSharpLanguage.Instance);
                    if (targetParameterTypeName != callbackLambdaParameterTypeName)
                    {
                        _highlights.Add(new HighlightingInfo(callbackInvocationExpression.ArgumentList.GetHighlightingRange(), new AgentZorgeHighlighting(string.Format("Incompatible parameter types in Callback method: Expected: {0}. Found: {1}.", targetParameterTypeName, callbackLambdaParameterTypeName))));
                    }
                }
            }
        }

        public void ProcessBeforeInterior(JetBrains.ReSharper.Psi.Tree.ITreeNode element)
        {
        }

        public bool ProcessingIsFinished
        {
            get { return _daemonProcess.InterruptFlag; }
        }

        public List<HighlightingInfo> Highlights
        {
            get { return _highlights; }
        }
    }
}
