using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
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
            var candidateExistingElements = new List<ISymbolInfo>();
            ISymbolTable table = GetSymbolTable(context);
            if (table != null)
            { 
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
                            if (typeParameter != null && context.ExpectedTypesContext.ExpectedITypes.Select(x => x.Type).Where(x => x != null).Any(x => typeParameter.IsExplicitlyConvertibleTo(x, ClrPredefinedTypeConversionRule.INSTANCE)))
                            {
                                candidateExistingElements.Add(info);
                            }
                        }
                    }
                });
            }
            foreach (ISymbolInfo candidateExistingElement in candidateExistingElements)
            {
                collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(candidateExistingElement.ShortName + ".Object"));
            }
            if (moqIsSeen && !candidateExistingElements.Any())
            {
                foreach (ExpectedTypeCompletionContextBase.ExpectedIType expectedType in context.ExpectedTypesContext.ExpectedITypes)
                {
                    if (expectedType.Type == null)
                        continue;
                    if (expectedType.Type.IsInterfaceType())
                    {
                        string typeName = expectedType.Type.GetPresentableName(CSharpLanguage.Instance);
                        if (candidateExistingElements.Any())
                        {
                            collector.AddToBottom(context.LookupItemsFactory.CreateTextLookupItem("new Mock<" + typeName + ">().Object"));
                        }
                        else
                        {
                            collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem("new Mock<" + typeName + ">().Object"));
                        }
                    }
                }
            }
            return true;
        }
    }
}