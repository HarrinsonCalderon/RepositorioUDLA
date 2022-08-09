<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormularioPrueba.aspx.cs" Inherits="CHAIRA_GESTIONRIESGO.Vistas.Privado.FormularioPrueba" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        .columna_grid {
            font: normal 11px arial;
            padding: 2px 5px 2px 5px;
            white-space: normal;
            text-align: justify;
        }
    </style>
    <script>
        var pctChange2 = function (value) {
            if (value == null) {
                value = 0;
            }
            return Ext.String.format(template, (value > 50) ? "green" : "red", value + "%");
        };
        function getFiltro() {
            var value = this.getRawValue();

            if (value && value[0] !== "*" && !Ext.net.FilterHeader.behaviour.getBehaviour("string", value)) {
                return "*" + value;
            }

            return value;
        };

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
</head>
<body>
    <ext:ResourceManager Theme="Crisp" runat="server"></ext:ResourceManager>
    <ext:Viewport Layout="BorderLayout" runat="server">
        <Items>
            <ext:TabPanel runat="server" Region="Center" ID="tab_principal" Layout="BorderLayout">
                <Items>
                    <ext:Panel runat="server"
                                ID="Center"
                                Region="Center" Icon="ChartBarEdit" Title="Reporte"
                                Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="GridPanel_REPORTE" runat="server" AutoScroll="true" MarginSpec="10 10 10 10" Hidden="false" RowLines="true" ColumnLines="true" UI="Primary" Title="<b>Datos - .::Informe General::.</b>" Icon="Database">
                                        <TopBar>
                                            <ext:Toolbar runat="server">
                                                <Items>
                                                    <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                                    <ext:Button ID="Button2" runat="server" Icon="PageExcel" ToolTip="Generar Reporte">
                                                        <DirectEvents>
                                                            <Click OnEvent="Export_Event" Timeout="34000000">
                                                                <EventMask ShowMask="true" Msg="Generando Excel..."></EventMask>
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="tipo_report" Value="1" Mode="Value"></ext:Parameter>
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>

                                                    </ext:Button>
                                                    <ext:Button ID="Button1" runat="server" Icon="PageWhiteAcrobat" ToolTip="Consultar Mongo">
                                                        <DirectEvents>
                                                            <Click onevent="VerSoporte">
                                                                <eventmask msg="Consultando...." showmask="true" target="CustomTarget" customtarget="PSoportes" />
                                                                <extraparams>
                                                                    <%--<ext:Parameter Name="MONID" Value="record.data.SOMP_MONGOID" Mode="Raw" />--%>
                                                                    <ext:Parameter Name="MONID" Value="1" Mode="Value" />
                                                                </extraparams>
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:Button>

                                                    <ext:Button ID="Button3" Hidden="false" runat="server" Text="Generar PDF" Icon="PageWhiteAcrobat">
                                                        <Listeners>
                                                            <Click Handler="App.direct.armaPDF()" />
                                                        </Listeners>
                                                    </ext:Button>

                                                    <ext:Button ID="Button4" Hidden="false" runat="server" Text="enviar correo" Icon="Email">
                                                         <DirectEvents>
                                                            <Click onevent="BEnviarMensaje_Click">
                                                                <eventmask msg="Consultando...." showmask="true" target="CustomTarget" customtarget="PSoportes" />
                                                                <extraparams>
                                                                    <%--<ext:Parameter Name="MONID" Value="record.data.SOMP_MONGOID" Mode="Raw" />--%>
                                                                    <ext:Parameter Name="MONID" Value="1" Mode="Value" />
                                                                </extraparams>
                                                            </Click>
                                                        </DirectEvents>
                                                        
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="Store1" runat="server" PageSize="40">
                                                <Model>
                                                    <ext:Model ID="Model3" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="ID_LINEA"></ext:ModelField>
                                                            <ext:ModelField Name="LINEA"></ext:ModelField>
                                                            <ext:ModelField Name="ID_PROGRAMA"></ext:ModelField>
                                                            <ext:ModelField Name="PROGRAMA"></ext:ModelField>
                                                            <ext:ModelField Name="ID_ESTRATEGIA"></ext:ModelField>
                                                            <ext:ModelField Name="ESTRATEGIA"></ext:ModelField>
                                                            <ext:ModelField Name="ID_OBJETIVO"></ext:ModelField>
                                                            <ext:ModelField Name="CODIFICACION"></ext:ModelField>
                                                            <ext:ModelField Name="OBJETIVO"></ext:ModelField>
                                                            <ext:ModelField Name="INPR_ID"></ext:ModelField>
                                                            <ext:ModelField Name="INDI_OBJETIVO"></ext:ModelField>
                                                            <ext:ModelField Name="INPR_DESIGNACION"></ext:ModelField>
                                                            <ext:ModelField Name="INPR_LINEABASE"></ext:ModelField>
                                                            <ext:ModelField Name="INPR_META"></ext:ModelField>
                                                            <ext:ModelField Name="ACTI_ID"></ext:ModelField>
                                                            <ext:ModelField Name="ACTI_DESCRIPCION"></ext:ModelField>
                                                            <ext:ModelField Name="ACTI_PORCENTAJEAPB"></ext:ModelField>
                                                            <ext:ModelField Name="ACTI_RESPONSABLE"></ext:ModelField>
                                                            <ext:ModelField Name="REAC_ID"></ext:ModelField>
                                                            <ext:ModelField Name="REAC_NOMBRE"></ext:ModelField>
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="ColumnModel3" runat="server">
                                            <Columns>
                                                <ext:RowNumbererColumn ID="RowNumbererColumn3" runat="server" Flex="1"></ext:RowNumbererColumn>
                                                <ext:TemplateColumn ID="TemplateColumn3" runat="server" DataIndex="LINEA" Flex="3" MinWidth="200" Text="<b><center>LÍNEA <br>ESTRATÉGICA </center></b>">
                                                    <Template ID="Template3" runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                        <div class="columna_grid">{LINEA}</div>
                                                    </tpl>
                                                        </Html>
                                                    </Template>

                                                </ext:TemplateColumn>
                                                <ext:TemplateColumn ID="TemplateColumn19" runat="server" DataIndex="PROGRAMA" Flex="3" MinWidth="200" Text="<b><center>NOMBRE DEL <BR>PROGRAMA</center></b>">
                                                    <Template ID="Template19" runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                        <div class="columna_grid">{PROGRAMA}</div>
                                                    </tpl>
                                                        </Html>
                                                    </Template>

                                                </ext:TemplateColumn>

                                                <ext:TemplateColumn ID="TemplateColumn20" runat="server" DataIndex="ESTRATEGIA" Flex="3" MinWidth="200" Text="<b><center>NOMBRE DE LA <BR>ESTRATEGIA</center></b>">
                                                    <Template ID="Template20" runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                        <div class="columna_grid">{ESTRATEGIA}</div>
                                                    </tpl>
                                                        </Html>
                                                    </Template>

                                                </ext:TemplateColumn>
                                                <ext:Column runat="server" DataIndex="CODIFICACION" Flex="2" MinWidth="150" Text="<b><center>CÓDIGO DEL <BR>OBJETIVO</center>  </b>" Align="Center">
                                                </ext:Column>

                                                <ext:TemplateColumn ID="TemplateColumn21" runat="server" DataIndex="OBJETIVO" Flex="3" MinWidth="200" Text="<b><center>NOMBRE DEL <BR>OBJETIVO</center></b>">
                                                    <Template ID="Template21" runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                        <div class="columna_grid">{OBJETIVO}</div>
                                                    </tpl>
                                                        </Html>
                                                    </Template>

                                                </ext:TemplateColumn>

                                                <ext:TemplateColumn ID="TemplateColumn22" runat="server" DataIndex="INDI_OBJETIVO" Flex="3" MinWidth="200" Text="<b><center>INDICADOR <br> PRODUCTO</center></b>">
                                                    <Template ID="Template22" runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                        <div class="columna_grid">{INDI_OBJETIVO}</div>
                                                    </tpl>
                                                        </Html>
                                                    </Template>

                                                </ext:TemplateColumn>

                                                <ext:Column runat="server" DataIndex="INPR_LINEABASE" Flex="1" MinWidth="100" Text="<b><center>LINEA </BR>  BASE</center></b>" Align="Center">
                                                </ext:Column>

                                                <ext:Column runat="server" DataIndex="INPR_META" Flex="1" MinWidth="100" Text="<b><center>LINEA </BR>  META</center></b>" Align="Center">
                                                </ext:Column>

                                                <ext:TemplateColumn ID="TemplateColumn23" runat="server" DataIndex="ACTI_DESCRIPCION" Flex="3" MinWidth="250" Text="<b><center>NOMBRE DE LA <br> ACTIVIDAD</center></b>">
                                                    <Template ID="Template23" runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                        <div class="columna_grid">{ACTI_DESCRIPCION}</div>
                                                    </tpl>
                                                        </Html>
                                                    </Template>

                                                </ext:TemplateColumn>
                                                <ext:Column runat="server" DataIndex="ACTI_PORCENTAJEAPB" Flex="1" MinWidth="110" Text="<b><center>PORCENTAJE </BR>  ACTIVIDAD</center></b>" Align="Center">
                                                    <Renderer Fn="pctChange2"></Renderer>
                                                </ext:Column>

                                                <ext:TemplateColumn ID="TemplateColumn24" runat="server" DataIndex="REAC_NOMBRE" Flex="3" MinWidth="200" Text="<b><center>RESPONSABLE DE LA <br> ACTIVIDAD</center></b>">
                                                    <Template ID="Template24" runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                        <div class="columna_grid">{REAC_NOMBRE}</div>
                                                    </tpl>
                                                        </Html>
                                                    </Template>


                                                </ext:TemplateColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" Mode="Multi"></ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:FilterHeader ID="FilterHeader3" runat="server" OnCreateFilterableField="OnCreateFilterableField" />
                                        </Plugins>
                                        <View>
                                            <ext:GridView ID="GridView2" runat="server" StripeRows="true"></ext:GridView>
                                        </View>
                                        <BottomBar>
                                            <ext:PagingToolbar ID="PagingToolbar2" runat="server">
                                                <Plugins>
                                                    <ext:ProgressBarPager ID="ProgressBarPager2" runat="server"></ext:ProgressBarPager>
                                                </Plugins>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                    <ext:Panel
                        runat="server"
                        ID="panel1"
                        Title="Avance del PDI"
                        Layout="FormLayout" AutoScroll="true" UI="Primary" Icon="ChartBar" Hidden="false" Region="Center">
                        <Items>
                             <ext:FieldSet runat="server" MarginSpec="20 10 30 10" Collapsible="True" Title="<B>INFORME DEL AVANCE DEL PDI</B>" Flex="1" Layout="HBoxLayout">

                             </ext:FieldSet>
                        </Items>
                    </ext:Panel>
                        
                </Items>
            </ext:TabPanel>

            <ext:Window runat="server" Maximizable="true" ID="WinVerAdjunto" Hidden="true" Icon="Zoom" Width="850" Height="500" Modal="true" Title="Soporte de la Meta" AutoScroll="true" Layout="card" ActiveItem="0">
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

            <ext:Window runat="server" ID="WSoporte" Width="600" Height="250" AutoScroll="True" Title="Adicionar Archivo" Hidden="true" Resizable="false" 
            Layout="FitLayout" Padding="10" Modal="true" Icon="DiskMagnify">
                <Items>
                    <ext:FormPanel runat="server" ID="FPSoporteMetaProducto">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" Pack="Start" />
                        </LayoutConfig>
                        <Items>
                            <ext:Label id="LMensajeArchivos" Html="Por favor adjuntar únicamente archivos relacionados con la meta, en caso de videos escribir en enlace en el campo de <strong>Resultados de la Meta</strong>." Flex="1" runat="server" />
                            <ext:TextField runat="server" Name="MEPR_ID" FieldLabel="MEPR_ID" Hidden="true" Text="0" AllowBlank="false" />
                            <ext:TextField runat="server" Name="MEPR_TIPO" FieldLabel="MEPR_TIPO" Hidden="true" Text="0" AllowBlank="false" />
                            <ext:TextField runat="server" ID="TFDescripcionSoporteMeta" Name="SOMP_DESCRIPCION" FieldLabel="Descripción" LabelAlign="Top" AllowBlank="false" />
                            <ext:FileUploadField runat="server" ID="TArchivoSoporteMeta" FieldLabel="<b>Archivo máx. 5 Mb (.rar, .zip, .pdf, .doc, .docx, .png, .jpg) *</b>" LabelAlign="Top" AllowBlank="false" Flex="1">
                                <Listeners>
                                    <Change Fn="changeArchivo" />
                                </Listeners>
                            </ext:FileUploadField>
                            <ext:Button runat="server" Hidden="false" ID="BArchivo" Text="Agregar archivo" UI="Default" Icon="Add">
                                <DirectEvents>
                                    <Click OnEvent="AgregarArchivo_Click" >
                                        <EventMask ShowMask="true" Msg="Guardando archivo" />
                                        <ExtraParams>
                                            <%--<ext:Parameter Name="data" Value="App.FPSoporteMetaProducto.getValues()" Mode="Raw" />--%>
                                            <ext:Parameter Name="data" Value="1" Mode="Value" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
        </Items>
    </ext:Viewport>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
