using Aurora.Domain.Shared.Extensions;
using System.ComponentModel.Design;

namespace Aurora.Domain.Shared.Core.Tree;

public interface ITreeModel<TKey, TChildren>
    where TChildren : ITreeModel<TKey, TChildren>
    where TKey : IEquatable<TKey> {
    /// <summary>
    /// 当前id
    /// </summary>
    public TKey Id { get; set; }

    /// <summary>
    /// 父级id
    /// </summary>
    public TKey ParentId { get; set; }

    /// <summary>
    /// 子级数据
    /// </summary>
    public IEnumerable<TChildren> Children { get; set; }
    
    public string CheckArr { get; set; }
}

public static class ITreeModelExtensions {
    public static IEnumerable<TChildren> Build<TKey, TChildren>(this IEnumerable<TChildren> self)
        where TChildren : ITreeModel<TKey, TChildren>
        where TKey : IEquatable<TKey> {
        // var parentIds = self.Where(p => p.ParentId is not null).Select(p => p.ParentId).ToList()
        //     .Union(self.Where(p => p.ParentId is null).Select(p => p.Id));
        // var childrenIds = self.Where(p => p.ParentId is not null).Select(p => p.Id).ToList();
        // parentIds = parentIds.Except(childrenIds).ToList();
        // var children = self.Where(p => childrenIds.Contains(p.Id));
        // var parent = self.Where(p => parentIds.Contains(p.Id));
        // if (children != null && children.Any()) {
        //     Parallel.ForEach(parent, item => {
        //         item.BuildChildren(children);
        //     });
        // }
        // return parent;
        var res = new List<TChildren>();
        if (self.IsNullOrEmpty()) {
            return res;
        }
        
        foreach (var item in self) {
            var parent = self.FirstOrDefault(t => t.Id.Equals(item.ParentId));
            if (parent is null) {
                res.Add(item);
                continue;
            }

            var children = new List<TChildren>();
            if (parent.Children.IsNotNullOrEmpty()) {
                children.AddRange(parent.Children);
            }
            children.Add(item);
            parent.Children = children;
        }

        return res;
    }

    private static void BuildChildren<TKey, TChildren>(
        this ITreeModel<TKey, TChildren> self, IEnumerable<TChildren> children)
        where TChildren : ITreeModel<TKey, TChildren>
        where TKey : IEquatable<TKey> {
        if (self is null || children is null || !children.Any())
            return;

        var parent = children.Where(p => p.ParentId.Equals(self.Id));
        if (parent is null || !parent.Any())
            return;
        self.Children = parent;
        var child = children.Except(parent);
        Parallel.ForEach(parent, item => {
            item.BuildChildren(child);
        });
    }
}