<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SimpleDownloadCounter.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <a href="Download?filename=file1.png">file1.png</a><br />
    <a href="Download?filename=file2.bmp">file2.bmp</a><br />
    <a href="Download?filename=adt-bundle-windows-x86-20130729.zip">bigfile.zip</a>
    </div>
    </form>
</body>
</html>
