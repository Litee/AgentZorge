using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Resolve;

namespace AgentZorge
{
    [Language(typeof (CSharpLanguage))]
    public class MoqSuggestMocksForParameters : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            CodeCompletionType codeCompletionType = context.BasicContext.CodeCompletionType;
            return codeCompletionType == CodeCompletionType.SmartCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            if (context.TerminatedContext == null)
                return false;
            ISymbolTable symbolTable = this.GetSymbolTable(context);
            if (symbolTable == null)
                return false;
            this.GetLookupItemsFromSymbolTable(symbolTable, collector, context);
            //            collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(proposedName));
            //            collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(proposedName + "Mock"));
            return true;
        }
    }
}