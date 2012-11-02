Imports System
Imports System.Data.Sql
Imports System.Data.SqlTypes
Imports System.Runtime.InteropServices
Imports Microsoft.SqlServer.Server
Imports DM.Build.Yukon.Attributes


<Serializable()> _
<StructLayout(LayoutKind.Sequential)> _
<SqlUserDefinedType(Format.Native)> _
Public Class $safeitemrootname$
  Implements INullable


  Public Overrides Function ToString() As String
    Dim s = "null"
    'enter code

    Return s
  End Function

  Public ReadOnly Property IsNull() As Boolean Implements INullable.IsNull
    Get
      'enter code to decide what to return
      Return True
    End Get
  End Property

Public Shared ReadOnly Property Null As $safeitemname$
    Get
    Dim h as $safeitemrootname$ = New $safeitemrootname$()
      Return h
    End Get
  End Property
Public Shared Function Parse(ByVal s as SqlString) As $safeitemrootname$ 
    If (s.IsNull Or s.Value.ToLower() = "null") Then
      Return Null
    End If
  Dim u As $safeitemrootname$ = new $safeitemrootname$()
    'enter code


    Return u
  End Function

End Class

