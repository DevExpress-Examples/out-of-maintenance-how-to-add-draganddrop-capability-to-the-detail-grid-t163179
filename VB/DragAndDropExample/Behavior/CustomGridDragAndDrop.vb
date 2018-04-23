Imports DevExpress.Mvvm.UI
Imports DevExpress.Xpf.Core.Native
Imports DevExpress.Xpf.Grid
Imports DevExpress.Xpf.Grid.DragDrop
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Threading

Namespace DragAndDropExample
	Friend Class CustomGridDragAndDrop
		Inherits GridDragDropManager

		Public TableView As TableView
		Public SourceRow As RowControl

		Public Sub New()
			AddHandler Drop, AddressOf CustomGridDragAndDrop_Drop
			AddHandler DragOver, AddressOf CustomGridDragAndDrop_DragOver
		End Sub

		Private Sub CustomGridDragAndDrop_DragOver(ByVal sender As Object, ByVal e As GridDragOverEventArgs)
			Dim result = VisualTreeHelper.HitTest(Me.View, Mouse.GetPosition(View))

			If result IsNot Nothing AndAlso result.VisualHit IsNot Nothing Then
				Dim hitRow = TryCast(LayoutTreeHelper.GetVisualParents(HitElement).Where(Function(row) TypeOf row Is GroupGridRow OrElse TypeOf row Is RowControl).FirstOrDefault(), FrameworkElement)

				If hitRow IsNot Nothing Then
					Dim rowData = TryCast(hitRow.DataContext, RowData)
					e.AllowDrop = Equals(rowData.Row.GetType(), e.DraggedRows(0).GetType())
					e.Handled = True
				Else
					e.AllowDrop = False
					e.Handled = True
				End If
			End If
		End Sub

		Protected Overrides Function CanStartDrag(ByVal e As MouseButtonEventArgs) As Boolean
			SourceRow = TryCast(LayoutTreeHelper.GetVisualParents(TryCast(e.OriginalSource, DependencyObject)).Where(Function(row) TypeOf row Is RowControl).FirstOrDefault(), RowControl)

			Return MyBase.CanStartDrag(e)
		End Function

		Private Sub CustomGridDragAndDrop_Drop(ByVal sender As Object, ByVal e As GridDropEventArgs)
			If TableView IsNot Nothing Then
				DropRow(e)
				TableView = Nothing
				e.Handled = True
			End If
		End Sub

		Protected Overrides Function CalcDraggingRows(ByVal e As DevExpress.Xpf.Core.IndependentMouseEventArgs) As IList
			If SourceRow IsNot Nothing Then
				Dim rowData = TryCast(SourceRow.DataContext, RowData)

				TableView = TryCast(rowData.View, TableView)

				Return New List(Of Object) From {rowData.Row}
			Else
				Return Nothing
			End If
		End Function


		Public Sub DropRow(ByVal args As GridDropEventArgs)

			Dim currentRow = TryCast(LayoutTreeHelper.GetVisualParents(HitElement).Where(Function(row) TypeOf row Is GroupGridRow OrElse TypeOf row Is RowControl).FirstOrDefault(), FrameworkElement)

			Dim rowData = TryCast(currentRow.DataContext, RowData)

			Dim tableSource = TryCast(TableView.Grid.ItemsSource, IList)
			Dim dataView = TryCast(rowData.View, TableView)
			Dim dataSource = TryCast(dataView.Grid.ItemsSource, IList)
			Dim rowIndex = dataView.Grid.GetListIndexByRowHandle(args.TargetRowHandle)

			Dim draggingRow = DraggingRows(0)

			If dataSource Is Nothing OrElse dataSource(0).GetType() IsNot draggingRow.GetType() OrElse dataSource.Count = 0 OrElse draggingRow Is rowData.Row Then
				Return
			End If

			If Equals(tableSource, dataSource) AndAlso tableSource.IndexOf(DraggingRows(0)) <= rowIndex Then
				rowIndex -= 1
			End If

			tableSource.Remove(DraggingRows(0))

			If Not dataView.Grid.IsGrouped Then
				If args.DropTargetType = DropTargetType.InsertRowsBefore Then
					dataSource.Insert(rowIndex, draggingRow)
				ElseIf args.DropTargetType = DropTargetType.InsertRowsAfter Then
					dataSource.Insert(Math.Min(rowIndex + 1, dataSource.Count), draggingRow)
				Else
'INSTANT VB WARNING: Instant VB cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
					If Mouse.GetPosition(currentRow).Y >= currentRow.ActualHeight / 2 Then
						dataSource.Insert(Math.Min(rowIndex + 1, dataSource.Count), draggingRow)
					Else
						dataSource.Insert(rowIndex, draggingRow)
					End If
				End If
			ElseIf args.DropTargetType = DropTargetType.InsertRowsIntoGroup OrElse dataView.Grid.IsGrouped Then
				Dim value = dataView.Grid.GetCellValue(rowData.RowHandle.Value, dataView.Grid.SortInfo(0).FieldName)

				TypeDescriptor.GetProperties(draggingRow.GetType())(dataView.Grid.SortInfo(0).FieldName).SetValue(draggingRow, value)

				If args.DropTargetType = DropTargetType.InsertRowsAfter Then
					dataSource.Insert(Math.Min(rowIndex + 1, dataSource.Count), draggingRow)
				Else
					dataSource.Insert(rowIndex, draggingRow)
				End If

				dataView.Grid.RefreshData()
			End If
		End Sub

		Private Shared IsSameGroupInfo As MethodInfo = GetType(GridDragDropManager).GetMethod("IsSameGroup", BindingFlags.NonPublic Or BindingFlags.Instance)
		Protected Function IsSameGroup(ByVal sourceManager As DragDropManagerBase, ByVal groupInfos() As GroupInfo, ByVal hitElement As DependencyObject) As Boolean
			Return DirectCast(IsSameGroupInfo.Invoke(Me, New Object() { sourceManager, groupInfos, hitElement }), Boolean)
		End Function

		Private Shared GetGroupInfosInfo As MethodInfo = GetType(GridDragDropManager).GetMethod("GetGroupInfos", BindingFlags.NonPublic Or BindingFlags.Instance)
		Protected Function GetGroupInfos(ByVal rowHandle As Integer) As GroupInfo()
			Return DirectCast(GetGroupInfosInfo.Invoke(Me, New Object() { rowHandle }), GroupInfo())
		End Function

		Private Shared SetReorderDropInfoInfo As MethodInfo = GetType(GridDragDropManager).GetMethod("SetReorderDropInfo", BindingFlags.NonPublic Or BindingFlags.Instance)
		Protected Sub SetReorderDropInfo(ByVal sourceManager As DragDropManagerBase, ByVal insertRowHandle As Integer, ByVal hitElement As DependencyObject)
			SetReorderDropInfoInfo.Invoke(Me, New Object() { sourceManager, insertRowHandle, hitElement })
		End Sub

		Private Shared SetMoveToGroupRowDropInfoInfo As MethodInfo = GetType(GridDragDropManager).GetMethod("SetMoveToGroupRowDropInfo", BindingFlags.NonPublic Or BindingFlags.Instance)

		Protected Sub SetMoveToGroupRowDropInfo(ByVal sourceManager As DragDropManagerBase, ByVal insertRowHandle As Integer, ByVal hitElement As DependencyObject)
			SetReorderDropInfoInfo.Invoke(Me, New Object() { sourceManager, insertRowHandle, hitElement })
		End Sub

		Protected Function IsGrouped(ByVal grid As GridControl) As Boolean
			Return grid.GroupCount > 0
		End Function
		Protected Function IsSorted(ByVal grid As GridControl) As Boolean
			Return grid.SortInfo.Count > 0
		End Function
		Protected Function IsSortedButNotGrouped(ByVal grid As GridControl) As Boolean
			Return IsSorted(grid) AndAlso Not IsGrouped(grid)
		End Function
		Protected Function ShouldReorderGroup(ByVal grid As GridControl) As Boolean
			Return grid.SortInfo.Count <= grid.GroupCount
		End Function

		Protected Overrides Sub PerformDropToViewCore(ByVal sourceManager As DragDropManagerBase)
			Dim hitInfo As GridViewHitInfoBase = GetHitInfo(HitElement)
			If BanDrop(hitInfo.RowHandle, hitInfo, sourceManager, DropTargetType.None) Then
				ClearDragInfo(sourceManager)
				Return
			End If
            PerformDropToView(sourceManager, TryCast(hitInfo, TableViewHitInfo), LastPosition, AddressOf SetReorderDropInfo, Function() AddressOf SetMoveToGroupRowDropInfo, AddressOf SetAddRowsDropInfo)
		End Sub

		Protected Function GetSourceGrid() As GridControl
			Dim parentRow = If(CType(LayoutHelper.FindParentObject(Of RowControl)(HitElement), FrameworkElement), CType(LayoutHelper.FindParentObject(Of GridRowContent)(HitElement), FrameworkElement))
			Return If(parentRow IsNot Nothing, TryCast(CType(parentRow.DataContext, RowData).View.DataControl, GridControl), GridControl)
		End Function

		Protected Overloads Sub PerformDropToView(ByVal sourceManager As DragDropManagerBase, ByVal hitInfo As TableViewHitInfo, ByVal pt As Point, ByVal reorderDelegate As MoveRowsDelegate, ByVal groupDelegateExtractor As Func(Of Boolean, MoveRowsDelegate), ByVal addRowsDelegate As MoveRowsDelegate)
			Dim insertRowHandle As Integer = hitInfo.RowHandle
			Dim grid = GetSourceGrid()

			If GridControl.IsGroupRowHandle(insertRowHandle) Then
				groupDelegateExtractor(True)(sourceManager, insertRowHandle, HitElement)
				Return
			End If
			If IsSortedButNotGrouped(grid) OrElse hitInfo.HitTest = TableViewHitTest.DataArea Then
				If sourceManager.DraggingRows.Count > 0 AndAlso GetDataAreaElement(HitElement) IsNot Nothing Then ' && !ReferenceEquals(sourceManager, this)
					addRowsDelegate(sourceManager, insertRowHandle, HitElement)
				Else
					ClearDragInfo(sourceManager)
				End If

				Return
			End If
			If insertRowHandle = GridControl.InvalidRowHandle OrElse insertRowHandle = GridControl.AutoFilterRowHandle OrElse insertRowHandle = GridControl.NewItemRowHandle Then
				ClearDragInfo(sourceManager)
				Return
			End If
			If GridControl.GroupCount > 0 Then
				Dim groupRowHandle As Integer = GridControl.GetParentRowHandle(insertRowHandle)
				If ShouldReorderGroup(grid) Then
					If Not IsSameGroup(sourceManager, GetGroupInfos(groupRowHandle), HitElement) Then
						groupDelegateExtractor(False)(sourceManager, groupRowHandle, HitElement)
					End If
					reorderDelegate(sourceManager, insertRowHandle, HitElement)
				Else
					groupDelegateExtractor(True)(sourceManager, groupRowHandle, HitElement)
				End If
			Else
				reorderDelegate(sourceManager, insertRowHandle, HitElement)
			End If
		End Sub

		Public Delegate Sub MoveRowsDelegate(ByVal sourceManager As DragDropManagerBase, ByVal targetRowHandle As Integer, ByVal hitElement As DependencyObject)
	End Class
End Namespace
