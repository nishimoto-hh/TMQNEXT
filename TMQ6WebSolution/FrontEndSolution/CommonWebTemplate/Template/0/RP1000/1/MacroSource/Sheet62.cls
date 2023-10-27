VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Sheet62"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = True
'********************************************************************************
'
' 機能シート：標準処理
' 更新日　　：2023/02/09 ATTS
'
'********************************************************************************
Dim SelectedRange As Range

'***************************************
' 入力チェックボタン_クリック
'***************************************
Private Sub Btn_InputCheck_Click()
    Call modCommon.CheckMain
End Sub

'***************************************
' コンボボックス変更
'***************************************
Private Sub ComboBox1_Change()
On Error GoTo ErrHandler
    Dim valColNo As Long
    
    valColNo = GetValColNo(ActiveSheet.Index, SelectedRange)

    'アクティブセル取得
    If ActiveCell Is Nothing Then
        Exit Sub
    Else
        Set SelectedRange = ActiveCell
    End If

    '選択されたリストをセルへ表示
    Call modCommon.ComboBox_Change(valColNo, ComboBox1, SelectedRange)
    
    Exit Sub

ErrHandler:
    'イベントを有効に戻す
    Application.EnableEvents = True
    Exit Sub
End Sub

'***************************************
' リスト変更
'***************************************
Private Sub ListBox1_Change()
On Error GoTo ErrHandler
    
    'セルの列番号取得
    Dim valColNo As Long
    'アクティブセル取得
    If ActiveCell Is Nothing Then
        Exit Sub
    Else
        Set SelectedRange = ActiveCell
    End If
    'アクティブセルの列番号取得
    valColNo = GetValColNo(ActiveSheet.Index, SelectedRange)

    Call modCommon.ListBox_Change(valColNo, ListBox1, SelectedRange)

    Exit Sub

ErrHandler:
    'イベントを有効に戻す
    Application.EnableEvents = True
    Exit Sub
End Sub

'***************************************
' 変更
'***************************************
Private Sub Worksheet_Change(ByVal Target As Range)
On Error GoTo ErrHandler
    
    'セルの列タイプ取得
    Dim cellType As String
    cellType = GetCellType(ActiveSheet.Index, Target)

    'セルの列番号取得
    Dim valColNo As Long
    valColNo = GetValColNo(ActiveSheet.Index, Target)

    'アクティブセル取得
    If ActiveCell Is Nothing Then
        Exit Sub
    Else
        Set SelectedRange = ActiveCell
    End If

    '選択シートにコンボボックスを設定
    Call modCommon.SetComboBox(cellType, valColNo, Target, ActiveSheet.Index, SelectedRange, Me.Name)

    '拡張項目があれば表示
    Call modCommon.GetKakuchoCheck(Target, SelectedRange.row, ActiveSheet.Index)

    '行コピー時、KEY項目をクリア
    Call modCommon.SetKeyClear(ActiveSheet.Index, Target)

    Exit Sub

ErrHandler:
    'イベントを有効に戻す
    Application.EnableEvents = True
    Exit Sub
End Sub

'***************************************
' シート選択
'***************************************
Private Sub Worksheet_SelectionChange(ByVal Target As Range)
On Error GoTo ErrHandler

    '部品 非表示
    ComboBox1.Visible = False
    ListBox1.Visible = False

    'セルの列タイプ取得
    Dim cellType As String
    cellType = GetCellType(ActiveSheet.Index, Target)

    'アクティブセル取得
    If ActiveCell Is Nothing Then
        Exit Sub
    Else
        Set SelectedRange = ActiveCell
    End If

    '選択シートにタイプ毎にセルの設定を行う
    Call modCommon.SetCellType(cellType, ComboBox1, ListBox1, Target, ActiveSheet.Index, SelectedRange)

    '拡張項目があれば表示
    Call modCommon.GetKakuchoCheck(Target, SelectedRange.row, ActiveSheet.Index)

    Exit Sub

ErrHandler:
    'イベントを有効に戻す
    Application.EnableEvents = True
    Exit Sub
End Sub





