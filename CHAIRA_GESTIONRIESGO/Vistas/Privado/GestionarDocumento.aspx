<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionarDocumento.aspx.cs" Inherits="CHAIRA_GESTIONRIESGO.Vistas.Privado.GestionarDocumento" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<%@ Import Namespace="System.Collections.Generic" %>



<!DOCTYPE html>

<html>
<head runat="server">
    <title>Documentos</title>

    <link href="/resources/css/examples.css" rel="stylesheet" />

    <script>


        var handler = function (grid, rowIndex, colIndex, actionItem, event, record, row) {
           // Ext.Msg.alert('Editando' + (record.get('done') ? ' completed task' : ''), record.get('id'));
            if (record.get('tipo') == "Directorio") {

            App.direct.ActivarVentana(record.get('id'), "1");
            } else {
                App.direct.ActivarVentana(record.get('id'), "2");
            }
            
        };
        //Arrastrar
        
        //var moveNode = function (item, oldParent, newParent, index, options) {
        //    var buf=[];
        //    buf.push("Node = " + item.data.text);
        //    buf.push("<br />");
        //    buf.push("Old parent = " + oldParent.data.);
        //    buf.push("<br />");
        //    buf.push("New parent = " + newParent.data.text);
        //    buf.push("<br />");
        //    buf.push("Index = " + index);

        //    Ext.Msg.alert("Node droped", buf.join(""));
        //};
        var filterTree = function (tf, e) {
            var tree = App.TreePanel1,
                store = tree.store,
                logic = tree,
                /*logic = App.FilterLogic.getValue() ? tree : store,*/
                text = tf.getRawValue();

            logic.clearFilter();

            if (Ext.isEmpty(text, false)) {
                return;
            }

            if (e.getKey() === Ext.EventObject.ESC) {
                clearFilter();
            } else {
                var re = new RegExp(".*" + text + ".*", "i");

                logic.filterBy(function (node) {
                    return re.test(node.data.text);
                });
            }
        };

        var clearFilter = function () {
            var field = App.TriggerField1,
                tree = App.TreePanel1,
                store = tree.store,
                logic = tree;
            //logic = App.FilterLogic.getValue() ? tree : store;

            field.setValue("");
            logic.clearFilter(true);
            tree.getView().focus();
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
    <form runat="server">
        <ext:ResourceManager runat="server" />
        <ext:Viewport runat="server" Layout="FitLayout" AutoScroll="true" Hidden="false">
            <Items>
                <ext:Panel
                    ID="Window1"
                    runat="server"
                    Closable="false"
                    Title="Gestión de los directorios"
                    AutoScroll="true"
                    Layout="Fit"
                    Collapsible="true">
                    <TopBar>

                        <ext:Toolbar runat="server">

                            <Items>

                                <ext:ToolbarTextItem runat="server" Text="Buscar:" />
                                <ext:ToolbarSpacer />
                                <ext:TextField
                                    ID="TriggerField1"
                                    runat="server"
                                    EnableKeyEvents="true">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" />
                                    </Triggers>
                                    <Listeners>
                                        <KeyUp Fn="filterTree" Buffer="250" />
                                        <TriggerClick Handler="clearFilter();" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:ToolbarSpacer />
                                <ext:Button ID="Button5" runat="server" Icon="ArrowDown">
                                    <Listeners>
                                        <Click Handler="#{TreePanel1}.expandAll();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="Button6" runat="server" Icon="ArrowUp" X="5">
                                    <Listeners>
                                        <Click Handler="#{TreePanel1}.collapseAll();" />
                                    </Listeners>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:TreePanel
                            runat="server"
                            Frame="false"
                            Width="500"
                            Height="300"
                            UseArrows="true"
                            RootVisible="false"
                            MultiSelect="true"
                            SingleExpand="true"
                            ID="TreePanel1"
                            FolderSort="true"
                             >
                           <%-- <Fields>
                                <ext:ModelField Name="Nombre" />
                                <ext:ModelField Name="Id" />
                                <ext:ModelField Name="Tamaño" />
                                <ext:ModelField Name="Tipo" />
                                <ext:ModelField Name="Operacion" />
                            </Fields>--%>
                            <ColumnModel>
                                <Columns>
                                    <ext:TreeColumn
                                        runat="server"
                                        Text="Nombre"
                                        Flex="10"
                                        Sortable="true"
                                        DataIndex="nombre"
                                        AutoScroll="true" />
                                    <ext:Column
                                        runat="server"
                                        Text="Id"
                                        Flex="2"
                                        Sortable="true"
                                        DataIndex="id"
                                        Hidden="true" />
                                    <%--<ext:Column
                                        runat="server"
                                        Text="Tamaño"
                                        Flex="2"
                                        Sortable="true"
                                        DataIndex="tamaño" />--%>
                                    <ext:Column
                                        runat="server"
                                        Text="Tipo"
                                        Flex="2"
                                        Sortable="true"
                                        DataIndex="tipo" />

                                    <ext:ActionColumn runat="server"
                                        Text="Editar"
                                        Width="50"
                                        MenuDisabled="true"
                                        Align="Center"
                                        Weight="100"
                                        Flex="2">
                                        <Items>
                                            <ext:ActionItem Tooltip="Editar" Icon="PageWhiteEdit" Handler="handler">
                                            </ext:ActionItem>
                                        </Items>
                                    </ext:ActionColumn>
                                </Columns>

                            </ColumnModel>
                             <%--<Listeners>
                            <ItemMove Fn="moveNode" Delay="1" />
                        </Listeners>
                        <View>
                            <ext:TreeView runat="server">
                               <Plugins>
                                   <ext:TreeViewDragDrop runat="server" ContainerScroll="true" />
                               </Plugins>
                            </ext:TreeView>
            </View>--%>
                    


                            <Store>
                        <ext:TreeStore runat="server" OnReadData="NodeLoad" ID="tienda">
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Parameters>
                                <%--<ext:StoreParameter Name="prefix" Value="#{TextField1}.getValue()" Mode="Raw" />--%>
                            </Parameters>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="1" Text="Directorio"  Icon="Folder"/>
                    </Root>
                    <ViewConfig LoadMask="false" />
                        </ext:TreePanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>

        <ext:Window ID="w_ventana" runat="server" Collapsible="true" Height="250"   Icon="Application" Title="Gestionar carpeta" Width="550" Modal="true" Hidden="true">

            <Items>
                <ext:FormPanel
                    runat="server"
                    Frame="false"
                    Height="300"
                    Width="500"
                    Border="false"
                    BodyPadding="10"
                    DefaultAnchor="100%">
                    <FieldDefaults
                        LabelAlign="Top"
                        LabelWidth="100"
                        LabelStyle="font-weight:bold;" />
                    <Defaults>
                        <ext:Parameter Name="margin" Value="0 0 10 0" Mode="Value" />
                    </Defaults>
                    <Items>
                        <ext:FieldContainer
                            runat="server"
                            LabelStyle="font-weight:bold;padding:0;"
                            Layout="HBoxLayout">
                            <FieldDefaults LabelAlign="Top" />
                            <Items>
                                <ext:TextField
                                    runat="server"
                                    Name="Nombre"
                                    Flex="1"
                                    FieldLabel="Nombre"
                                    AllowBlank="false"
                                    ID="t_nombreCarpetaw"
                                    />
                                    
                                    <ext:TextField
                                    runat="server"
                                    Name="Nombre2"
                                    Flex="1"
                                    FieldLabel="Agregar carpeta hija"
                                    AllowBlank="false"
                                        EmptyText="Nombre:"
                                       MarginSpec="0 0 0 10"
                                        ID="t_NombreCarpeta"
                                        />
                                <ext:Button runat="server" Text="Agregar " MarginSpec="25 0 0 10" OnDirectClick="AgregarCarpeta_Click"></ext:Button>

                            </Items>
                        </ext:FieldContainer>
                         
                        <ext:FieldContainer
                            runat="server"
                            LabelStyle="font-weight:bold;padding:0;"
                            Layout="HBoxLayout">
                            <FieldDefaults LabelAlign="Top" />
                            <Items>
                                <%-- Guardar Archivo --%>
                                <ext:FileUploadField runat="server" ID="TArchivoSoporteMeta"  Flex="2">
                                    <Listeners>
                                        <Change Fn="changeArchivo" />
                                    </Listeners>
                                </ext:FileUploadField>
                                <ext:Button runat="server" Hidden="false" ID="BArchivo" Text="Agregar archivo" UI="Default" Icon="Add" >
                                    <DirectEvents>
                                        <Click OnEvent="AgregarArchivo_Click">
                                            <EventMask ShowMask="true" Msg="Guardando archivo" />
                                            <ExtraParams>
                                                <%--<ext:Parameter Name="data" Value="App.FPSoporteMetaProducto.getValues()" Mode="Raw" />--%>
                                                <ext:Parameter Name="padre" Value="1" Mode="Value" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                                <%-- Fin guardar archivo --%>
                                <%-- Tomar ruta --%>
                                <%-- <ext:Button runat="server" Text="Get File Path">
                                    <Listeners>
                                        <Click Handler="var v = #{BasicField}.getValue(); Ext.Msg.alert('Selected&nbsp;File', v && v != '' ? v : 'None');" />
                                    </Listeners>
                                </ext:Button>--%>
                                 <ext:ComboBox
                                            ID="c_Estado"
                                            runat="server"
                                            DisplayField="Text"
                                            ValueField="Value"
                                            QueryMode="Local"
                                            EmptyText="Estado:"
                                             Flex="1"
                                            AllowBlank="false"
                                            MarginSpec="0 0 0 10"
                                             >
                                            <Store>
                                                <ext:Store ID="s_comboEstado" runat="server">
                                                    <Model>
                                                        <ext:Model runat="server" IDProperty="Value">
                                                            <Fields>
                                                                <ext:ModelField Name="Text" />
                                                                <ext:ModelField Name="Value" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                             
                                        </ext:ComboBox>

                            </Items>
                        </ext:FieldContainer>
                        <ext:Panel ID="Panel7" runat="server" Frame="false" Border="false" Layout="CenterLayout">
                            <Items>
                                <ext:Panel ID="Panel10" runat="server" Border="false">
                                    <Items>
                                        <ext:Button ID="b_guardar" runat="server" Text="Guardar" MarginSpec="10 0 10 0" Icon="DatabaseSave" OnDirectClick="w_GuardarCarpeta">
                                        </ext:Button>
                                        <ext:Button ID="Button16" runat="server" Text="Cancelar" MarginSpec="10 0 10 5" Icon="Cancel" Handler="App.w_ventana.hide();">
                                        </ext:Button>

                                    </Items>
                                </ext:Panel>


                            </Items>
                        </ext:Panel>

                    </Items>

                </ext:FormPanel>
            </Items>
        </ext:Window>


        <ext:Window ID="w_gestionarArchivo" runat="server" Collapsible="true" Height="650"   Icon="Application" Title="Gestionar archivo" Width="620" Modal="true" AutoScroll="true" Hidden="true">

            <Items>
                <ext:FormPanel
                    runat="server"
                    Frame="false"
                    Height="600"
                    Width="600"
                    Border="false"
                    BodyPadding="10"
                    DefaultAnchor="100%"
                    
                    >
                    <FieldDefaults
                        LabelAlign="Top"
                        LabelWidth="100"
                        LabelStyle="font-weight:bold;" />
                    <Defaults>
                        <ext:Parameter Name="margin" Value="0 0 10 0" Mode="Value" />
                    </Defaults>
                    <Items>
                        <ext:FieldContainer
                            runat="server"
                            LabelStyle="font-weight:bold;padding:0;"
                            Layout="HBoxLayout">
                            <FieldDefaults LabelAlign="Top" />
                            <Items>
                                <ext:TextField
                                    runat="server"
                                    ID="t_nombre_Archivo"
                                    Flex="1"
                                    FieldLabel="Nombre"
                                    AllowBlank="false" />
                                <ext:Button runat="server" Text="Cambiar nombre" MarginSpec="25 0 0 10"></ext:Button>

                            </Items>
                        </ext:FieldContainer>

                         <ext:Panel ID="Panel4" runat="server" Frame="false" Border="false" Layout="CenterLayout">

                            <Items>
                                <ext:Panel ID="Panel5" runat="server" Border="false" Layout="ColumnLayout">
                                    <Items>
                                          
                                        <ext:Button ID="Button3" runat="server" Text="Eliminar archivo" MarginSpec="10 0 10 0" Icon="Cancel">
                                        </ext:Button>
                                        <ext:Button ID="Button4" runat="server" Text="Reemplazar archivo" MarginSpec="10 0 10 5" Icon="Database">
                                        </ext:Button>
                                       
                                    </Items>
                                </ext:Panel>

                            </Items>
                        </ext:Panel>

                        <%--<ext:Panel ID="Panel3" runat="server" Height="400" Border="true" >
                            <Items></Items>
                        </ext:Panel>--%>
                        <ext:FormPanel runat="server"   ID="WinVerAdjunto" Hidden="false" Icon="Zoom"  Height="400" Weight="490" Modal="false"   AutoScroll="true" Layout="card" >
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
                      
                    </ext:FormPanel>
                        <ext:Panel ID="Panel1" runat="server" Frame="false" Border="false" Layout="CenterLayout">
                            <Items>
                                <ext:Panel ID="Panel2" runat="server" Border="false">
                                    <Items>
                                        <ext:Button ID="Button1" runat="server" Text="Guardar" MarginSpec="10 0 10 0" Icon="DatabaseSave">
                                        </ext:Button>
                                        <ext:Button ID="Button2" runat="server" Text="Cancelar" MarginSpec="10 0 10 5" Icon="Cancel">
                                        </ext:Button>

                                    </Items>
                                </ext:Panel>


                            </Items>
                        </ext:Panel>
                    </Items>

                </ext:FormPanel>
            </Items>
        </ext:Window>

    </form>
</body>
</html>
