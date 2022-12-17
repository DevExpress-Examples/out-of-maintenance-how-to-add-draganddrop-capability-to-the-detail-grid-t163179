<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128647985/22.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T163179)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [CustomGridDragAndDrop.cs](./CS/DragAndDropExample/Behavior/CustomGridDragAndDrop.cs) (VB: [CustomGridDragAndDrop.vb](./VB/DragAndDropExample/Behavior/CustomGridDragAndDrop.vb))
* [MainWindow.xaml](./CS/DragAndDropExample/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/DragAndDropExample/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/DragAndDropExample/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/DragAndDropExample/MainWindow.xaml.vb))
* [TaskViewModel.cs](./CS/DragAndDropExample/ViewModel/TaskViewModel.cs) (VB: [TaskViewModel.vb](./VB/DragAndDropExample/ViewModel/TaskViewModel.vb))
<!-- default file list end -->
# How to add DragAndDrop capability to the detail grid


<p>DXGrid natively supports <strong>Drag&Drop</strong> for detail grids starting with <strong>v17.2</strong>. It is sufficient to enable the <a href="https://documentation.devexpress.com/WPF/DevExpress.Xpf.Grid.DataViewBase.AllowDragDrop.property">DataViewBase.AllowDragDrop</a> option to activate the drag-and-drop functionality. Please see the <a href="https://documentation.devexpress.com/WPF/11346/Controls-and-Libraries/Data-Grid/Drag-and-Drop">Drag-and-Drop</a> help topic for more information.<br><br><br>This example demonstrates how to add the DragAndDrop capability to the detail grid. Use it if you are working with a version earlier than v17.2.<br>The example contains a custom<strong> GridDragDropManager</strong> class where the <strong>CalcDraggingRows</strong> method is overridden to obtain a selected row. Also, the manager's <strong>DragOver</strong> and <strong>Drop</strong> events are handled. The target position to which a selected row should be dropped is calculated in the Drop event handler.</p>

<br/>


