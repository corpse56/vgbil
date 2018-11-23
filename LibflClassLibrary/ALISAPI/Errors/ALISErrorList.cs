using ALISAPI.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.Errors
{
    public static class ALISErrorList
    {
        public static List<ALISError> _list = new List<ALISError>()
        {
            new ALISError()
            {
                Code = "R001",
                Message = "Введён неверный логин или пароль",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "R002",
                Message = "Токен недействителен или не существует",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "R003",
                Message = "Неизвестный тип логина",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "R004",
                Message = "Читатель не найден",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "R005",
                Message = "Указанная дата рождения не совпадает с датой рождения пользователя",
                httpStatusCode = System.Net.HttpStatusCode.Conflict,
            },
            new ALISError()
            {
                Code = "R006",
                Message = "Пароль не может быть пустой",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "R007",
                Message = "Письмо не отправлено и нет временной записи в БД",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "R008",
                Message = "Такой ссылки не существует",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "R009",
                Message = "Ссылка просрочена",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "R010",
                Message = "E-mail уже используется",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "R011",
                Message = "Письмо не отправлено и нет временной записи в БД",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "R012",
                Message = "Такой ссылки не существует",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "R013",
                Message = "Ссылка просрочена",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "R014",
                Message = "Такой ссылки не существует",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            new ALISError()
            {
                Code = "G001",
                Message = "Не удалось десериализовать входной JSON запрос",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "G002",
                Message = "Необрабатываемая ошибка времени выполнения",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "G003",
                Message = "Обязательное поле пустое",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "G004",
                Message = "Числовое значение равно 0 или отрицательное",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "G005",
                Message = "Дата вне диапазона",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "G006",
                Message = "Превышена длина строки",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "G007",
                Message = "Файл не найден",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
            new ALISError()
            {
                Code = "G008",
                Message = "Файл имеет неверный формат",
                httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
            },
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            new ALISError()
            {
                Code = "B001",
                Message = "Книга не найдена",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },

        };
    }
}
