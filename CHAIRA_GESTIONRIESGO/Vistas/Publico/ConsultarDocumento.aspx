<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultarDocumento.aspx.cs" Inherits="CHAIRA_GESTIONRIESGO.Vistas.Publico.ConsultarDocumento" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<%@ Import Namespace="System.Collections.Generic" %>

 

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Documentos</title>
    <link href="/resources/css/examples.css" rel="stylesheet" />
    <link href="../../Estilos/Css/EstiloPrincipal.css" rel="stylesheet" />
    <style>
        a{
        text-decoration:none;
        color:black;  
        }
        .panel{
            background-color: lightblue;
        }
    </style>
    <script>
        var loadPage2 = function (tabPanel, record) {
            
            //console.log(tabPanel.getComponent("node" + record.id))
           
        }
        var loadPage = function (tabPanel, record) {
            var tab = tabPanel.getComponent(record.id);
             
           //console.log(tab)
            if (!tab && record.data.href!="") {
                tab = tabPanel.add({
                    id: record.id,
                    title: record.data.text,
                    closable: true,

                    loader: {
                        url: record.data.href,
                        renderer: "frame",
                        loadMask: {
                            showMask: true,
                            msg: "Cargando " + record.data.href + "..."
                        }
                    },
                    autoScroll: true
                });
            }
             
            tabPanel.setActiveTab(tab);
        };

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
             
                <ext:TreePanel
                    ID="TreePanel1"
                    runat="server"
                    Region="West"
                    Width="420"
                    Title="Directorios"
                    Icon="ChartOrganisation"
                    Collapsible="true"
                    Split="true"
                    >
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
                         <ext:Button ID="Button1" runat="server"    Icon="ArrowDown" >
                            <Listeners>
                                <Click Handler="#{TreePanel1}.expandAll();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="Button2" runat="server"   Icon="ArrowUp" X="5">
                            <Listeners>
                                <Click Handler="#{TreePanel1}.collapseAll();" />
                            </Listeners>
                        </ext:Button>
                        <%--<ext:Checkbox ID="FilterLogic" runat="server" BoxLabel="TreePanel filtering" Checked="true">
                            <ToolTips>
                                <ext:ToolTip runat="server" Html="If checked then tree logic filtering (instead store logic)" />
                            </ToolTips>
                        </ext:Checkbox>--%>
                    </Items>
                </ext:Toolbar>
            </TopBar>
                    <Listeners>
                        <ItemClick Handler="if (record.data) {  e.stopEvent(); loadPage(#{Pages}, record); return false;}" />
                    </Listeners>
          <%--  <Fields>
                <ext:ModelField Name="task" />
                <ext:ModelField Name="user" />
            </Fields>--%>
                 <%--   <ColumnModel>
                <Columns>
                    <ext:TreeColumn
                        runat="server"
                        Text="Nombre"
                        Flex="2"
                        Sortable="true"
                        DataIndex="Nombre" />
                       <ext:Column
                        runat="server"
                        Text="Tamaño"
                        Flex="1"
                        Sortable="true"
                        DataIndex="Tamaño" />
                     <ext:Column
                        runat="server"
                        Text="Tipo"
                        Flex="1"
                        Sortable="true"
                        DataIndex="Tipo" />
                   </Columns>
                    </ColumnModel>--%>
                   
                </ext:TreePanel>
                     
                        
                <ext:TabPanel
                    ID="Pages"
                    runat="server"
                    Region="Center"
                  >
                </ext:TabPanel>
            
               
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>