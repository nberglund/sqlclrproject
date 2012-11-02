Imports System
Imports System.Data.Sql
Imports System.Data.SqlTypes
Imports System.Runtime.InteropServices
Imports Microsoft.SqlServer.Server
Imports DM.Build.Yukon.Attributes


<Serializable()> _
<StructLayout(LayoutKind.Sequential)> _
<SqlUserDefinedAggregate(Format.Native)> _
Public Class $safeitemrootname$

  Public Sub Init()

  End Sub


  Public Sub Accumulate(ByVal Value As SqlString)

  End Sub


  Public Sub Merge(ByVal Group As $safeitemrootname$)

  End Sub

  Public Function Terminate() As SqlString

    Return ""
  End Function
End Class
