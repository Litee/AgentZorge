using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
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
            if (!callbackInvocationExpression.IsMoqCallbackMethod() && !callbackInvocationExpression.IsMoqGenericReturnsMethod())
                return;
            var invokedExpression = callbackInvocationExpression.InvokedExpression as IReferenceExpression;
            if (invokedExpression == null)
                return;
            var setupOrReturnInvocationExpression = invokedExpression.QualifierExpression as IInvocationExpression;
            IInvocationExpression setupInvocationExpression;
            // Setup(...).Callback(...) or Setup(...).Returns(...)
            if (setupOrReturnInvocationExpression.IsMoqSetupMethod())
            {
                setupInvocationExpression = setupOrReturnInvocationExpression;
            }
            // Setup(...).Returns(...).Callback(...)
            else if (setupOrReturnInvocationExpression.IsMoqGenericReturnsMethod())
            {
                var invokedExpression2 = setupOrReturnInvocationExpression.InvokedExpression as IReferenceExpression;
                if (invokedExpression2 == null)
                    return;
                setupInvocationExpression = invokedExpression2.QualifierExpression as IInvocationExpression;
            }
            else
            {
                return;
            }
            var targetMethod = setupInvocationExpression.GetMockedMethodFromSetupMethod();
            if (targetMethod == null)
                return;
            var callbackArguments = callbackInvocationExpression.ArgumentList.Arguments;
            if (callbackArguments.Count != 1)
                return;
            var callbackLambdaExpression = callbackArguments[0].Value as ILambdaExpression;
            if (callbackLambdaExpression == null)
                return;
            var callbackLambdaParameterDeclarations = callbackLambdaExpression.ParameterDeclarations;
            if (callbackLambdaParameterDeclarations.Count > 0)
            {
                if (targetMethod.Item1.Parameters.Count != callbackLambdaParameterDeclarations.Count)
                {
                    _highlights.Add(new HighlightingInfo(callbackInvocationExpression.ArgumentList.GetHighlightingRange(), new AgentZorgeHighlighting(callbackInvocationExpression, string.Format("Invalid number of parameters in Callback method. Expected: {0}. Found: {1}.", targetMethod.Item1.Parameters.Count, callbackLambdaParameterDeclarations.Count))));
                }
                else
                {
                    var targetTypeNames = new List<string>();
                    var usedTypeNames = new List<string>();
                    var typesAreCompatible = true;
                    for (int i = 0; i < targetMethod.Item1.Parameters.Count; i++)
                    {
                        var targetParameter = targetMethod.Item1.Parameters[i];
                        var targetParameterTypeName = targetMethod.Item2 == null ? targetParameter.Type.GetLongPresentableName(CSharpLanguage.Instance) : targetMethod.Item2.Apply(targetParameter.Type).GetLongPresentableName(CSharpLanguage.Instance);
                        var callbackLambdaParameterTypeName = callbackLambdaParameterDeclarations[i].DeclaredElement.Type.GetLongPresentableName(CSharpLanguage.Instance);
                        targetTypeNames.Add(targetParameterTypeName);
                        usedTypeNames.Add(callbackLambdaParameterTypeName);
                        if (targetParameterTypeName != callbackLambdaParameterTypeName)
                        {
                            typesAreCompatible = false;
                        }
                    }
                    if (!typesAreCompatible)
                    {
                        var tooltip = string.Format("Incompatible parameter types in Callback method: Expected: ({0}). Found: ({1}).", string.Join(", ", targetTypeNames), string.Join(", ", usedTypeNames));
                        _highlights.Add(new HighlightingInfo(callbackInvocationExpression.ArgumentList.GetHighlightingRange(), new AgentZorgeHighlighting(callbackInvocationExpression, tooltip)));
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
