using System.Collections.Generic;
using JetBrains.Annotations;
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

        public MoqIncompatibleCallbackParametersAnalysis([NotNull] IDaemonProcess daemonProcess)
        {
            _daemonProcess = daemonProcess;
        }

        public bool InteriorShouldBeProcessed([NotNull] JetBrains.ReSharper.Psi.Tree.ITreeNode element)
        {
            return true;
        }

        public void ProcessAfterInterior([NotNull] JetBrains.ReSharper.Psi.Tree.ITreeNode element)
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
            if (callbackMethodReturnType == null || callbackMethodReturnType.GetLongPresentableName(CSharpLanguage.Instance) != "Moq.Language.Flow.ICallbackResult")
                return;
            var invokedExpression = callbackInvocationExpression.InvokedExpression as IReferenceExpression;
            if (invokedExpression == null)
                return;
            var setupOrReturnInvocationExpression = invokedExpression.QualifierExpression as IInvocationExpression;
            if (setupOrReturnInvocationExpression == null || setupOrReturnInvocationExpression.Reference == null)
                return;
            var setupOrReturnResolveResult = setupOrReturnInvocationExpression.Reference.Resolve();
            var setupOrReturnMethodDeclaration = setupOrReturnResolveResult.DeclaredElement as IMethod;
            if (setupOrReturnMethodDeclaration == null)
                return;
            IInvocationExpression setupInvocationExpression = null;
            if (setupOrReturnMethodDeclaration.ShortName == "Returns")
            {
                var invokedExpression2 = setupOrReturnInvocationExpression.InvokedExpression as IReferenceExpression;
                if (invokedExpression2 == null)
                    return;
                setupInvocationExpression = invokedExpression2.QualifierExpression as IInvocationExpression;
            }
            else
            {
                setupInvocationExpression = setupOrReturnInvocationExpression;
            }
            var mockMethodInvocationExpression = MoqHelper.GetMockedMethodExpressionFromSetupMethod(setupInvocationExpression);
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

        public void ProcessBeforeInterior([NotNull] JetBrains.ReSharper.Psi.Tree.ITreeNode element)
        {
        }

        public bool ProcessingIsFinished
        {
            get { return _daemonProcess.InterruptFlag; }
        }

        [NotNull]
        public List<HighlightingInfo> Highlights
        {
            get { return _highlights; }
        }
    }
}
