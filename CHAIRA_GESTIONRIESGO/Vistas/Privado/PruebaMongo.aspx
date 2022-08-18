<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PruebaMongo.aspx.cs" Inherits="CHAIRA_GESTIONRIESGO.Vistas.Privado.PruebaMongo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script>
        var changeArchivo = function () {
            if (App.TArchivoSoporteMeta.fileInputEl.dom.files[0].size <= 5242880 && App.TArchivoSoporteMeta.fileInputEl.dom.files[0].size > 0) {
                return true;
            } else {
                Ext.Msg.notify('Información', 'El tamaño del archivo no puede exceder 5mb');
                App.TArchivoSoporteMeta.reset();
                return false;
            }
        };
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server" />

            <ext:Panel
                ID="Window1"
                runat="server"
                Frame="false"
                AutoScroll="true"
                Border="false">
                <Items>
                    <%-- Inicio guardar archivo --%>
                    <ext:FileUploadField runat="server" ID="TArchivoSoporteMeta" FieldLabel="<b>Archivo máx. 5 Mb (.rar, .zip, .pdf, .doc, .docx, .png, .jpg) *</b>" LabelAlign="Top" AllowBlank="false" Flex="1">
                        <Listeners>
                            <Change Fn="changeArchivo" />
                        </Listeners>
                    </ext:FileUploadField>
                    <ext:Button runat="server" Hidden="false" ID="BArchivo" Text="Agregar archivo" UI="Default" Icon="Add">
                        <DirectEvents>
                            <Click OnEvent="AgregarArchivo_Click">
                                <EventMask ShowMask="true" Msg="Guardando archivo" />
                                <ExtraParams>
                                    <%--<ext:Parameter Name="data" Value="App.FPSoporteMetaProducto.getValues()" Mode="Raw" />--%>
                                    <ext:Parameter Name="data" Value="1" Mode="Value" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:Button>


                    <%-- Fin guardar archivo --%>
                    <ext:Button ID="Button1" runat="server" Icon="PageWhiteAcrobat" ToolTip="Consultar Mongo" Text="Consultar">
                        <DirectEvents>
                            <Click OnEvent="VerSoporte">
                                <EventMask Msg="Consultando...." ShowMask="true" Target="CustomTarget" CustomTarget="PSoportes" />
                                <ExtraParams>
                                    <%--<ext:Parameter Name="MONID" Value="record.data.SOMP_MONGOID" Mode="Raw" />--%>
                                    <ext:Parameter Name="MONID" Value="1" Mode="Value" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Window runat="server"   ID="WinVerAdjunto" Hidden="true" Icon="Zoom" Width="500" Height="500" Modal="true" Title="Soporte de la Meta" AutoScroll="true" Layout="card" MarginSpec="200 200 0 0" >
                        <Items>
                            <ext:Panel runat="server" ID="PanelContenedor1">
                                <Loader runat="server" Mode="Frame" />
                            </ext:Panel>
                            <ext:Container ID="Contenedor2" runat="server" AutoScroll="true">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Center" Pack="Center" />
                                </LayoutConfig>
                                <Items>
                                    <ext:Image runat="server" ID="ImagenAdjunto" />
                                </Items>
                            </ext:Container>
                        </Items>
                        <Buttons>
                            <ext:Button runat="server" Icon="DoorOut" Text="Cerrar" Handler="App.WinVerAdjunto.hide();" />
                        </Buttons>
                    </ext:Window>
                    <%--<ext:Panel ID="Panel1" Frame="true" runat="server" Height="500" Weight="300" Title="Ver archivo" Border="true">
                                <Items></Items>
                            </ext:Panel>--%>
                </Items>
            </ext:Panel>

        </div>
    </form>
</body>
</html>