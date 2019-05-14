/*
1. Все нужные настройки -- в файле web.config, все интуитивно понятно! ничего перекомпилировать не надо!
1.1. хэш пароля пользователя системы генерируется с помощью алгоритма SHA1, в линуксе можно сделать так, цитирую:
echo -ne secret | sha1sum
конец цитаты, где secret -- пароль пользователя.там, где сейчас worker01, можно добавить еще пользователей, чтобы и их пускало в систему
1.2. key= "ALIS_ADDRESS"-- тут хранится адрес API, его надо менять для отладки.
*/

using System;
using System.Web.UI;

namespace ReadersEMailCheck_2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.StatusCode = 302;
            Response.Redirect("~/EMC", true);
            Response.End();
        }
    }
}