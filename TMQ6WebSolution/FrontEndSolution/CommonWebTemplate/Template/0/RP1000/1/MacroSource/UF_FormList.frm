VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} UF_FormList 
   Caption         =   "選択"
   ClientHeight    =   5448
   ClientLeft      =   105
   ClientTop       =   450
   ClientWidth     =   9165.001
   OleObjectBlob   =   "UF_FormList.frx":0000
   StartUpPosition =   1  'オーナー フォームの中央
End
Attribute VB_Name = "UF_FormList"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit

'---------------------------------------------------------------------------------------------------
' 呼び元との受け渡し変数
Private g_FormDate1 As Date                                 ' 現在日付
Private g_intEntMode As Integer                             ' 入力モード(0=セル、1=TextBox)
Private g_objRange As Range                                 ' 対象セル
Private g_LongIdCol As Long                            ' 対象セルのIDの列
'Private g_strName As String                                 ' 対象セルの名称
Private g_objTextBox As MSForms.TextBox                     ' 対象TextBox
Private g_strDateFormat As String                           ' 日付フォーマット(TextBox時)
'---------------------------------------------------------------------------------------------------
' フォーム表示中に保持するモジュール変数
Private g_tblYobi As Variant                                ' 曜日テーブル
Private g_tblDateLabel(44) As New clsUF_Cal5Label1          ' 日付ラベルイベントクラステーブル
Private g_tblFixLabel(11) As New clsUF_Cal5Label2           ' 固定ラベルイベントクラステーブル
Private g_intCurYear As Integer                             ' 現在表示年
Private g_intCurMonth As Integer                            ' 現在表示月
Private g_CurPos As Long                                    ' 現在日付位置
Private g_CurPosF As Long                                   ' 当月月初日位置
Private g_CurPosT As Long                                   ' 当月月末日位置
Private g_swBatch As Boolean                                ' イベント抑制SW
Private g_VisibleYear As Boolean                            ' Conboの年表示スイッチ
Private g_VisibleMonth As Boolean                           ' Comboの月表示スイッチ

'***************************************
'閉じるボタン
'***************************************
Private Sub CommandButton_Close_Click()
    '閉じる
    Unload UF_FormList

End Sub

'***************************************
'検索ボタン
'***************************************
Private Sub CommandButton_Search_Click()
    'テキストボックス入力値で検索
    Call sub_Find

End Sub

'***************************************
'選択ボタン
'***************************************
Private Sub CommandButton_Select_Click()
    
    '選択行を画面表示元へ表示
    Dim sSelectName As String
    If Me.ListBox1.ListIndex >= 0 Then
        sSelectName = Me.ListBox1.List(Me.ListBox1.ListIndex, 3)
        Dim lstWs As Worksheet
        Set lstWs = ActiveSheet
        lstWs.Cells(g_objRange.row, g_objRange.Column).Value = Me.ListBox1.List(Me.ListBox1.ListIndex, 3)
        lstWs.Cells(g_objRange.row, g_LongIdCol).Value = Me.ListBox1.List(Me.ListBox1.ListIndex, 2)
    End If
    
    '画面を閉じる
    Unload UF_FormList

End Sub

'***************************************
'クリアボタン
'***************************************
Private Sub CommandButton_Clear_Click()
    TextBox1.text = ""
    Me.ListBox1.Clear
    
    '初期表示
    Call UserForm_Initialize
End Sub

'***************************************
'リストボックス　ダブルクリック時
'***************************************
Private Sub ListBox1_DblClick(ByVal Cancel As MSForms.ReturnBoolean)
    '選択ボタン処理を行う
    Call CommandButton_Select_Click

End Sub

Private Sub TextBox1_DblClick(ByVal Cancel As MSForms.ReturnBoolean)
'    'テキストボックス入力値で検索
'    Call sub_Find

End Sub

Private Sub TextBox1_KeyUp(ByVal KeyCode As MSForms.ReturnInteger, ByVal Shift As Integer)
'    'テキストボックス入力値で検索
'    Call sub_Find

End Sub

Private Sub UserForm_Initialize()

Debug.Print "UserForm_Initialize() ---- Start:" & Time

    Dim lastRow  As Long
    Dim i As Long
    Dim lstWs As Worksheet
    Set lstWs = Worksheets(SheetName_SelectList)
    lastRow = lstWs.Cells(lstWs.Rows.Count, 1).End(xlUp).row

    Dim ary_d 'リストに設定するデータ用配列
    ary_d = Worksheets(SheetName_SelectList).Range("A1:D" & lastRow)
    With Me.ListBox1
        .ColumnCount = 4               '表示列数
        .ColumnWidths = "0;0;0;600"  '列幅
        .List = ary_d                  '参照範囲
    End With
    
    ary_d = Worksheets(SheetName_SelectList).Range("A1:D" & lastRow)
    With Me.ListBox_ALL
        .ColumnCount = 4               '表示列数
        .ColumnWidths = "0;0;0;600"  '列幅
        .List = ary_d                  '参照範囲
    End With


End Sub

'*************************************************************
' テキストボックスの値をリストから検索しセットする
'*************************************************************

Private Sub sub_Find()
    Dim i As Long
    Dim sKey
    sKey = TextBox1.text
    
    If sKey = "" Then
        'テキストボックス未入力時、全件リストボックス（非表示）より全件表示する
        For i = 0 To ListBox_ALL.ListCount - 1
            ListBox1.AddItem ""
            '
            ListBox1.List(i, 0) = ListBox_ALL.List(i, 0)
            '
            ListBox1.List(i, 1) = ListBox_ALL.List(i, 1)
            '
            ListBox1.List(i, 2) = ListBox_ALL.List(i, 2)
            '名称
            ListBox1.List(i, 3) = ListBox_ALL.List(i, 3)
        Next i
    
    End If
    
    Me.ListBox1.Clear
    
    For i = 0 To ListBox_ALL.ListCount - 1
        
        '入力値と一致した場合、表示
        If InStr(ListBox_ALL.List(i, 3), sKey) > 0 Then
            ListBox1.AddItem ""
            'ID
            ListBox1.List(ListBox1.ListCount - 1, 0) = ListBox_ALL.List(i, 0)
            'ID
            ListBox1.List(ListBox1.ListCount - 1, 1) = ListBox_ALL.List(i, 1)
            '名称
            ListBox1.List(ListBox1.ListCount - 1, 2) = ListBox_ALL.List(i, 2)
            '名称
            ListBox1.List(ListBox1.ListCount - 1, 3) = ListBox_ALL.List(i, 3)

        End If
        
    Next i
    
    For i = 0 To ListBox1.ListCount - 1
        '完全一致の場合、リスト行を選択する
        If ListBox1.List(i, 3) = sKey Then
            ListBox1.ListIndex = i
        End If
    Next i

End Sub

'***************************************************************************************************
' ■■■ プロパティ ■■■
'***************************************************************************************************
' タイトル
'---------------------------------------------------------------------------------------------------
Friend Property Let prpTitle(ByVal strTitle As String)
    Me.Caption = strTitle
End Property

'===================================================================================================
' 入力モード(1=セル、2=TextBox)
'---------------------------------------------------------------------------------------------------
Friend Property Let prpEntMode(ByVal intValue As Integer)
    g_intEntMode = intValue
End Property

'===================================================================================================
' 対象セル(Object)
'---------------------------------------------------------------------------------------------------
Friend Property Set prpRange(ByRef objValue As Range)
    Set g_objRange = objValue
End Property

'===================================================================================================
' 対象セルIDの列
'---------------------------------------------------------------------------------------------------
Friend Property Let prpIdCol(ByVal lngValue As Long)
    g_LongIdCol = lngValue
End Property

'===================================================================================================
' 対象TextBox(Object)
'---------------------------------------------------------------------------------------------------
Friend Property Set prpTextBox(ByRef objValue As MSForms.TextBox)
    Set g_objTextBox = objValue
End Property

'===================================================================================================
' 日付フォーマット(TextBox時)
'---------------------------------------------------------------------------------------------------
Friend Property Let prpDateFormat(ByVal strFormat As String)
    g_strDateFormat = strFormat
End Property



