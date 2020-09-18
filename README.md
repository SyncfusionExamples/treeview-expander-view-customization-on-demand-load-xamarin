# How to customize the expander view when loading on demand in Xamarin.Forms TreeView (SfTreeView)

You can customize the load indicator by using custom expander icon and [SfBusyIndicator](https://help.syncfusion.com/xamarin/busy-indicator/overview) in Xamarin.Forms [SfTreeView](https://help.syncfusion.com/xamarin/treeview/overview). Also, you can customize load indicator based on level.

You can also refer to the following documents regarding custom expander icon,

[https://www.syncfusion.com/kb/10289/how-to-expand-and-collapse-treeview-node-using-image-instead-expander](https://www.syncfusion.com/kb/10289/how-to-expand-and-collapse-treeview-node-using-image-instead-expander)

You can also refer the following article.
https://www.syncfusion.com/kb/11934/how-to-customize-the-expander-view-when-loading-on-demand-in-xamarin-forms-treeview 

**XAML: Expander View – BusyIndicator as load indicator and Image used for expand and collapse**

Load custom expander icon and **BusyIndicator** in the same column and change the visibility based on [TreeViewNode](https://help.syncfusion.com/cr/xamarin/Syncfusion.TreeView.Engine.TreeViewNode.html) properties using [Converters](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/data-binding/converters).

``` xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:TreeViewXamarin"
             xmlns:treeview="clr-namespace:Syncfusion.XForms.TreeView;assembly=Syncfusion.SfTreeView.XForms"
             xmlns:sfbusyindicator="clr-namespace:Syncfusion.SfBusyIndicator.XForms;assembly=Syncfusion.SfBusyIndicator.XForms"
             x:Class="TreeViewXamarin.MainPage" Padding="0,20,0,0">
    <ContentPage.BindingContext>
        <local:MusicInfoRepository x:Name="viewModel"/>
    </ContentPage.BindingContext>
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:ExpanderIconVisibilityConverter x:Key="ExpanderIconVisibilityConverter"/>
            <local:ExpanderIconConverter x:Key="ExpanderIconConverter" />
            <local:IndicatorColorConverter x:Key="IndicatorColorConverter" />
        </ResourceDictionary>
    </ContentPage.Resources>
    <ContentPage.Behaviors>
        <local:Behavior/>
    </ContentPage.Behaviors>
    <ContentPage.Content>
        <treeview:SfTreeView x:Name="treeView" ExpanderWidth="0" ItemTemplateContextType="Node" LoadOnDemandCommand="{Binding TreeViewOnDemandCommand}" ItemsSource="{Binding Menu}">
            <treeview:SfTreeView.ItemTemplate>
                <DataTemplate>
                    <Grid x:Name="grid" Padding="5,5,5,5" BackgroundColor="White">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="50" />
                            <ColumnDefinition Width="*" />
                        </Grid.ColumnDefinitions>
                        <Grid Grid.Column="1" Padding="1,0,0,0" VerticalOptions="Center">
                            <Label LineBreakMode="NoWrap" TextColor="Black" Text="{Binding Content.ItemName}" FontSize="20" VerticalTextAlignment="Center"/>
                        </Grid>
                        <Grid >
                            <Image Source="{Binding IsExpanded, Converter={StaticResource ExpanderIconConverter}}"
                                   IsVisible="{Binding HasChildNodes, Converter={StaticResource ExpanderIconVisibilityConverter}}"
                                   VerticalOptions="Center" HorizontalOptions="Center" HeightRequest="35" WidthRequest="35">
                                <Image.GestureRecognizers>
                                    <TapGestureRecognizer Tapped="TapGestureRecognizer_Tapped" />
                                </Image.GestureRecognizers>
                            </Image>
                            <Grid IsVisible="{Binding Content.IsInAnimation, Mode=TwoWay}">
                                <sfbusyindicator:SfBusyIndicator x:Name="grid1" TextColor="{Binding Level, Converter={StaticResource IndicatorColorConverter}}" 
                                                                 IsBusy="True" Margin="2" BackgroundColor="White" ViewBoxHeight="25" ViewBoxWidth="25" HeightRequest="32" AnimationType="SingleCircle"/>
                            </Grid>
                        </Grid>
                    </Grid>
                </DataTemplate>
            </treeview:SfTreeView.ItemTemplate>
        </treeview:SfTreeView>
    </ContentPage.Content>
</ContentPage>
```

**C#:**  **Handled the BusyIndicator visibility based on the IsInAnimation model property**

Update the **IsInAnimation** property in the execute method of the [LoadOnDemandCommand](https://help.syncfusion.com/cr/xamarin/Syncfusion.XForms.TreeView.SfTreeView.html#Syncfusion_XForms_TreeView_SfTreeView_LoadOnDemandCommand).

``` c#
namespace TreeViewXamarin
{
    public class MusicInfoRepository
    {
        public ICommand TreeViewOnDemandCommand{ get; set; }

        public MusicInfoRepository()
        {
            TreeViewOnDemandCommand = new Command(ExecuteOnDemandLoading, CanExecuteOnDemandLoading);
        }

        private bool CanExecuteOnDemandLoading(object sender)
        {
            var hasChildNodes = ((sender as TreeViewNode).Content as MusicInfo).HasChildNodes;
            if (hasChildNodes)
                return true;
            else
                return false;
        }

        private void ExecuteOnDemandLoading(object obj)
        {
            var node = obj as TreeViewNode;

            // Skip the repeated population of child items when every time the node expands.
            if (node.ChildNodes.Count > 0)
            {
                node.IsExpanded = true;
                return;
            }

            MusicInfo musicInfo = node.Content as MusicInfo;
            musicInfo.IsInAnimation = true;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(500);
                var items = GetSubMenu(musicInfo.ID);
                // Populating child items for the node in on-demand
                node.PopulateChildNodes(items);
                if (items.Count() > 0)
                    node.IsExpanded = true;

                musicInfo.IsInAnimation = false;
            });
        }
    }
}
```

**C#**

The expander icon visibility handled based on the [HasChildNodes](https://help.syncfusion.com/cr/xamarin/Syncfusion.TreeView.Engine.TreeViewNode.html#Syncfusion_TreeView_Engine_TreeViewNode_HasChildNodes).

``` c#
namespace TreeViewXamarin
{
    public class ExpanderIconVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!(bool)value) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
```
**C#: Load indicator color customization**

Converter to customize the load indicator color based on the [TreeViewNode.Level](https://help.syncfusion.com/cr/xamarin/Syncfusion.TreeView.Engine.TreeViewNode.html#Syncfusion_TreeView_Engine_TreeViewNode_Level) property.

``` c#
namespace TreeViewXamarin
{
    public class IndicatorColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? Color.Red : Color.Green;
        }
    }
}
```
**Output**

![CustomExpanderTreeView](https://github.com/SyncfusionExamples/treeview-expander-view-customization-on-demand-load-xamarin/blob/master/ScreenShot/CustomExpanderTreeView.gif)
