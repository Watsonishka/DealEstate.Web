public class AnonymousIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CookieName = "X-Anonymous-ID";

    public AnonymousIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context) // код нужен для того, чтобы привязать анонимную корзину/избранное к конкретному браузеру,
                                                       // даже пока пользователь не ввел логин и пароль.
    {
        if (!context.Request.Cookies.TryGetValue(CookieName, out var anonymousId)) // Система проверяет: «Приходил ли этот браузер к нам раньше?».
                                                                                   // Если браузер прислал куку X-Anonymous-ID, мы просто берем её значение
                                                                                   // в переменную anonymousId
        {
            anonymousId = Guid.NewGuid().ToString(); // Генерация и установка нового ID (если гость впервые)

            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddDays(90),
                SameSite = SameSiteMode.Strict // это настройка безопасности куки, которая говорит браузеру: «Отправляй эту куку ТОЛЬКО тогда,
                                               // когда пользователь находится прямо на нашем сайте».
            };

            context.Response.Cookies.Append(CookieName, anonymousId, options);
        }

        context.Items["AnonymousID"] = anonymousId; // без этой строчки контроллеры во время первого клика «Добавить в корзину» получили бы null вместо ID

        await _next(context); // Middleware выполнил свою работу (убедился, что гость идентифицирован) и передает запрос дальше по конвейеру
    }
}
