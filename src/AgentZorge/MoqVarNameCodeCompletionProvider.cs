using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Naming;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Settings;
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
            IReferenceName referenceName = null;
            var localVarDeclaration = identifier.Parent as ILocalVariableDeclaration;
            ScopeKind localSelfScoped = ScopeKind.Common;
            var elementKinds = NamedElementKinds.Locals;
            if (localVarDeclaration != null)
            {
                referenceName = localVarDeclaration.ScalarTypeName;
            }
            else
            {
                var fieldDeclaration = identifier.Parent as IFieldDeclaration;
                if (fieldDeclaration != null)
                {
                    referenceName = fieldDeclaration.ScalarTypeName;
                    localSelfScoped = ScopeKind.TypeAndNamespace;
                    if (fieldDeclaration.IsStatic)
                    {
                        if (fieldDeclaration.GetAccessRights().Has(AccessRights.PRIVATE))
                        {
                            elementKinds = fieldDeclaration.IsReadonly ? NamedElementKinds.PrivateStaticReadonly : NamedElementKinds.PrivateStaticFields;
                        }
                        else
                        {
                            elementKinds = fieldDeclaration.IsReadonly ? NamedElementKinds.StaticReadonly : NamedElementKinds.PublicFields;
                        }
                    }
                    else
                    {
                        if (fieldDeclaration.GetAccessRights().Has(AccessRights.PRIVATE))
                        {
                            elementKinds = NamedElementKinds.PrivateInstanceFields;
                        }
                        else
                        {
                            elementKinds = NamedElementKinds.PublicFields;
                        }
                    }
                }
            }
            if (referenceName != null)
            {
                ResolveResultWithInfo referenceNameResolveResult = referenceName.Reference.Resolve();
                var resolveResultWithInfo = referenceNameResolveResult.DeclaredElement as IClass;
                if (resolveResultWithInfo != null && resolveResultWithInfo.GetClrName().FullName == "Moq.Mock`1")
                {
                    string nameIdentifier = referenceName.ShortName;
                    ITypeArgumentList typeArgumentList = referenceName.TypeArgumentList;
                    IList<IType> typeArguments = typeArgumentList.TypeArguments;
                    if (typeArguments.Count == 1)
                    {
                        IType typeArgument = typeArguments[0];
                        var genericTypeResolveResult = typeArgument.GetScalarType().Resolve();
                        NamingManager namingManager = typeArgument.GetPsiServices().Naming;
                        var suggestionOptions = new SuggestionOptions();
                        if (genericTypeResolveResult.IsEmpty)
                        {
                            var proposedName = namingManager.Suggestion.GetDerivedName(typeArgument.GetPresentableName(CSharpLanguage.Instance), elementKinds, localSelfScoped, CSharpLanguage.Instance, suggestionOptions, referenceName.GetSourceFile());
                            collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(proposedName));
                            collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(proposedName + "Mock"));
                        }
                        else
                        {
                            var proposedName = namingManager.Suggestion.GetDerivedName(genericTypeResolveResult.DeclaredElement, elementKinds, localSelfScoped, CSharpLanguage.Instance, suggestionOptions, referenceName.GetSourceFile());
                            collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(proposedName));
                            collector.AddToTop(context.LookupItemsFactory.CreateTextLookupItem(proposedName + "Mock"));
                        }
                    }
                }
            }
        }
    }
}