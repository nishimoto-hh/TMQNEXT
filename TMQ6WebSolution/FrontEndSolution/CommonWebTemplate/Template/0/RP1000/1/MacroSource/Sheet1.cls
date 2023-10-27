VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Sheet1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = True
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'
' 機能シート：シート固有処理：保全活動
' 更新日　　：2023/02/09 ATTS
'
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
Dim SelectedRange As Range

'製造権限
Private Const Cell_Manufacturing As String = "A1"
'保全権限
Private Const Cell_Maintenance As String = "B1"
'保全履歴表示切替フラグ
Private Const Cell_ControlFlag As String = "CQ1"
'保全依頼 開始列
Private Const Col_Start_Request As String = "AP"
'保全依頼 終了列
Private Const Col_End_Request As String = "BN"
'保全依頼 列範囲
Private Const Col_Request As String = Col_Start_Request & ":" & Col_End_Request
'保全計画 開始列
Private Const Col_Start_Plan As String = "BO"
'保全計画 終了列
Private Const Col_End_Plan As String = "BW"
'保全履歴 開始列
Private Const Col_Start_History As String = "BX"
'保全履歴（個別工場） 終了列
Private Const Col_End_HistoryIndividual As String = "DQ"
'保全計画～保全履歴（個別工場） 列範囲
Private Const Col_Plan_History As String = Col_Start_Plan & ":" & Col_End_HistoryIndividual
'保全履歴 列範囲
Private Const Col_History As String = Col_Start_History & ":CP"
'保全履歴（個別工場） 列範囲
Private Const Col_HistoryIndividual As String = "CQ:" & Col_End_HistoryIndividual
'保全計画 発生日列
Private Const Col_OccurrenceDate As String = "BP"
'保全履歴（個別工場） 発生日列
Private Const Col_HistoryIndividual_OccurrenceDate As String = "CX"
'保全履歴（個別工場） 予備品(有無)ID列
Private Const Col_HistoryIndividual_PartsExistence As String = "DJ"
'保全依頼入力有無フラグ
Private Const Col_Request_Input_Flg As String = "DR"
'保全計画入力有無フラグ
Private Const Col_Plan_Input_Flg As String = "DS"
'保全履歴入力有無フラグ
Private Const Col_History_Input_Flg As String = "DT"

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

    '===以下、シート固有処理===
    
    '保全計画の発生日が変更された場合、保全履歴（個別工場）の発生日に同じ値を設定
    If Not Intersect(Target, Columns(Col_OccurrenceDate)) Is Nothing Then
        Dim targetValue As String
        targetValue = Target.Value
        Range(Col_HistoryIndividual_OccurrenceDate & Target.row).Value = targetValue
    End If
    
    '値が変更された際に各情報の入力有無フラグを設定する（アプリ側の入力チェックで使用する）
    If Target.row >= Input_RowNo_Start Then
        '一時的にイベントを無効化(値を設定すると無限ループになるため)
        Application.EnableEvents = False
        
        '保全依頼情報
        Dim request As Range
        Set request = Range(Col_Start_Request & Target.row & ":" & Col_End_Request & Target.row)
        If WorksheetFunction.CountBlank(request) = request.Count Then
            '保全依頼情報の項目に入力が無い場合
            Range(Col_Request_Input_Flg & Target.row).Value = 0
        Else
            '保全依頼情報のいずれかの項目に入力がある場合
            Range(Col_Request_Input_Flg & Target.row).Value = 1
        End If
        
        '保全計画情報
        Dim plan As Range
        Set plan = Range(Col_Start_Plan & Target.row & ":" & Col_End_Plan & Target.row)
        If WorksheetFunction.CountBlank(plan) = plan.Count Then
            '保全計画情報の項目に入力が無い場合
            Range(Col_Plan_Input_Flg & Target.row).Value = 0
        Else
            '保全計画情報のいずれかの項目に入力がある場合
            Range(Col_Plan_Input_Flg & Target.row).Value = 1
        End If
        
        '保全履歴情報
        '保全履歴（個別工場）の発生日の列番号を取得
        Dim colNum As Integer
        colNum = ColConv(Col_HistoryIndividual_OccurrenceDate)
        
        '保全履歴（個別工場）の発生日は、保全計画の発生日と同じ値が自動設定されるので、入力有無のチェックからは除外する
        Dim history As Range
        '保全履歴の１つめの列～保全履歴（個別工場）の発生日の前の列まで
        Set history = Range(Col_Start_History & Target.row & ":" & ColConv(colNum - 1) & Target.row)
        If WorksheetFunction.CountBlank(history) = history.Count Then
            '指定範囲の項目に入力が無い場合
            
            '保全履歴（個別工場）の発生日の次の列～保全履歴（個別工場）の最後の列まで
            Set history = Range(ColConv(colNum + 1) & Target.row & ":" & Col_End_HistoryIndividual & Target.row)
            '予備品(有無)ID列が0(選択なし)の場合は、入力無しと扱う
            Dim partsExistenceFlg As String
            partsExistenceFlg = Range(Col_HistoryIndividual_PartsExistence & Target.row).Value
            Dim cnt As Integer
            cnt = 0
            If partsExistenceFlg = "0" Then
                cnt = 1
            End If
            If WorksheetFunction.CountBlank(history) + cnt = history.Count Then
                '指定範囲の項目に入力が無い場合
                Range(Col_History_Input_Flg & Target.row).Value = 0
            Else
                '指定範囲のいずれかの項目に入力がある場合
                Range(Col_History_Input_Flg & Target.row).Value = 1
            End If
        Else
            '指定範囲のいずれかの項目に入力がある場合
            Range(Col_History_Input_Flg & Target.row).Value = 1
        End If
        
        'イベントを有効に戻す
        Application.EnableEvents = True
    End If

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
' 保全活動シート 列の表示切替
'***************************************
Public Sub ChangeColumnDisplay()
On Error GoTo ErrHandler

    '製造権限を取得
    Dim manufacturing As Integer
    manufacturing = Range(Cell_Manufacturing).Value
    '保全権限を取得
    Dim maintenance As Integer
    maintenance = Range(Cell_Maintenance).Value
    '保全履歴表示切替フラグを取得(NULL：一般・個別どちらも表示 or 0：一般工場のみ or 1：個別工場のみ)
    Dim controlFlag As String
    controlFlag = Range(Cell_ControlFlag).Value
    
    If manufacturing <> 1 Then
        '製造権限が無い場合、保全依頼の列を非表示
        Columns(Col_Request).Hidden = True
    End If
    If maintenance <> 1 Then
        '保全権限が無い場合、保全計画～保全履歴（個別工場）の列を非表示
        Columns(Col_Plan_History).Hidden = True
    Else
        If controlFlag = 0 Then
            '保全履歴一般工場のみの場合、保全履歴（個別工場）の列を非表示
            Columns(Col_HistoryIndividual).Hidden = True
        ElseIf controlFlag = 1 Then
            '保全履歴個別工場のみの場合、保全履歴の列を非表示
            Columns(Col_History).Hidden = True
        End If
    End If
    
    Exit Sub

ErrHandler:
    Exit Sub
End Sub

'***************************************
'列変換(数字→アルファベット、アルファベット→数字)
'***************************************
Private Function ColConv(var As Variant) As Variant
On Error GoTo ErrHandler
    Dim temp As String
     
    If IsNumeric(var) = True Then
      '数値の場合はAddress取得し、アルファベットのみを返す
      temp = Cells(1, CLng(var)).Address(RowAbsolute:=False, ColumnAbsolute:=False)
      ColConv = Left(temp, Len(temp) - 1)
    Else
      'アルファベットだったら行番号を返す
      ColConv = Range(var & "1").Column
    End If
    
    Exit Function

ErrHandler:
    Exit Function
End Function
