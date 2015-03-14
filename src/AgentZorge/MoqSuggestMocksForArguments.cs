using System.Linq;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
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
            var table = GetSymbolTable(context);
            table.ForAllSymbolInfos(info =>
            {
                var declaredElement = info.GetDeclaredElement();
                var type = declaredElement.Type();
                if (type != null)
                {
                    if (type.GetClassType().ConvertToString() == "Class:Moq.Mock`1")
                    {
                        var typeParameter = TypesUtil.GetTypeArgumentValue(type, 0);
                        if (context.ExpectedTypesContext.ExpectedITypes.Any(x => typeParameter.IsExplicitlyConvertibleTo(x.Type, ClrPredefinedTypeConversionRule.INSTANCE)))
                        {
                            collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(info.ShortName + ".Object"));
                        }
                    }
                }
            });
            return true;
        }
    }
}