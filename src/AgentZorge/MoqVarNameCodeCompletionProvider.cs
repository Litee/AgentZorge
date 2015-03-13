using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Resx.Utils;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace AgentZorge
{
    [Language(typeof (CSharpLanguage))]
    public class MoqVarNameCodeCompletionProvider : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            CodeCompletionType codeCompletionType = context.BasicContext.CodeCompletionType;
            return codeCompletionType == CodeCompletionType.BasicCompletion || codeCompletionType == CodeCompletionType.SmartCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            AddLookupItemsNew(context, collector);
            return true;
        }

        private void AddLookupItemsNew(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            if (context.TerminatedContext == null)
            {
                return;
            }
            var identifier = context.TerminatedContext.TreeNode as IIdentifier;
            if (identifier == null)
            {
                return;
            }
            var localVarDeclaration = identifier.Parent as ILocalVariableDeclaration;
            var fieldDeclaration = identifier.Parent as IFieldDeclaration;
            var regularParameterDeclaration = identifier.Parent as IRegularParameterDeclaration;
            if (localVarDeclaration != null)
            {
                ProcessReferenceName(context, collector, localVarDeclaration.ScalarTypeName, NamedElementKinds.Locals, ScopeKind.Common);
            }
            else if (fieldDeclaration != null)
            {
                if (fieldDeclaration.IsStatic)
                {
                    if (fieldDeclaration.GetAccessRights().Has(AccessRights.PRIVATE))
                    {
                        ProcessReferenceName(context, collector, fieldDeclaration.ScalarTypeName, fieldDeclaration.IsReadonly ? NamedElementKinds.PrivateStaticReadonly : NamedElementKinds.PrivateStaticFields, ScopeKind.TypeAndNamespace);
                    }
                    else
                    {
                        ProcessReferenceName(context, collector, fieldDeclaration.ScalarTypeName, fieldDeclaration.IsReadonly ? NamedElementKinds.StaticReadonly : NamedElementKinds.PublicFields, ScopeKind.TypeAndNamespace);
                    }
                }
                else
                {
                    if (fieldDeclaration.GetAccessRights().Has(AccessRights.PRIVATE))
                    {
                        ProcessReferenceName(context, collector, fieldDeclaration.ScalarTypeName, NamedElementKinds.PrivateInstanceFields, ScopeKind.TypeAndNamespace);
                    }
                    else
                    {
                        ProcessReferenceName(context, collector, fieldDeclaration.ScalarTypeName, NamedElementKinds.PublicFields, ScopeKind.TypeAndNamespace);
                    }
                }
            }
            else if (regularParameterDeclaration != null)
            {
                ProcessReferenceName(context, collector, regularParameterDeclaration.ScalarTypeName, NamedElementKinds.Parameters, ScopeKind.Common);
            }
        }

        private static void ProcessReferenceName(CSharpCodeCompletionContext context, GroupedItemsCollector collector, IReferenceName referenceName, NamedElementKinds elementKinds, ScopeKind localSelfScoped)
        {
            if (referenceName == null)
                return;
            var referenceNameResolveResult = referenceName.Reference.Resolve();
            var referencedElementAsString = referenceNameResolveResult.DeclaredElement.ConvertToString();
            if (referencedElementAsString == "Class:Moq.Mock`1")
            {
                var typeArgumentList = referenceName.TypeArgumentList;
                var typeArguments = typeArgumentList.TypeArguments;
                if (typeArguments.Count == 1)
                {
                    var typeArgument = typeArguments[0];
                    var scalarType = typeArgument.GetScalarType();
                    if (scalarType == null)
                        return;
                    var genericTypeResolveResult = scalarType.Resolve();
                    var namingManager = typeArgument.GetPsiServices().Naming;
                    var suggestionOptions = new SuggestionOptions();
                    string proposedName;
                    if (genericTypeResolveResult.IsEmpty)
                    {
                        proposedName = namingManager.Suggestion.GetDerivedName(typeArgument.GetPresentableName(CSharpLanguage.Instance), elementKinds, localSelfScoped, CSharpLanguage.Instance, suggestionOptions, referenceName.GetSourceFile());
                    }
                    else
                    {
                        proposedName = namingManager.Suggestion.GetDerivedName(genericTypeResolveResult.DeclaredElement, elementKinds, localSelfScoped, CSharpLanguage.Instance, suggestionOptions, referenceName.GetSourceFile());
                    }
                    collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(proposedName));
                    collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(proposedName + "Mock"));
                }
            }
        }
    }
}