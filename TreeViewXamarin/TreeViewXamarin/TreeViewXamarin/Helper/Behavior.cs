using Syncfusion.XForms.TreeView;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TreeViewXamarin
{
    public class Behavior : Behavior<ContentPage>
    {
        #region Fields

        SfTreeView TreeView;
        #endregion

        #region Overrides

        protected override void OnAttachedTo(ContentPage bindable)
        {
            TreeView = bindable.FindByName<SfTreeView>("treeView");
            TreeView.QueryNodeSize += TreeView_QueryNodeSize;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            TreeView.QueryNodeSize -= TreeView_QueryNodeSize;
            TreeView = null;
            base.OnDetachingFrom(bindable);
        }
        #endregion

        #region Event

        private void TreeView_QueryNodeSize(object sender, QueryNodeSizeEventArgs e)
        {
            e.Height = e.GetActualNodeHeight();
            e.Handled = true;
        }
        #endregion
    }
}
