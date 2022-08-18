<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionarRolPermiso.aspx.cs" Inherits="CHAIRA_GESTIONRIESGO.Vistas.Privado.GestionarRolPermiso" %>
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
             
                App.direct.CambiarRol(record.get('id'));
           

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
                            FolderSort="true">
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                          <ext:Button runat="server" Text="Nuevo rol" Icon="DatabaseSave" MarginSpec="10 10 0 10" OnDirectClick="AgregarRol"></ext:Button>
                                        <ext:Button runat="server" Text="Editar rol" Icon="ApplicationEdit" MarginSpec="10 10 0 10" OnDirectClick="EditarRol"></ext:Button>
                                        <ext:ComboBox
                                            ID="c_roles"
                                            runat="server"
                                            DisplayField="Text"
                                            ValueField="Value"
                                            QueryMode="Local"
                                            EmptyText="Rol:"
                                            Flex="1"
                                            MaxWidth="200"
                                            AllowBlank="false"
                                            MarginSpec="10 10 0 10">
                                            <Store>
                                                <ext:Store ID="s_roles" runat="server">
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
                                            <DirectEvents>
                                                <Select OnEvent="actualiazarMenu">

                                                </Select>
                                            </DirectEvents>
                                        </ext:ComboBox>
                                        <ext:Button runat="server" Text="Guardar" Icon="DatabaseSave"></ext:Button>
                                        <ext:Button runat="server" Text="Cancelar" Icon="Cancel"></ext:Button>
                                       
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Fields>
                              <%--  <ext:ModelField Name="Nombre" />
                                <ext:ModelField Name="Id" />
                                <ext:ModelField Name="Estado" />--%>

                            </Fields>
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
                                    <ext:CheckColumn runat="server"
                                        Text="Estado"
                                        Flex="2"
                                        Sortable="true"
                                        DataIndex="estado"
                                        StopSelection="false"
                                        Editable="false" >
                                        <Listeners>
                                            
                                            <CheckChange  Handler="metodo">
                                     
                                            </CheckChange>
                                        </Listeners>
                                       <%-- <Listeners>
                                            
                                            <CheckChange  Handler="Ext.Msg.alert('Editando',record.get('id'));">
                                     
                                            </CheckChange>
                                        </Listeners>
                                         <DirectEvents>
                                                 <CheckChange onevent="BEnviarMensaje_Click">
                                                                <extraparams>
                                                                    <%--<ext:Parameter Name="MONID" Value="record.data.SOMP_MONGOID" Mode="Raw" />
                                                                    <ext:Parameter Name="MONID" Value="" Mode="Value" />
                                                                </extraparams> 
                                                 </CheckChange>
                                         </DirectEvents>
                                        --%>
                                        </ext:CheckColumn>

                                     <ext:ActionColumn runat="server"
                                        Text="Cambiar estado"
                                        Width="50"
                                        MenuDisabled="true"
                                        Align="Center"
                                        Weight="100"
                                        Flex="2">
                                        <Items>
                                            <ext:ActionItem Tooltip="Cambiar estado" Icon="PageWhiteEdit" Handler="handler">
                                            </ext:ActionItem>
                                        </Items>
                                    </ext:ActionColumn>

                                </Columns>

                            </ColumnModel>
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

        <ext:Window ID="w_crearRol" runat="server" Collapsible="true" Height="185" Icon="Application" Title="Crear rol" Width="350" Modal="true" Hidden="true">
            <Items>
                <ext:Panel ID="Panel1" runat="server" Height="100"   Layout="CenterLayout" Frame="false">
                    <Items>
                                 <ext:FieldContainer
                            runat="server"
                            LabelStyle="font-weight:bold;padding:0;"
                            Layout="HBoxLayout">
                            <FieldDefaults LabelAlign="Top" />
                            <Items>
                                <ext:TextField
                                    ID="t_nombreRol"
                                    runat="server"
                                    Name="Nombre"
                                    Flex="1"
                                    FieldLabel="Nombre"
                                    AllowBlank="false"
                                    MarginSpec="0 0 0 10"/>
                                
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:Panel>
        
                  <ext:Panel ID="Panel7" runat="server" Frame="false" Border="false" Layout="CenterLayout">
                            <Items>
                                <ext:Panel ID="Panel10" runat="server" Border="false">
                                    <Items>
                                        <ext:Button ID="b_guardarRol" runat="server" Text="Guardar" MarginSpec="10 0 10 0" Icon="DatabaseSave" OnDirectClick="GuardarRol">
                                        </ext:Button>
                                        <ext:Button ID="b_cancelarRol" runat="server" Text="Cancelar" MarginSpec="10 0 10 5" Icon="Cancel" OnDirectClick="CancelarRol">
                                        </ext:Button>

                                    </Items>
                                </ext:Panel>


                            </Items>
                        </ext:Panel>
            </Items>
        </ext:Window>
        <ext:Window ID="w_EditarRol" runat="server" Collapsible="true" Height="185" Icon="Application" Title="Editar rol" Width="350" Modal="true" Hidden="true">
            <Items>
                 <ext:Panel ID="Panel2" runat="server" Frame="false" Border="false" Layout="CenterLayout">
                            <Items>
                                <ext:Panel ID="Panel5" runat="server" Border="false">
                                    <Items>
                                        

                                        <ext:TextField ID="t_editarNombreRol" runat="server" FieldLabel="Nombre" LabelAlign="Top" >
                                        </ext:TextField>
                                         <ext:ComboBox
                                            ID="c_estadoRoles"
                                            runat="server"
                                            DisplayField="Text"
                                            ValueField="Value"
                                            QueryMode="Local"
                                            EmptyText="Estado rol:"
                                            Flex="1"
                                            MaxWidth="200"
                                            AllowBlank="false"
                                            MarginSpec="10 0 0 0">
                                            <Store>
                                                <ext:Store ID="s_rolesEditar" runat="server">
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
                                </ext:Panel>


                            </Items>
                        </ext:Panel>
        
                  <ext:Panel ID="Panel3" runat="server" Frame="false" Border="false" Layout="CenterLayout">
                            <Items>
                                <ext:Panel ID="Panel4" runat="server" Border="false">
                                    <Items>
                                        <ext:Button ID="b_guardarModificacionRol" runat="server" Text="Guardar" MarginSpec="10 0 10 0" Icon="DatabaseSave" OnDirectClick="GuardarModificacionRol">
                                        </ext:Button>
                                        <ext:Button ID="b_cancelarModificacionRol" runat="server" Text="Cancelar" MarginSpec="10 0 10 5" Icon="Cancel" OnDirectClick="CancelarModificacionRol">
                                        </ext:Button>

                                    </Items>
                                </ext:Panel>


                            </Items>
                        </ext:Panel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>
