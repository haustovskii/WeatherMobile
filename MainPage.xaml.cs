using Newtonsoft.Json;
using System.Net;

namespace WeatherMobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
        public async void WeatherLoad()
        {
            try
            {
                //создаем пустые переменные для использования в соединении с API
                string responsdata = string.Empty;
                string city = string.Empty;
                //получаем значение из EntCity куда мы вводим название города
                city = EntCity.Text;
                //создаем переменную типа http-запроса
                HttpWebRequest request = WebRequest.Create("http://api.weatherapi.com/v1/forecast.json?key=1f3d81110d91494187080148230202&q=" +
                city + "&days=1&aqi=no&alerts=no") as HttpWebRequest;
                //присваем значение Wed-запроса (ссылка с названием города, от которого будет зависеть данные, которые мы получим)
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //создаем переменную и возвращаем значения полученные с API
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    //считываем данные
                    responsdata = reader.ReadToEnd();
                    //завершаем чтение
                    reader.Close();
                    //создаем динамическую переменную для и присваеваем ей значение возвращенных данных, но перед этим конвертируем
                    //их с помощью библиотеки NewtonsoftJson
                    dynamic responce_global = JsonConvert.DeserializeObject(responsdata);
                    L2Veter.Text = (responce_global["current"]["wind_kph"]) + " км/ч";
                    string napravlenie = responce_global["current"]["wind_dir"];
                    switch (napravlenie)
                    {
                        case "E": napravlenie = "В"; break;
                        case "W": napravlenie = "З"; break;
                        case "N": napravlenie = "С"; break;
                        case "S": napravlenie = "Ю"; break;
                        case "NW": napravlenie = "СЗ"; break;
                        case "SW": napravlenie = "ЮЗ"; break;
                        case "NE": napravlenie = "СВ"; break;
                        case "SE": napravlenie = "ЮВ"; break;
                        case "NNW": napravlenie = "ССЗ"; break;
                        case "SSW": napravlenie = "ЮЮЗ"; break;
                        case "NNE": napravlenie = "ССВ"; break;
                        case "SSE": napravlenie = "ЮЮВ"; break;
                    }
                    L2Naprav.Text = napravlenie;
                    //присваеваем HeaderTemp значение темпиратуры по ощущению в этом городе
                    HeaderTemp.Text = "Ощущается как " + responce_global["current"]["feelslike_c"] + "°";
                    //присваеваем L3UF значение ультрафиолета в данном городе 
                    L3UF.Text = Convert.ToDouble(responce_global["current"]["uv"]).ToString();
                    //присваеваем Pb3UF значение ультрафиолета для наглядного представления видимости значения
                    Pb3UF.Progress = Convert.ToDouble(L3UF.Text) / 10;
                    string condition = responce_global["current"]["condition"]["text"];
                    //присваеваем EntCity значение города
                    EntCity.Text = responce_global["location"]["name"];
                    //присваеваем LHeader значение температуры в этом городе
                    LHeader.Text = Convert.ToString(Math.Round(responce_global["current"]["temp_c"])) + "°";
                    //присваеваем LHeaderStatus значение осадков в этом городе
                    LHeaderStatus.Text = condition.ToString();
                }
            }
            catch
            {
                //в случае если город не найден, вызваем метод
                await DisplayAlert("Ошибка", "Закрыть", null, "Город не найден");
            }
        }
        private void FindCity_Clicked(object sender, EventArgs e)
        {
            //по нажатию кнопку вызываем метод
            WeatherLoad();
        }
    }

}
