using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

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

        private void AddLookupItemsNew([NotNull] CSharpCodeCompletionContext context, [NotNull] GroupedItemsCollector collector)
        {
            if (context.TerminatedContext == null)
                return;
            var identifier = context.TerminatedContext.TreeNode as IIdentifier;
            var mockedMethodArgument = identifier
                .GetParentSafe<IReferenceExpression>()
                .GetParentSafe<ICSharpArgument>();
            if (mockedMethodArgument == null)
                return;
            var mockedMethodInvocationExpression = mockedMethodArgument
                .GetParentSafe<IArgumentList>()
                .GetParentSafe<IInvocationExpression>();
            if (mockedMethodInvocationExpression == null)
                return;
            var setupMethodInvocationExpression = mockedMethodInvocationExpression
                .GetParentSafe<ILambdaExpression>()
                .GetParentSafe<IArgument>()
                .GetParentSafe<IArgumentList>()
                .GetParentSafe<IInvocationExpression>();
            if (setupMethodInvocationExpression == null || !setupMethodInvocationExpression.IsMoqSetupMethod())
                return;
            int argumentIndex = mockedMethodArgument.IndexOf();
            if (argumentIndex == 0 && mockedMethodInvocationExpression.Reference != null)
            {
                var mockedMethodResolved = mockedMethodInvocationExpression.Reference.Resolve();
                var declaredElements = Enumerable.Repeat(mockedMethodResolved.DeclaredElement, 1)
                    .Concat(mockedMethodResolved.Result.Candidates)
                    .Where(x => x != null);
                var methods = declaredElements
                    .OfType<IMethod>()
                    .Where(x => x.Parameters.Count() > 1)
                    .ToList();
                methods.ForEach(method =>
                    {
                        var parameter = method.Parameters.Select(x => "It.IsAny<" + x.Type.GetPresentableName(CSharpLanguage.Instance) + ">()");
                        collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(string.Join(", ", parameter)));
                    });
            }
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