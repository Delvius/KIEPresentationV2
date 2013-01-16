using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Foundation;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;

namespace GoogleCalendar
{
    [DataContract]
    public class Token
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }
        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }
    }

    internal enum HttpMethod
    {
        POST = 0,
        GET = 1,
        PUT = 2,
        DELETE = 4
    }

    public class CalendarService
    {
        #region Singleton
        private static CalendarService _instance;
        private static readonly object padlock = new object();
        public static CalendarService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CalendarService();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion Singleton


           #region LogApi
        private static readonly string ClientID = "169263469598.apps.googleusercontent.com";
        private static readonly string ClientSecret = "GRDX6x5P9Axi-5o0z6bQdW5m";
        //private static readonly string ApiKey = "AIzaSyB0c32CnNAjYQegkIBRS-QXhF2i9NxH4CM";
        private static readonly string Scope = "https://www.googleapis.com/auth/calendar";
        string googleUri = "https://accounts.google.com/o/oauth2/auth";
        string response_type = "code";
        string redirect_uri = "http://localhost/auth";
        #endregion LogApi

      


        #region TokenFile
        private const string FILE_NAME = "google.calendar.metro.auth";
        private const string SEPARATOR = "\r\n";
        private const string KEY = "sk#pi;84ur{nMu!e";
        #endregion TokenFile

        private Token AccessToken;

        private CalendarService() { }

        /// <summary>
        /// Methode qui supprime le token
        /// </summary>
        public async void CleanToken()
        {
            bool tok=false;
            try
            {
                string encryptedText = await FileManager.ReadFile(FILE_NAME);
                string decryptedText = CryptographicManager.Decrypt(encryptedText, KEY);
                if (!String.IsNullOrEmpty(encryptedText))
                    tok = true;
            }
            catch (Exception)
            {
            }

            if (tok)
            {
                FileManager.RemoveFileAsync(FILE_NAME);
                AccessToken = null;
            }
        }

        /// <summary>
        /// Methode qui vérifie si le Token peut être rafraichit depuis le Token existant, et qui fait une demande de token sinon
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitToken()
        {
            bool canRefresh = false;
            try
            {
                string encryptedText = await FileManager.ReadFile(FILE_NAME);
                string decryptedText = CryptographicManager.Decrypt(encryptedText, KEY);
                if (String.IsNullOrEmpty(encryptedText) || String.IsNullOrEmpty(decryptedText))
                    canRefresh = false;
                else
                    canRefresh = await RefreshToken(decryptedText);
            }
            catch (Exception)
            {
            }
            //Cannot await in catch...
            try
            {
                if (!canRefresh)
                    return await LoadToken();
            }
            catch (Exception e)
            {
                FileManager.Log("Cann't InitToken : " + e.Message);
            }
            return false;
        }

        /// <summary>
        /// Methode qui permet de vérifier si le token a pu être obtenu depuis le refresh token existant, s'il y en a un
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsInitToken()
        {
            bool canRefresh = false;
            try
            {
                string encryptedText = await FileManager.ReadFile(FILE_NAME);
                string decryptedText = CryptographicManager.Decrypt(encryptedText, KEY);
                if (String.IsNullOrEmpty(encryptedText) || String.IsNullOrEmpty(decryptedText))
                    canRefresh = false;
                else
                    canRefresh = await RefreshToken(decryptedText);
            }
            catch (Exception)
            {
            }
            //Cannot await in catch...
        
            return canRefresh;
        }


        #region METHODES DE COMMUNICATION AVEC GOOGLE
        public async Task<Stream> Post(string requestUri, HttpContent content, bool needAuthorization = true)
        {
            return await ExecRequest(HttpMethod.POST, requestUri, needAuthorization, content);
        }
        public async Task<Stream> Put(string requestUri, HttpContent content, bool needAuthorization = true)
        {
            return await ExecRequest(HttpMethod.PUT, requestUri, needAuthorization, content);
        }

        public async Task<Stream> Get(string requestUri, bool needAuthorization = true)
        {
            return await ExecRequest(HttpMethod.GET, requestUri, needAuthorization);
        }

        public async Task<Stream> Delete(string requestUri, bool needAuthorization = true)
        {
            return await ExecRequest(HttpMethod.DELETE, requestUri, needAuthorization);
        }
        #endregion

        /// <summary>
        /// Execute une requête httprequest
        /// </summary>
        /// <param name="method">La methode de la requete</param>
        /// <param name="requestUri">L'URI de la requête</param>
        /// <param name="content">Le contenu de la requête</param>
        /// <param name="needAuthorization">If the request need an authorization</param>
        /// <returns>La reponse de la requete (null si la requete a echoué)</returns>
        /// <remarks>POST et PUT doivent avoir un contenu</remarks>
        private async Task<Stream> ExecRequest(HttpMethod method, string requestUri, bool needAuthorization = true, HttpContent content = null)
        {
            HttpMessageHandler handler = new HttpClientHandler();
            HttpClient httpClient = new HttpClient(handler);
            httpClient.MaxResponseContentBufferSize = 100000;
            if (needAuthorization && AccessToken != null && !String.IsNullOrEmpty(AccessToken.AccessToken))
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken.AccessToken);

            HttpResponseMessage result;

            switch (method)
            {
                case HttpMethod.DELETE:
                    result = await httpClient.DeleteAsync(requestUri);
                    break;

                case HttpMethod.POST:
                    if (content == null) return null;
                    result = await httpClient.PostAsync(requestUri, content);
                    break;

                case HttpMethod.PUT:
                    if (content == null) return null;
                    result = await httpClient.PutAsync(requestUri, content);
                   

                    break;

                case HttpMethod.GET:
                    result = await httpClient.GetAsync(requestUri);
                    break;

                default:
                    return null;
            }

            if (result.IsSuccessStatusCode)
            {
                Stream stream = await result.Content.ReadAsStreamAsync();
                stream.Position = 0;
                return stream;
            }
            else
                GoogleErrorOccured(await result.Content.ReadAsStringAsync());

            return null;
        }

        /// <summary>
        /// Methode qui permet de sérialiser un objet en JSON
        /// </summary>
        /// <param name="objet"></param>
        /// <returns></returns>
        public Stream Serialize(object objet)
        {
            DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(objet.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, objet);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Methode qui permet de récupérer un nouveau TOKEN
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoadToken()
        {
            //Step 1. Create a Request and Callback Uri for the Authentication operation
            Uri requestUri = new Uri(string.Format("{0}?scope={1}&redirect_uri={2}&response_type={3}&client_id={4}",googleUri, Scope, redirect_uri, response_type, ClientID), UriKind.RelativeOrAbsolute);
            Uri callbackUri = new Uri(redirect_uri, UriKind.RelativeOrAbsolute);

            //Step 2. Initialize a Authentication operation using WebAuthenticationBroker
            WebAuthenticationResult webResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None,requestUri, callbackUri);

            switch (webResult.ResponseStatus)
            {
                case WebAuthenticationStatus.ErrorHttp:
                    GoogleErrorOccured(webResult.ResponseData);
                    break;

                case WebAuthenticationStatus.Success:
                    string code = webResult.ResponseData.Split(new string[] { "code=" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    string postData = string.Format("client_id={0}&client_secret={1}&code={2}&redirect_uri={3}&grant_type=authorization_code", ClientID, ClientSecret, code, redirect_uri);
                    StringContent content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
                    Stream result = await Post("https://accounts.google.com/o/oauth2/token", content, false);

                    if (result != null)
                    {
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Token));
                        byte[] arrayMessage = new byte[result.Length];
                        result.Read(arrayMessage, 0, arrayMessage.Length);
                        var message = Encoding.UTF8.GetString(arrayMessage, 0, arrayMessage.Length);
                        System.Diagnostics.Debug.WriteLine(message);
                        result.Position = 0;
                        AccessToken = (Token)deserializer.ReadObject(result);
                        FileManager.WriteFileAsync(FILE_NAME, CryptographicManager.Encrypt(AccessToken.RefreshToken, KEY));
                        return true;
                    }
                    break;

                case WebAuthenticationStatus.UserCancel:
                    //The User selected to cancel the Authentication process
                    break;

                default:
                    break;

            }
            return false;
        }



        /// <summary>
        /// Methode qui permet de rafraichir le Token depuis l'access Token
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public async Task<bool> RefreshToken(string refresh_token)
        {
            string postData = string.Format("client_id={0}&client_secret={1}&refresh_token={2}&grant_type=refresh_token", ClientID, ClientSecret, refresh_token);
            StringContent content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
            Stream result = await Post("https://accounts.google.com/o/oauth2/token", content, false);
            if (result != null)
            {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Token));
                AccessToken = (Token)deserializer.ReadObject(result);
                AccessToken.RefreshToken = refresh_token;
                FileManager.WriteFileAsync(FILE_NAME, CryptographicManager.Encrypt(AccessToken.RefreshToken, KEY));
                return true;
            }
            return false;
        }

        public void GoogleErrorOccured(Stream stream)
        {
            byte[] arrayMessage = new byte[stream.Length];
            stream.Read(arrayMessage, 0, arrayMessage.Length);
            var message = Encoding.UTF8.GetString(arrayMessage, 0, arrayMessage.Length);
            GoogleErrorOccured(message);
        }

        public void GoogleErrorOccured(string message)
        {
            FileManager.Log("Google::" + message);
        }
    }
}
