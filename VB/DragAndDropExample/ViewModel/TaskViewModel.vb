Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace DragAndDropExample
	Friend Class TaskViewModel

		Private _List As ObservableCollection(Of TaskList)

		Public ReadOnly Property List() As ObservableCollection(Of TaskList)
			Get
				If _List Is Nothing Then
					_List = New ObservableCollection(Of TaskList)()
					For i As Integer = 0 To 9
						Dim taskList As New TaskList() With {.TaskGroup = "Group" & i, .GroupNumber = i, .List = New ObservableCollection(Of Task)()}
						For j As Integer = 0 To 5
							taskList.List.Add(New Task() With {.Name = "Task" & j, .Date = New Date(2014, 10, (New Random()).Next(1, 31)), .IsCompleted = j Mod 2 <> 0})
						Next j
						_List.Add(taskList)
					Next i
				End If
				Return _List
			End Get
		End Property

		Public Class Task
			Public Property Name() As String
			Public Property [Date]() As Date
			Public Property IsCompleted() As Boolean
		End Class

		Public Class TaskList
			Public Property TaskGroup() As String
			Public Property GroupNumber() As Integer
			Public Property List() As ObservableCollection(Of Task)
		End Class
	End Class
End Namespace
