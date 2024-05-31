VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Sheet13"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = True
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'
' 機能シート：シート固有処理：保全活動_故障情報
' 更新日　　：2023/02/09 ATTS
'
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
Dim SelectedRange As Range

'故障分析情報表示切替フラグ
Private Const Cell_ControlFlag As String = "A1"
'故障分析情報
Private Const Col_HistoryFailure As String = "BE:BL"
'故障分析情報（個別工場）
Private Const Col_HistoryFailureIndividual As String = "BM:CH"

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

'***************************************
' 保全活動_故障情報シート 列の表示切替
'***************************************
Public Sub ChangeColumnDisplay()
On Error GoTo ErrHandler

    '故障分析情報表示切替フラグを取得(NULL：一般・個別どちらも表示 or 0：一般工場のみ or 1：個別工場のみ)
    Dim controlFlag As String
    controlFlag = Range(Cell_ControlFlag).Value
    
    If controlFlag = 0 Then
        '故障分析一般工場のみの場合、故障分析情報（個別工場）の列を非表示
        Columns(Col_HistoryFailureIndividual).Hidden = True
    ElseIf controlFlag = 1 Then
        '故障分析個別工場のみの場合、故障分析情報の列を非表示
        Columns(Col_HistoryFailure).Hidden = True
    End If
    
    Exit Sub

ErrHandler:
    Exit Sub
End Sub


