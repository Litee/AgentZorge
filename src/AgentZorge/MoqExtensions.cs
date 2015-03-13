using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resx.Utils;

namespace AgentZorge
{
    internal static class MoqExtensions
    {
        [CanBeNull]
        public static IMethod GetMockedMethodFromSetupMethod([CanBeNull] this IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null || !IsMoqSetupMethod(invocationExpression))
                return null;
            var methodArguments = invocationExpression.ArgumentList.Arguments;
            if (methodArguments.Count != 1)
                return null;
            var lambdaExpression = methodArguments[0].Value as ILambdaExpression;
            if (lambdaExpression == null)
                return null;
            var mockMethodInvocationExpression = lambdaExpression.BodyExpression as IInvocationExpression;
            if (mockMethodInvocationExpression == null || mockMethodInvocationExpression.Reference == null)
                return null;
            var targetMethodResolveResult = mockMethodInvocationExpression.Reference.Resolve();
            return targetMethodResolveResult.DeclaredElement as IMethod;

        }

        public static bool IsMoqSetupMethod([CanBeNull] IDeclaredElement declaredElement)
        {
            var declaredElementAsString = declaredElement.ConvertToString();
            if (declaredElementAsString == "Method:Moq.Mock`1.Setup(System.Linq.Expressions.Expression`1[TDelegate -> System.Action`1[T -> T]] expression)")
                return true;
            if (declaredElementAsString == "Method:Moq.Mock`1.Setup(System.Linq.Expressions.Expression`1[TDelegate -> System.Func`2[T -> T, TResult -> TResult]] expression)")
                return true;
            return false;
        }

        public static bool IsMoqSetupMethod([CanBeNull] this IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null || invocationExpression.Reference == null)
                return false;
            var resolveResult = invocationExpression.Reference.Resolve();
            return IsMoqSetupMethod(resolveResult.DeclaredElement);
        }

        [CanBeNull]
        public static IMethod ToMoqSetupMethod([CanBeNull] this IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null || invocationExpression.Reference == null)
                return null;
            var resolveResult = invocationExpression.Reference.Resolve();
            return IsMoqSetupMethod(resolveResult.DeclaredElement) ? resolveResult.DeclaredElement as IMethod : null;
        }

        public static bool IsMoqCallbackMethod([CanBeNull] this IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null || invocationExpression.Reference == null)
                return false;
            var resolveResult = invocationExpression.Reference.Resolve();
            var declaredElementAsString = resolveResult.DeclaredElement.ConvertToString();
            // Callback method has many overloads, so checking by prefix
            return declaredElementAsString != null && declaredElementAsString.StartsWith("Method:Moq.Language.ICallback.Callback(System.Action");
        }
        public static bool IsMoqReturnsMethod([CanBeNull] this IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null || invocationExpression.Reference == null)
                return false;
            var resolveResult = invocationExpression.Reference.Resolve();
            var declaredElementAsString = resolveResult.DeclaredElement.ConvertToString();
            // Returns method has many overloads, so checking by prefix
            return declaredElementAsString != null && declaredElementAsString.StartsWith("Method:Moq.Language.IReturns`2.Returns(");
        }
    }
}
