Imports System.IO
Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call importFichiers1()
        Call ImportListeDossier1()
        MsgBox("Import OK")
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim dernierchoix = ListBox1.SelectedItems(ListBox1.SelectedItems.Count - 1).ToString()
        TextBox2.Text = dernierchoix

        If ListBox1.SelectedItems.Count > 1 Then
            VideoPlayer1.Ctlcontrols.stop()
            VideoPlayer1.URL = Nothing
            If Path.GetExtension(TextBox2.Text) = ".mp4" Then Exit Sub
        End If

        If Path.GetExtension(TextBox2.Text) = ".jpg" Then
            PictureBox1.Show()

            Dim img As Image = Image.FromFile(TextBox1.Text & "\" & TextBox2.Text)
            PictureBox1.Image = New Bitmap(img)
            img.Dispose()

            VideoPlayer1.Hide()
            VideoPlayer1.Ctlcontrols.stop()
            VideoPlayer1.URL = Nothing
        ElseIf Path.GetExtension(ListBox1.SelectedItem.ToString) = ".mp4" Then
            PictureBox1.Hide()

            VideoPlayer1.Show()
            VideoPlayer1.URL = TextBox1.Text & "\" & ListBox1.SelectedItem
            VideoPlayer1.Ctlcontrols.play()
        End If

        'refresh list dossiers
        Call ImportListeDossier1()

    End Sub

    Sub ImportListeDossier1()
        ListBox3.Items.Clear()

        For Each foundDirectory In Directory.GetDirectories(TextBox1.Text, ".", SearchOption.TopDirectoryOnly)
            Dim dossier As String = System.IO.Path.GetFileName(foundDirectory)
            ListBox3.Items.Add(dossier)
        Next

    End Sub
    Sub ImportFichiers1()
        'raz par precaution
        ListBox1.Items.Clear()

        'On boucle sur les fichiers et on ajoute a la liste
        Dim di As New IO.DirectoryInfo(TextBox1.Text)
        Dim aryFi As IO.FileInfo() = di.GetFiles("*", SearchOption.TopDirectoryOnly)
        Dim fi As IO.FileInfo

        For Each fi In aryFi
            ListBox1.Items.Add(fi.Name)
        Next
    End Sub
    Private Sub PictureBox1_DoubleClick(sender As Object, e As EventArgs) Handles PictureBox1.DoubleClick
        Process.Start(TextBox1.Text & "\" & ListBox1.SelectedItem)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'On check si ca existe, au cas ou on le cree
        If (Not System.IO.Directory.Exists(TextBox1.Text & "\" & TextBox3.Text)) Then
            System.IO.Directory.CreateDirectory(TextBox1.Text & "\" & TextBox3.Text)
        End If
        ListBox3.Items.Add(TextBox3.Text)
        TextBox3.Text = Nothing
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim path As String = TextBox1.Text & "\" & ListBox3.SelectedItem
        System.IO.Directory.Delete(path, True)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        If ListBox1.SelectedItems.Count > 1 Then
            VideoPlayer1.Ctlcontrols.stop()
        End If

        For Each itm As String In ListBox1.SelectedItems
            Dim path As String = TextBox1.Text & "\" & itm.ToString
            System.IO.File.Delete(path)
        Next

        PictureBox1.Image = Nothing
        VideoPlayer1.URL = Nothing

        Call importFichiers1()

        If ListBox1.Items.Count = 0 Then
            ListBox1.ClearSelected()
        Else
            ListBox1.SelectedIndex = 0
            ListBox3.SelectedItem = TextBox4.Text
        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        VideoPlayer1.settings.volume = 100
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If ListBox3.SelectedItems.Count = 0 Then Exit Sub

        VideoPlayer1.URL = Nothing

        For Each itm As String In ListBox1.SelectedItems
            Dim cheminoriginal As String = TextBox1.Text & "\" & itm.ToString
            Dim cheminfinal As String = TextBox1.Text & "\" & ListBox3.SelectedItem & "\" & itm.ToString

            On Error Resume Next
            File.Move(cheminoriginal, cheminfinal)
            On Error GoTo 0
        Next

        Call importFichiers1()

        If ListBox1.Items.Count = 0 Then
            ListBox1.ClearSelected()
        Else
            ListBox1.SelectedIndex = 0
            ListBox3.SelectedItem = TextBox4.Text
        End If
    End Sub

    Private Sub ListBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox3.SelectedIndexChanged
        Dim dernierchoix = ListBox3.SelectedItems(ListBox3.SelectedItems.Count - 1).ToString()
        TextBox4.Text = dernierchoix
    End Sub
End Class
