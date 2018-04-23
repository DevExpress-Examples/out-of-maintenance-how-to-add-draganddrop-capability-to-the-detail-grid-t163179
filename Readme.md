# How to add DragAndDrop capability to the detail grid


<p>DXGrid natively supports <strong>Drag&Drop</strong> for detail grids starting with <strong>v17.2</strong>. It is sufficient to enable the <a href="https://documentation.devexpress.com/WPF/DevExpress.Xpf.Grid.DataViewBase.AllowDragDrop.property">DataViewBase.AllowDragDrop</a> option to activate the drag-and-drop functionality. Please see the <a href="https://documentation.devexpress.com/WPF/11346/Controls-and-Libraries/Data-Grid/Drag-and-Drop">Drag-and-Drop</a> help topic for more information.<br><br><br>This example demonstrates how to add the DragAndDrop capability to the detail grid. Use it if you are working with a version earlier than v17.2.<br>The example contains a custom<strong> GridDragDropManager</strong> class where the <strong>CalcDraggingRows</strong> method is overridden to obtain a selected row. Also, the manager's <strong>DragOver</strong> and <strong>Drop</strong> events are handled. The target position to which a selected row should be dropped is calculated in the Drop event handler.</p>

<br/>


