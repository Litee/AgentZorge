using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentZorge
{
    internal static class MoqHelper
    {
        [CanBeNull]
        public static IInvocationExpression GetMockedMethodExpressionFromSetupMethod([CanBeNull] IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null)
                return null;
            if (invocationExpression.Reference == null)
                return null;
            var resolveResult = invocationExpression.Reference.Resolve();
            var method = resolveResult.DeclaredElement as IMethod;
            if (method == null || method.ShortName != "Setup")
                return null;
            var methodReturnTypeName = method.ReturnType.GetLongPresentableName(CSharpLanguage.Instance);
            if (methodReturnTypeName != "Moq.Language.Flow.ISetup<T>" && methodReturnTypeName != "Moq.Language.Flow.ISetup<T,TResult>")
                return null;
            var methodArguments = invocationExpression.ArgumentList.Arguments;
            if (methodArguments.Count != 1)
                return null;
            var lambdaExpression = methodArguments[0].Value as ILambdaExpression;
            if (lambdaExpression == null)
                return null;
            return lambdaExpression.BodyExpression as IInvocationExpression;
        }
    }
}
