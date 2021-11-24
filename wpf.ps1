
[System.Reflection.Assembly]::LoadWithPartialName("PresentationFramework") | Out-Null

function Import-Xaml{
 [xml]$xaml = Get-Content -Path C:\Taurus\wpfps1\window.xaml
 $manager = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xaml.NameTable
 $manager.AddNamespace("x","http://schemas.microsoft.com/winfx/2006/xaml");
 $xamlReader = New-Object System.Xml.XmlNodeReader $xaml
 [Windows.Markup.XamlReader]::Load($xamlReader)
}

$Window = Import-Xaml

$Label = $Window.FindName("Label")
$Button = $Window.FindName("Button")

$Button.add_Click({
 $Label.Content = "Hello"
})

$Window.ShowDialog()