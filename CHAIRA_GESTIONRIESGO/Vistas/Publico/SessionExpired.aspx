<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionExpired.aspx.cs" Inherits="CHAIRA_GESTIONRIESGO.Vistas.Publico.SessionExpired" %>

<!DOCTYPE html>

<html >
<head runat="server">
    <title>Plataforma Chaira Universidad de la Amazonia 2011</title>
     <link href="../../Resources/Metro/v3/css/metro.css" rel="stylesheet" />
    <link href="../../Resources/Metro/v3/css/metro-icons.css" rel="stylesheet" />
    <link href="../../Resources/Metro/v3/css/docs.css" rel="stylesheet" />
    <script src="../../Resources/Metro/v3/js/jquery-2.1.3.min.js"></script>
    <script src="../../Resources/Metro/v3/js/metro.js"></script>
    <script src="../../Resources/Metro/v3/js/docs.js"></script>
    <script src="../../Resources/Metro/v3/js/prettify/run_prettify.js"></script>
    <script src="../../Resources/Metro/v3/js/ga.js"></script>
    <script src="../../Resources/Metro/v3/js/select2.min.js"></script>
    <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
    <style type="text/css">
        .imgen
        {
            width:70%;
            height: 70%;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <div align="center">
    
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Recursos/Imagenes/PageError/Expired.png" CssClass="imgen"/>
            <br />    
             <br />      
        <%--<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Logon.aspx">Volver a inicio</asp:HyperLink>--%>
            <a class="button success block-shadow-success text-shadow" href="https://chaira.udla.edu.co/Chaira/Logon.aspx">Volver al inicio de sesión</a>
    </div>
    </form>
</body>
</html>
