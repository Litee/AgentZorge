using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Impl.reflection2.elements.Compiled;
using JetBrains.ReSharper.Psi.Naming;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;
using JetBrains.ReSharper.Psi.Resolve.TypeInference;

namespace AgentZorge
{
    [Language(typeof (CSharpLanguage))]
    public class MoqSetupMethodParameterCodeCompletionProvider : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            CodeCompletionType codeCompletionType = context.BasicContext.CodeCompletionType;
            return codeCompletionType == CodeCompletionType.SmartCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            AddLookupItemsNew(context, collector);
            return true;
        }

        private void AddLookupItemsNew(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            if (context.TerminatedContext == null)
                return;
            var identifier = context.TerminatedContext.TreeNode as IIdentifier;
            if (identifier == null)
                return;
            var referenceExpression = identifier.Parent as IReferenceExpression;
            if (referenceExpression == null)
                return;
            var argument = referenceExpression.Parent as ICSharpArgument;
            if (argument == null)
                return;
            var argumentList = argument.Parent as IArgumentList;
            if (argumentList == null)
                return;
            int argumentIndex = argumentList.Arguments.IndexOf(argument);
            var mockMethodInvocationExpression = argumentList.Parent as IInvocationExpression;
            if (mockMethodInvocationExpression == null)
                return;
            var setupMethodLambdaExpression = mockMethodInvocationExpression.Parent as ILambdaExpression;
            if (setupMethodLambdaExpression == null)
                return;
            var setupMethodArgument = setupMethodLambdaExpression.Parent as IArgument;
            if (setupMethodArgument == null)
                return;
            var setupMethodArgumentList = setupMethodArgument.Parent as IArgumentList;
            if (setupMethodArgumentList == null)
                return;
            var setupMethodInvocationExpression = setupMethodArgumentList.Parent as IInvocationExpression;
            if (setupMethodInvocationExpression == null)
                return;
            if (setupMethodInvocationExpression.Reference == null)
                return;
            var invokedExpression = setupMethodInvocationExpression.InvokedExpression as IReferenceExpression;
            if (invokedExpression == null)
                return;
            if (invokedExpression.NameIdentifier.Name != "Setup")
                return;
            var qualifierExpression = invokedExpression.QualifierExpression as IReferenceExpression;
            if (qualifierExpression == null)
                return;
            var qualifierExpressionResolveResult = qualifierExpression.Reference.Resolve();
            var field = qualifierExpressionResolveResult.DeclaredElement as ITypeOwner;
            if (field == null)
                return;
            var declaredType = field.Type as IDeclaredType;
            if (declaredType == null)
                return;
            var resolveResult = declaredType.Resolve();
            var variableType = resolveResult.DeclaredElement as IClass;
            if (variableType == null)
                return;
            if (variableType == null || variableType.GetClrName().FullName != "Moq.Mock`1")
                return;
            if (context.ExpectedTypesContext == null)
                return;
            foreach (var expectedType in context.ExpectedTypesContext.ExpectedITypes)
            {
                if (expectedType.Type == null)
                    continue;
                var typeName = expectedType.Type.GetPresentableName(CSharpLanguage.Instance);
                collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem("It.IsAny<" + typeName + ">()"));
            }
        }
    }
}