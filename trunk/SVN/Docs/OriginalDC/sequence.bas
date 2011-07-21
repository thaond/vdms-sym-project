Sub Main
	Dim trigName As String 
	Dim trigBody As String 
	Dim attrib As AttributeObj 
	Dim crlf As String 
	crlf = Chr(13) + Chr(10) 

	Dim seqName As String
	Dim seqBody As String

	seqName = Right(CurrEntity.TableName, Len(CurrEntity.TableName) - InStr(4, CurrEntity.TableName, "_") )
	seqName = "v2_seq_" + Replace(seqName, "_", "")
	If Len(seqName) > 27 Then
		seqName = Left(seqName, 27)
	End If

	seqBody = "DROP SEQUENCE " + seqName + ";" + crlf
	seqBody = seqBody + "CREATE SEQUENCE " + seqName + " MINVALUE 1 MAXVALUE " + crlf
	seqBody = seqBody + "999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;"

	trigName = Left(CurrEntity.TableName, 25) + "_U_PK"
 
	trigBody = "CREATE OR REPLACE TRIGGER " + trigName + " BEFORE INSERT ON " + CurrEntity.TableName + crlf
	trigBody = trigBody + "REFERENCING OLD AS OLD NEW AS NEW" + crlf 
	trigBody = trigBody + "FOR EACH ROW" + crlf 
	trigBody = trigBody + "BEGIN" + crlf 
 
	trigBody = trigBody + "SELECT " + seqName + ".NEXTVAL "
 
	For Each attrib In CurrEntity.Attributes 
		If attrib.PrimaryKey = True Then 
			trigBody = trigBody + "INTO :NEW." +attrib.ColumnName  + " FROM DUAL;"
			Exit For
		End If 
	Next attrib 
 
	trigBody = trigBody + crlf + "END;"
	 
	'Resultstring outputs the trigger to the DDL script when the Generate Database 
	'wizard is used.  The string variable used to generate the trigger DDL needs to be Set To it.
	resultstring = seqBody + crlf + crlf + trigBody
	 
	'This message box is used to view the SQL when debugging the VB code.  A table has To be selected.
	'MsgBox(seqBody + crlf + crlf + trigBody)
End Sub
