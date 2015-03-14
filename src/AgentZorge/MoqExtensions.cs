using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
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
            if (targetMethodResolveResult.ResolveErrorType == ResolveErrorType.OK)
            {
                return targetMethodResolveResult.DeclaredElement as IMethod;
            }
            return null;
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
            var method = resolveResult.DeclaredElement as IMethod;
            if (method == null || method.ShortName != "Callback")
                return false;
            var containingType = method.GetContainingType();
            var containingClassAsString = containingType.ConvertToString();
            return containingClassAsString == "Interface:Moq.Language.ICallback" || containingClassAsString == "Interface:Moq.Language.ICallback`2";
        }

        public static bool IsMoqReturnsMethod([CanBeNull] this IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null || invocationExpression.Reference == null)
                return false;
            var resolveResult = invocationExpression.Reference.Resolve();
            var method = resolveResult.DeclaredElement as IMethod;
            if (method == null || method.ShortName != "Returns")
                return false;
            var containingType = method.GetContainingType();
            var containingClassAsString = containingType.ConvertToString();
            return containingClassAsString == "Interface:Moq.Language.IReturns`2";
        }
    }
}
