using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;

using System.Text;
using System.Collections.Generic;
using CHAIRA_GESTIONRIESGO.Conexion;

using CHAIRA_GESTIONRIESGO.Utilities;
using System.Data;
using CHAIRA_GESTIONRIESGO.Modelo;
using CHAIRA_GESTIONRIESGO.Controlador;

namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public class Util
    {
        /// <summary>
        /// Obtiene el PEGE_ID  del usuario actualmente logueado a partir de su sesión.
        /// </summary>
        /// <returns>retorna el PEGE_ID</returns>
        public string GetPege(HttpRequest bc = null)
        {

            return "62477";
            try
            {

                if (!(HttpContext.Current.Request.Url.Host.Equals("localhost") || HttpContext.Current.Request.Url.Host.Equals("127.0.0.1")))
                {
                    if (HttpContext.Current.Request.UrlReferrer == null) //|| HttpContext.Current.Request.UrlReferrer.Host.Equals("chaira.uniamazonia.edu.co")
                    {
                        FormsAuthentication.SignOut();
                        HttpContext.Current.Session.Abandon();
                        HttpContext.Current.Server.Transfer("~/Vistas/Publico/SessionExpired.aspx");
                    }
                }
                if (HttpContext.Current.Request["Token"] != null)
                {
                    HttpContext.Current.Session.Remove("pege_id");
                    HttpContext.Current.Session["pege_id"] = Uri.UnescapeDataString(HttpContext.Current.Request["Token"]);
                }

                if (HttpContext.Current.Session["pege_id"] == null)
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Server.Transfer("~/Vistas/Publico/SessionExpired.aspx");
                }

                var pegeDecrypt = string.Empty;
                var pegeEncrypt = ((string)HttpContext.Current.Session["pege_id"]);
                if (!string.IsNullOrEmpty(pegeEncrypt))
                {
                    var denc = new Crypto();
                    pegeDecrypt = denc._Decrypt(pegeEncrypt);
                    //pegeDecrypt = pegeEncrypt;
                    //if (bc != null)
                    //    RegistroActividad(pegeDecrypt, bc);
                    //else
                    //    RegistroActividad(pegeDecrypt);
                }
                else
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Server.Transfer("~/Vistas/Publico/SessionExpired.aspx");
                }
                return pegeDecrypt;

            }
            catch (Exception ex)
            {
                NotificacionCOD.notificar("errorPin", "Error no controlado", "Error al realizar acciones en autenticación: " + ex.Message);
                return null;
            }
        }

        public string consultarEstadoBD()
        {

            DataTable estadoBD = CUtil.getEstadoBD();


            return estadoBD.Rows[0][0].ToString();
        }

        public string consultarEstadoServidor()
        {

            DataTable estadoServidor = CUtil.getEstadoServidor();

            return estadoServidor.Rows[0][0].ToString();
        }

        private void RegistroActividad(string pegeId, HttpRequest request = null, string Lat = null, string Lon = null, string Dis = null)
        {
            string BDEstado = consultarEstadoBD();
            string ServidorEstado = consultarEstadoServidor();
            try
            {
                var path = HttpRuntime.AppDomainAppPath;
                path += @"Resources\Log";

                var dir = new DirectoryInfo(path);

                if (!dir.Exists)
                    dir.Create();

                var log = HttpRuntime.AppDomainAppPath;
                log = string.Format("{0}Resources\\Log\\{1}.txt", log, pegeId);

                var url = HttpContext.Current.Request.Url.ToString();

                if (!File.Exists(log))
                {
                    var writer = new StreamWriter(log);
                    writer.Close();
                }

                try
                {
                    //var dbo = new SROracle.DBOracle();

                    //if (dbo.OraSqlQuery("SELECT CONFIGURACIONGLOBAL.COGO_VALOR FROM CHAIRA.CONFIGURACIONGLOBAL WHERE CONFIGURACIONGLOBAL.COGO_IDENTIFICADOR='AUD_APP_SERVIDOR'").Tables[0].Rows[0][0].ToString() == "1")
                    if (ServidorEstado.Equals("1"))
                    {
                        var fs = new FileStream(log, FileMode.Append, FileAccess.Write);
                        var sw = new StreamWriter(fs);

                        if (request != null)
                        {
                            sw.WriteLine("[" + DateTime.Now + "] - " + url + " \r" +
                                        "IP = " + request.UserHostAddress + "; \r" +
                                        "Host name = " + request.UserHostName + "; \r" +

                                        //"Host name = " + System.Net.Dns.GetHostEntry(request.UserHostAddress).HostName + "; \r" +
                                        "Browser user agent = " + request.UserAgent + "; \r" +
                                        "Type = " + request.Browser.Type + "; \r" +

                                        //"Name = " + Request.Browser + "; \r" +
                                        "Version = " + request.Browser.Version + "; \r" +

                                        //"Major Version = " + Request.Browser.MajorVersion + "; \r" +
                                        //"Minor Version = " + Request.Browser.MinorVersion + "; \r" +
                                        "Platform = " + request.Browser.Platform + "; \r" +
                                        "Is Beta = " + request.Browser.Beta + "; \r" +

                                        //"Is Crawler = " + Request.Browser.Crawler + "; \r" +
                                        //"Is AOL = " + Request.Browser.AOL + "; \r" +
                                        //"Is Win16 = " + Request.Browser.Win16 + "; \r" +
                                        //"Is Win32 = " + Request.Browser.Win32 + "; \r" +
                                        "Supports Frames = " + request.Browser.Frames + "; \r" +
                                        "Supports Tables = " + request.Browser.Tables + "; \r" +
                                        "Supports Cookies = " + request.Cookies + "; \r" +

                                        //"Supports VB Script = " + request.Browser.VBScript + "; \r" +
                                        "Supports JavaScript = " + request.Browser.JavaScript + "; \r" +
                                        "EcmaScriptVersion = " + request.Browser.EcmaScriptVersion + "; \r" +

                                        //"Supports Java Applets = " + request.Browser.JavaApplets + "; \r" +
                                        //"Supports ActiveX Controls = " + request.Browser.ActiveXControls + "; \r" +

                                        //"CDF = " + Request.Browser.CDF + "; \r" +
                                        "Mobile Device = " + request.Browser.IsMobileDevice + "; \r" +
                                        "ScreenPixelsHeight = " + request.Browser.ScreenPixelsHeight + "; \r" +
                                        "ScreenPixelsWidth = " + request.Browser.ScreenPixelsWidth);
                        }
                        else
                            sw.WriteLine("[" + DateTime.Now + "] - " + url);

                        sw.Close();
                    }
                    //if (dbo.OraSqlQuery("SELECT CONFIGURACIONGLOBAL.COGO_VALOR FROM CHAIRA.CONFIGURACIONGLOBAL WHERE CONFIGURACIONGLOBAL.COGO_IDENTIFICADOR='AUD_APP_BD'").Tables[0].Rows[0][0].ToString() == "1")
                    if (BDEstado.Equals("1"))
                    {
                        //var sql = "INSERT INTO CHAIRA.AUD_APLICATIVO(AUAP_FECHA, PEGE_ID, AUAP_URL, AUAP_LATITUD, AUAP_LONGITUD, AUAP_DISTANCIA , AUAP_SESIONID ) VALUES (SYSDATE, '" + pegeId + "', '" + url + "','" + (Lat == null ? "" : Lat) + "','" + (Lon == null ? "" : Lon) + "','" + (Dis == null ? "" : Dis) + "','" + ((string)HttpContext.Current.Session["sSesionId"] == null ? "" : (string)HttpContext.Current.Session.SessionID) + "')";
                        //dbo.OraSqlQuerySingle(sql);
                        string excep = string.Empty;
                        List<Parametro> objAUDI = new List<Parametro>
                        {
                            new Parametro("VARPEGEID", pegeId, "NUMBER"),
                            new Parametro("VARURL", url, "VARCHAR2"),
                            new Parametro("VARLAT", (Lat == null ? "" : Lat), "VARCHAR2"),
                            new Parametro("VARLON", (Lon == null ? "" : Lon), "VARCHAR2"),
                            new Parametro("VARDIS", (Dis == null ? "" : Dis), "VARCHAR2"),
                            new Parametro("VARSESION", ((string)HttpContext.Current.Session["sSesionId"] == null ? "" : (string)HttpContext.Current.Session.SessionID), "VARCHAR2")
                        };

                        excep = CUtil.INSERTARAUDAPLICATIVO(objAUDI);

                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }




        /// <summary>
        /// Obtiene la url de la pagina desde la ruta del servidor para ser
        /// comparada en la BD pagina
        /// </summary>
        /// <param name="urls">URL fisica</param>
        /// <returns>URL virtual</returns>
        public string[] Urls(string[] urls)
        {
            var uris = new string[urls.Length];
            var path = HttpRuntime.AppDomainAppPath;

            for (var i = 0; i < urls.Length; i++)
            {
                uris[i] = "/Chaira/" + urls[i].Replace(path, "").Replace('\\', '/');
            }
            return uris;
        }




        /// <summary>
        /// Retorna los días habiles del mes
        /// </summary>
        /// <param name="YYYYMM">Año y mes a consultar en formato YYYYMM</param>
        /// <returns>DataTable</returns>



        #region Encriptación utilizando AES 256
        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        public string EncryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }
        #endregion

        #region Registrar Todos lo Iconos de Ext v1.7
        public void RegistrarIconos(Ext.Net.ResourceManager ResourceManager1)
        {
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Accept);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Add);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Anchor);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Application);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationCascade);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationDouble);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationForm);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationFormAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationFormDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationFormEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationFormMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationGet);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationHome);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsx);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxCascade);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxDouble);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxGet);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxHome);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxSplit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationOsxTerminal);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationPut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationSideBoxes);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationSideContract);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationSideExpand);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationSideList);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationSideTree);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationSplit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationTileHorizontal);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationTileVertical);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationViewColumns);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationViewDetail);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationViewGallery);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationViewIcons);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationViewList);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationViewTile);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationXp);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ApplicationXpTerminal);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowBranch);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowDivide);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowDown);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowEw);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowIn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowInLonger);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowInout);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowJoin);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowLeft);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowMerge);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowNe);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowNs);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowNsew);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowNw);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowNwNeSwSe);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowNwSe);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowOut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowOutLonger);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowRedo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowRefresh);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowRefreshSmall);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowRight);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowRotateAnticlockwise);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowRotateClockwise);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowSe);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowSw);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowSwitch);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowSwitchBluegreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowSwNe);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowTurnLeft);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowTurnRight);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowUndo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ArrowUp);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AsteriskOrange);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AsteriskRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AsteriskYellow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Attach);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarBronze1);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarBronze2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarBronze3);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarGold1);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarGold2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarGold3);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarSilver1);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarSilver2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.AwardStarSilver3);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Basket);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BasketAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BasketDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BasketEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BasketError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BasketGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BasketPut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BasketRemove);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Bell);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellSilver);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellSilverStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellSilverStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BellStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Bin);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BinClosed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BinEmpty);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Blank);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Bomb);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Book);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookAddresses);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookAddressesAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookAddressesDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookAddressesEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookAddressesError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookAddressesKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Bookmark);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookmarkAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookmarkDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookmarkEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookmarkError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookmarkGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookNext);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookOpen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookOpenMark);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookPrevious);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BookTabs);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderAll);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderBottom);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderDraw);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderInner);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderInnerHorizontal);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderInnerVertical);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderLeft);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderNone);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderOuter);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderRight);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BorderTop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Box);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BoxError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BoxPicture);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BoxWorld);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Brick);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BrickAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BrickDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BrickEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BrickError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BrickGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BrickLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BrickMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Bricks);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Briefcase);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Bug);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BugAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BugDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BugEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BugError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BugFix);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BugGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BugLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BugMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Build);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BuildCancel);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Building);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BuildingAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BuildingDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BuildingEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BuildingError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BuildingGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BuildingKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BuildingLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletArrowBottom);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletArrowDown);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletArrowTop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletArrowUp);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletBlack);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletCross);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletDatabase);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletDatabaseYellow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletDisk);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletEarth);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletEject);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletFeed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletGet);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletHome);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletLeft);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletMinus);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletOrange);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletPageWhite);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletPicture);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletPink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletPlus);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletPurple);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletRight);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletShape);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletSparkle);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletStopAlt);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletTick);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletToggleMinus);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletTogglePlus);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletWhite);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletWrench);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletWrenchRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.BulletYellow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Button);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cake);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CakeOut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CakeSliced);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Calculator);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalculatorAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalculatorDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalculatorEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalculatorError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalculatorLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Calendar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarSelectDay);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarSelectNone);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarSelectWeek);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarViewDay);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarViewMonth);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CalendarViewWeek);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Camera);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraPicture);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraSmall);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CameraStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cancel);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Car);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CarAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CarDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CarError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CarRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CarStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CarStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CartAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CartDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CartEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CartError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CartFull);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CartGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CartMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CartPut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CartRemove);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdBurn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdEject);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdPlay);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cdr);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrBurn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrCross);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrEject);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrPlay);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrStopAlt);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdrTick);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdStopAlt);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CdTick);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartBar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartBarAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartBarDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartBarEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartBarError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartBarLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartCurve);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartCurveAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartCurveDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartCurveEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartCurveError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartCurveGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartCurveLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartLine);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartLineAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartLineDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartLineEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartLineError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartLineLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartOrganisation);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartOrganisationAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartOrganisationDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartOrgInverted);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartPie);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartPieAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartPieDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartPieEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartPieError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartPieLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ChartPieLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CheckError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Clipboard);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Clock);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockPause);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockPlay);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ClockStop2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cmy);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cog);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CogAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CogDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CogEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CogError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CogGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CogStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CogStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Coins);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CoinsAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CoinsDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Color);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ColorSwatch);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ColorWheel);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Comment);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CommentAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CommentDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CommentDull);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CommentEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CommentPlay);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CommentRecord);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Comments);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CommentsAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CommentsDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Compass);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Compress);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Computer);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerOff);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ComputerWrench);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Connect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Contrast);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ContrastDecrease);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ContrastHigh);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ContrastIncrease);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ContrastLow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlAddBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlBlank);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlBlankBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlEject);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlEjectBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlEnd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlEndBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlEqualizer);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlEqualizerBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlFastforward);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlFastforwardBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Controller);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControllerAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControllerDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControllerError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlPause);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlPauseBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlPlay);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlPlayBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlPower);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlPowerBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlRecord);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlRecordBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlRemove);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlRemoveBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlRepeat);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlRepeatBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlRewind);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlRewindBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlStartBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ControlStopBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Creditcards);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cross);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Css);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CssAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CssDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CssError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CssGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CssValid);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cup);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupBlack);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CupTea);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cursor);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CursorSmall);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Cut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.CutRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Database);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseCopy);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseGear);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseRefresh);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseSave);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseTable);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseWrench);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseYellow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseYellowStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatabaseYellowStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Date);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DateAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DateDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DateEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DateError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DateGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DateLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DateMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DateNext);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DatePrevious);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Decline);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Delete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DeviceStylus);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Disconnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Disk);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DiskBlack);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DiskBlackError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DiskBlackMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DiskDownload);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DiskEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DiskError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DiskMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DiskMultiple);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DiskUpload);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Door);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DoorError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DoorIn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DoorOpen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DoorOut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Drink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DrinkEmpty);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DrinkRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Drive);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveBurn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveCd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveCdEmpty);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveCdr);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveDisk);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveNetwork);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveNetworkError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveNetworkStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveRename);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveUser);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DriveWeb);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Dvd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DvdAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DvdDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DvdEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DvdError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DvdGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DvdKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DvdLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DvdStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.DvdStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EjectBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EjectGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Email);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailAttach);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailOpen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailOpenImage);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmailTransfer);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmoticonEvilgrin);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmoticonGrin);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmoticonHappy);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmoticonSmile);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmoticonSurprised);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmoticonTongue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmoticonUnhappy);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmoticonWaii);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.EmoticonWink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Erase);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Error);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ErrorAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ErrorDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ErrorGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Exclamation);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Eye);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Eyes);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Feed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedDisk);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FeedStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Female);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Film);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmEject);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmSave);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FilmStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Find);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FingerPoint);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FlowerDaisy);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Folder);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderBell);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderBookmark);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderBrick);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderBug);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderCamera);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderDatabase);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderExplore);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderFeed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderFilm);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderFind);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderFont);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderHeart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderHome);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderImage);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderLightbulb);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderPage);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderPageWhite);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderPalette);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderPicture);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderTable);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderUp);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderUser);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FolderWrench);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Font);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FontAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FontColor);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FontDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FontGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FontLarger);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.FontSmaller);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ForwardBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ForwardGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Group);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.GroupAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.GroupDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.GroupEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.GroupError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.GroupGear);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.GroupGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.GroupKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.GroupLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Heart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HeartAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HeartBroken);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HeartConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HeartDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Help);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Hourglass);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HourglassAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HourglassDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HourglassGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HourglassLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.House);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HouseConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HouseGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HouseKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HouseLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HouseStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Html);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HtmlAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HtmlDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HtmlError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HtmlGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.HtmlValid);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Image);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ImageAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ImageDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ImageEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ImageLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ImageMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Images);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ImageStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Information);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Ipod);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.IpodCast);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.IpodCastAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.IpodCastDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.IpodConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.IpodNano);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.IpodNanoConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.IpodSound);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Joystick);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.JoystickAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.JoystickConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.JoystickDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.JoystickError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Key);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.KeyAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Keyboard);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.KeyboardAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.KeyboardConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.KeyboardDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.KeyboardMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.KeyDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.KeyGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.KeyStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.KeyStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Laptop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopDisk);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LaptopWrench);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Layers);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Layout);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutContent);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ZoomOut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ZoomIn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Zoom);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.XhtmlValid);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.XhtmlGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.XhtmlError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.XhtmlDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.XhtmlAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Xhtml);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WrenchOrange);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Wrench);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldOrbit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldNight);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldDawn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WorldAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.World);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WebcamStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WebcamStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WebcamError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WebcamDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WebcamConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WebcamAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Webcam);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WeatherSun);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WeatherSnow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WeatherRain);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WeatherLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WeatherCloudyRain);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WeatherCloudy);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WeatherClouds);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.WeatherCloud);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Wand);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.VectorKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.VectorDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.VectorAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Vector);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.VcardKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.VcardEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.VcardDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.VcardAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Vcard);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserTick);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserSuitBlack);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserSuit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserOrange);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserMature);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserHome);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserGrayCool);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserGray);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserFemale);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserEarth);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserCross);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserComment2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserComment);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserBrown);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserB);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserAlert);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.UserAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.User);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Tux);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TransmitRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TransmitGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TransmitError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TransmitEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TransmitDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TransmitBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TransmitAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Transmit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TimeRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TimelineMarker);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TimeGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TimeGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TimeDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TimeAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Time);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Tick);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ThumbUp);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ThumbDown);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Theme);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextUppercase);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextUnderline);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextTab);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextSuperscript);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextSubscript);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextStrikethrough);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextSpelling);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextSmallcaps);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextSignature);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextShading);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextRuler);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextRotate90);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextRotate270);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextRotate180);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextRotate0);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextRightToLeft);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextReplace);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextPaddingTop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextPaddingRight);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextPaddingLeft);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextPaddingBottom);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextMirror);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextLowercaseA);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextLowercase);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextListNumbers);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextListBullets);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextLinespacing);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextLetterspacing);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextLetterOmega);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextLeftToRight);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextKerning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextItalic);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextInverse);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextIndentRemove);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextIndent);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextHorizontalrule);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextHeading6);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextHeading5);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextHeading4);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextHeading3);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextHeading2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextHeading1);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextFontDefault);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextFlip);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextFit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextfieldRename);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextfieldKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextfieldDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextfieldAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Textfield);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextDropcaps);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextDoubleUnderline);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextDirection);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextComplete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextColumns);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextBold);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextAllcaps);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextAlignRight);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextAlignLeft);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextAlignJustify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextAlignCenter);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TextAb);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelevisionStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelevisionOut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelevisionOff);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelevisionIn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelevisionDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelevisionAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Television);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelephoneRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelephoneLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelephoneKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelephoneGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelephoneError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelephoneEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelephoneDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TelephoneAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Telephone);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagYellow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagsRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagsGrey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagPurple);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagPink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagOrange);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagBlueEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagBlueDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagBlueAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TagBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Tag);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TabRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableSort);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableSave);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableRowInsert);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableRowDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableRow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableRelationship);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableRefresh);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableMultiple);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableGear);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableColumnDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableColumnAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableColumn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableCell);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TableAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Table);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TabGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TabGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TabEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TabDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TabBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.TabAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Tab);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Sum);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StyleGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StyleEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StyleDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StyleAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Style);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StopRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StopGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StopBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Stop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StatusOnline);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StatusOffline);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StatusInvisible);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StatusBusy);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StatusBeRightBack);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StatusAway);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StarSilver);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StarHalfGrey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StarGrey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StarGoldHalfSilver);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StarGoldHalfGrey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StarGold);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StarBronzeHalfGrey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.StarBronze);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Star);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SportTennis);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SportSoccer);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SportShuttlecock);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SportRaquet);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SportGolfPractice);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SportGolf);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SportFootball);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SportBasketball);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Sport8ball);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Spellcheck);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SoundOut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SoundNone);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SoundMute);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SoundLow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SoundIn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SoundHigh);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SoundDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SoundAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Sound);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SortDescending);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SortAscending);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SmartphoneWrench);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SmartphoneKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SmartphoneGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SmartphoneError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SmartphoneEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SmartphoneDisk);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SmartphoneDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SmartphoneConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SmartphoneAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Smartphone);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SitemapColor);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Sitemap);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShieldStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShieldStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShieldSilver);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShieldRainbow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShieldGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShieldError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShieldDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShieldAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Shield);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Share);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeUngroup);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeSquareSelect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeSquareLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeSquareKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeSquareGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeSquareError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeSquareEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeSquareDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeSquareAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeSquare);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapesManySelect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapesMany);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeShadowToggle);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeShadow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeShadeC);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeShadeB);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeShadeA);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeRotateClockwise);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeRotateAnticlockwise);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeMoveFront);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeMoveForwards);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeMoveBackwards);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeMoveBack);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeHandles);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeGroup);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeFlipVertical);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeFlipHorizontal);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeAlignTop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeAlignRight);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeAlignMiddle);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeAlignLeft);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeAlignCenter);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ShapeAlignBottom);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Shape3d);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Shading);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerWrench);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerUncompressed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerDatabase);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerCompressed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerChart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ServerAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Server);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SectionExpanded);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.SectionCollapsed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Seasons);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptSave);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptPalette);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptGear);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptCodeRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptCode);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ScriptAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Script);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RubyPut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RubyLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RubyKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RubyGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RubyGet);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RubyGear);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RubyDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RubyAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Ruby);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RssValid);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RssGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RssError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RssDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RssAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Rss);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RosetteBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Rosette);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Rgb);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RewindGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RewindBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReverseGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReverseBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ResultsetPrevious);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ResultsetNext);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ResultsetLast);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ResultsetFirst);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportWord);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportUser);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportPicture);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportDisk);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.ReportAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Report);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Reload);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RecordRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RecordGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RecordBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.RainbowStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Rainbow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterMono);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterEmpty);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterColor);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterCancel);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PrinterAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Printer);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PreviousGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PluginLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PluginKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PluginGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PluginError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PluginEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PluginDisabled);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PluginDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PluginAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Plugin);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PlayGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PlayBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PillGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PillError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PillDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PillAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Pill);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Pilcrow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PicturesThumbs);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureSave);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Pictures);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureEmpty);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureClipboard);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PictureAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Picture);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Photos);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhotoPaint);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhotoLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhotoEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhotoDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhotoAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Photo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneSound);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PhoneAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Phone);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PencilGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PencilDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PencilAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Pencil);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PauseRecord);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PauseGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PauseBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PasteWord);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PastePlain);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Palette);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PaintcanRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PaintCanBrush);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Paintcan);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PaintbrushColor);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Paintbrush);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Paint);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWorld);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWord);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteZip);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteWrench);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteWorld);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteWord);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteWidth);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteVisualstudio);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteVector);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteTux);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteTextWidth);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteText);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteSwoosh);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteStar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteStack);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteSideBySide);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteRuby);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteRefresh);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhitePut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhitePowerpoint);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhitePicture);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhitePhp);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhitePasteTable);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhitePaste);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhitePaintbrush);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhitePaint2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhitePaint);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteOffice);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteMedal);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteHorizontal);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteH);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteGet);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteGear);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteFreehand);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteFont);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteFlash);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteFind);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteExcel);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteDvd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteDatabaseYellow);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteDatabase);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCup);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCsharp);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCplusplus);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCopy);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteConnect);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCompressed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteColdfusion);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCodeRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCode);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCdr);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteCamera);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteC);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteBreak);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteActionscript);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhiteAcrobat);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageWhite);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageSave);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageRefresh);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PagePortraitShot);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PagePortrait);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PagePaste);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PagePaintbrush);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageLandscapeShot);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageLandscape);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageHeaderFooter);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageGear);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageForward);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageFind);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageExcel);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageCopy);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageCode);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageCancel);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageBreakInsert);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageBreak);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageBack);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageAttach);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PageAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Page);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageWhite);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageSe);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageIn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageDown);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.PackageAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Package);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Overlays);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Outline);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NoteGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NoteError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NoteEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NoteDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NoteAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Note);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.None);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NextGreen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NextBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NewspaperLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NewspaperGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NewspaperDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NewspaperAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Newspaper);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NewRed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.NewBlue);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.New);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Neighbourhood);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MusicNote);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Music);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MouseError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MouseDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MouseAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Mouse);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MoonFull);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MonitorLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MonitorLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MonitorKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MonitorGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MonitorError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MonitorEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MonitorDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MonitorAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Monitor);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MoneyYen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MoneyPound);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MoneyEuro);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MoneyDollar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MoneyDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MoneyAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Money);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalSilverDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalSilverAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalSilver3);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalSilver2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalSilver1);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalGoldDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalGoldAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalGold3);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalGold2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalGold1);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalBronzeDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalBronzeAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalBronze3);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalBronze2);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MedalBronze1);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapMagnify);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapCursor);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapClipboard);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MapAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Map);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Male);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Mail);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MagnifierZoomIn);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Magnifier);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.MagifierZoomOut);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LorryStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LorryStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LorryLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LorryGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LorryFlatbed);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LorryError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LorryDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LorryAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Lorry);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LockStop);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LockStart);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LockOpen);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LockKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LockGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LockEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LockDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LockBreak);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LockAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Lock);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LinkGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LinkError);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LinkEdit);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LinkDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LinkBreak);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LinkAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Link);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LightningGo);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LightningDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LightningAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Lightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LightbulbOff);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LightbulbDelete);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LightbulbAdd);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.Lightbulb);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutSidebar);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutLink);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutLightning);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutKey);
            ResourceManager1.RegisterIcon(Ext.Net.Icon.LayoutHeader);

        }
        #endregion

        public string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }






    }
}