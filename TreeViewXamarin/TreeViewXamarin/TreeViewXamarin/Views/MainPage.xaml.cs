using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TreeViewXamarin
{
    public partial class MainPage : ContentPage
    {
        #region Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Call back
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var imageIcon = sender as Image;
            var treeViewNode = imageIcon.BindingContext as Syncfusion.TreeView.Engine.TreeViewNode;
            if (treeViewNode.IsExpanded)
                treeView.CollapseNode(treeViewNode);
            else
                treeView.ExpandNode(treeViewNode);
        }
        #endregion
    }
}
