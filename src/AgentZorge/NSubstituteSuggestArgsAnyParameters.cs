using System.Linq;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems.Impl;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp.Rules;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resx.Utils;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentZorge
{
    [Language(typeof (CSharpLanguage))]
    public class NSubstituteSuggestArgsAnyParameters : CSharpItemsProviderBase<CSharpCodeCompletionContext>
    {

        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            CodeCompletionType codeCompletionType = context.BasicContext.CodeCompletionType;
            return codeCompletionType == CodeCompletionType.SmartCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            bool nSubstituteIsSeen = false;
            ISymbolTable table = GetSymbolTable(context);
            if (table != null)
            {
                table.ForAllSymbolInfos(info =>
                {
                    IDeclaredElement declaredElement = info.GetDeclaredElement();
                    if (declaredElement.ConvertToString() == "Class:NSubstitute.Substitute")
                    {
                        nSubstituteIsSeen = true;
                    }
                });
            }
            if (!nSubstituteIsSeen)
            {
                return true;
            }
            if (context.ExpectedTypesContext != null)
            {
                foreach (ExpectedTypeCompletionContextBase.ExpectedIType expectedType in context.ExpectedTypesContext.ExpectedITypes)
                {
                    if (expectedType.Type == null)
                        continue;
                    var typeName = expectedType.Type.GetPresentableName(CSharpLanguage.Instance);
                    var textLookupItem = new TextLookupItem("Arg.Any<" + typeName + ">()");
                    textLookupItem.InitializeRanges(context.CompletionRanges, context.BasicContext);
                    textLookupItem.PlaceTop();
                    collector.Add(textLookupItem);
                }
            }
            if (context.TerminatedContext == null)
                return true;
            var identifier = context.TerminatedContext.TreeNode as IIdentifier;
            var mockedMethodArgument = identifier
                .GetParentSafe<IReferenceExpression>()
                .GetParentSafe<ICSharpArgument>();
            if (mockedMethodArgument == null)
                return true;
            var mockedMethodInvocationExpression = mockedMethodArgument
                .GetParentSafe<IArgumentList>()
                .GetParentSafe<IInvocationExpression>();
            if (mockedMethodInvocationExpression == null)
                return true;
            if (mockedMethodInvocationExpression.Reference != null)
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
                    var parameter = method.Parameters.Select(x => "Arg.Any<" + x.Type.GetPresentableName(CSharpLanguage.Instance) + ">()");
                    var textLookupItem = new TextLookupItem(string.Join(", ", parameter));
                    textLookupItem.InitializeRanges(context.CompletionRanges, context.BasicContext);
                    textLookupItem.PlaceTop();
                    collector.Add(textLookupItem);
                });
            }
            return true;
        }
    }
}