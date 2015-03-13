using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentZorge
{
    internal static class ResharperExtensions
    {
        [CanBeNull]
        public static T GetParentSafe<T>([CanBeNull] this ITreeNode treeNode) where T : class
        {
            if (treeNode == null)
                return default(T);
            return treeNode.Parent as T;
        }
    }
}
