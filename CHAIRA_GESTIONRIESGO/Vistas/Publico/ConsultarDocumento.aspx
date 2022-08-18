<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultarDocumento.aspx.cs" Inherits="CHAIRA_GESTIONRIESGO.Vistas.Publico.ConsultarDocumento" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<%@ Import Namespace="System.Collections.Generic" %>



<!DOCTYPE html>

<html>
<head runat="server">
    <title>Documentos</title>
    <link href="/resources/css/examples.css" rel="stylesheet" />

    <style>
        a {
            text-decoration: none;
            color: black;
        }

        .panel {
            background-color: lightblue;
        }
    </style>
    <script>
        var loadPage2 = function (tabPanel, record) {

            //console.log(tabPanel.getComponent("node" + record.id))

        }
        //var loadPage = function (tabPanel, record) {
        //    var tab = tabPanel.getComponent(record.id);

        //    //console.log(tab)
        //    if (!tab && record.data.href != "") {
        //        tab = tabPanel.add({
        //            id: record.id,
        //            title: record.data.text,
        //            closable: true,

        //            loader: {
        //                url: record.data.href,
        //                renderer: "frame",
        //                loadMask: {
        //                    showMask: true,
        //                    msg: "Cargando " + record.data.href + "..."
        //                }
        //            },
        //            autoScroll: true
        //        });
        //    }

        //    tabPanel.setActiveTab(tab);
        //};

        //Filtro mapa
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
    </script>
   
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" />

        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <%--  <ext:Panel ID="Panel1" runat="server"    Frame="false" Region="North"   >
                    <Items>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                        

                                </Items>
                            </ext:Toolbar>
                    </Items>
                </ext:Panel>--%>
                  <ext:TextField ID="TextField1" runat="server" Text="Node" Width="200" />

                <ext:TreePanel
                    ID="TreePanel1"
                    runat="server"
                    Region="West"
                    Width="420"
                    Title="Directorios"
                    Icon="ChartOrganisation"
                    Collapsible="true"
                    Split="true">
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
                                <ext:Button ID="Button1" runat="server" Icon="ArrowDown">
                                    <Listeners>
                                        <Click Handler="#{TreePanel1}.expandAll();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="Button2" runat="server" Icon="ArrowUp" X="5">
                                    <Listeners>
                                        <Click Handler="#{TreePanel1}.collapseAll();" />
                                    </Listeners>
                                </ext:Button>

                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Listeners>
                        <%--<ItemClick Handler="if (record.data) {  e.stopEvent(); loadPage(#{Pages}, record); return false;}" />--%>
                        <ItemClick Handler="e.stopEvent();App.direct.verPDF(record.data.href)" />
                    </Listeners>

                    <Store>
                        <ext:TreeStore runat="server" OnReadData="NodeLoad">
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Parameters>
                                <ext:StoreParameter Name="prefix" Value="#{TextField1}.getValue()" Mode="Raw" />
                            </Parameters>
                        </ext:TreeStore>
                    </Store>
                    <Root>
                        <ext:Node NodeID="1" Text="Directorio"  Icon="Folder"/>
                    </Root>
                    <ViewConfig LoadMask="false" />
                </ext:TreePanel>


                <%--<ext:TabPanel
                    ID="Pages"
                    runat="server"
                    Region="Center">
                    <Items>
                        <ext:Window runat="server" >

                        </ext:Window>
                    </Items>
                </ext:TabPanel>--%>
                <ext:FormPanel runat="server" Region="Center"  ID="WinVerAdjunto"     Modal="true" Title="Visualización" AutoScroll="true" Layout="card" MarginSpec="0 0 0 0" >
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

            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
