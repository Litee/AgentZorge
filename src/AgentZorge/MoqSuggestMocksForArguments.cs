using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems.Impl;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp.Rules;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resx.Utils;
using JetBrains.ReSharper.Psi.Util;

namespace AgentZorge
{
    [Language(typeof (CSharpLanguage))]
    public class MoqSuggestMocksForArguments : CSharpItemsProviderBase<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            CodeCompletionType codeCompletionType = context.BasicContext.CodeCompletionType;
            return codeCompletionType == CodeCompletionType.SmartCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            bool moqIsSeen = false;
            ISymbolTable table = GetSymbolTable(context);
            var candidateExistingElements = new List<ISymbolInfo>();
            table.ForAllSymbolInfos(info =>
            {
                IDeclaredElement declaredElement = info.GetDeclaredElement();
                if (declaredElement.ConvertToString() == "Class:Moq.Mock")
                {
                    moqIsSeen = true;
                }
                IType type = declaredElement.Type();
                if (type != null)
                {
                    if (type.GetClassType().ConvertToString() == "Class:Moq.Mock`1")
                    {
                        IType typeParameter = TypesUtil.GetTypeArgumentValue(type, 0);
                        if (typeParameter != null && context.ExpectedTypesContext != null && context.ExpectedTypesContext.ExpectedITypes != null && context.ExpectedTypesContext.ExpectedITypes.Select(x => x.Type).Where(x => x != null).Any(x => typeParameter.IsExplicitlyConvertibleTo(x, ClrPredefinedTypeConversionRule.INSTANCE)))
                        {
                            candidateExistingElements.Add(info);
                        }
                    }
                }
            });
            foreach (ISymbolInfo candidateExistingElement in candidateExistingElements)
            {
                var lookupItem = new TextLookupItem(candidateExistingElement.ShortName + ".Object");
#if RESHARPER9
                lookupItem.InitializeRanges(context.CompletionRanges, context.BasicContext);
#endif
                lookupItem.PlaceTop();
                collector.Add(lookupItem);
            }
            if (moqIsSeen && !candidateExistingElements.Any() && context.ExpectedTypesContext != null)
            {
                foreach (ExpectedTypeCompletionContextBase.ExpectedIType expectedType in context.ExpectedTypesContext.ExpectedITypes)
                {
                    if (expectedType.Type == null)
                        continue;
                    if (expectedType.Type.IsInterfaceType())
                    {
                        string typeName = expectedType.Type.GetPresentableName(CSharpLanguage.Instance);
                        var lookupItem = new TextLookupItem("new Mock<" + typeName + ">().Object");
#if RESHARPER9
                        lookupItem.InitializeRanges(context.CompletionRanges, context.BasicContext);
#endif
                        if (candidateExistingElements.Any())
                        {
                            lookupItem.PlaceBottom();
                        }
                        else
                        {
                            lookupItem.PlaceTop();
                        }
                        collector.Add(lookupItem);
                    }
                }
            }
            return true;
        }
    }
}