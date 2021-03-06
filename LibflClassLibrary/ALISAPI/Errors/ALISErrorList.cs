﻿using ALISAPI.Errors;
using LibflClassLibrary.ImageCatalog;
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
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
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
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "R011",
                Message = "Письмо не отправлено и нет временной записи в БД",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
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
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
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
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "G002",
                Message = "Необрабатываемая ошибка времени выполнения",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "G003",
                Message = "Обязательное поле пустое",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "G004",
                Message = "Числовое значение равно 0 или отрицательное",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "G005",
                Message = "Дата вне диапазона",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "G006",
                Message = "Превышена длина строки",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "G007",
                Message = "Файл не найден",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "G008",
                Message = "Файл имеет неверный формат",
                httpStatusCode = System.Net.HttpStatusCode.UnsupportedMediaType,
            },
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            new ALISError()
            {
                Code = "B001",
                Message = "Книга не найдена",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "B002",
                Message = "Ошибка заполнения экземпляра в базе.",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "B003",
                Message = "Электронной копии для указанного Id книги не существует",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            new ALISError()
            {
                Code = "L001",
                Message = "Аккаунт Литрес не выдан данному читателю.",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "L002",
                Message = "Аккаунт литрес уже назначен читателю. Нельзя назначить аккаунт дважды.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "L003",
                Message = "Свободные Литрес аккаунты закончились. Попробуйте позже.",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "L004",
                Message = "Получение аккаунта из Литрес завершилось неудачей. Попробуйте позже.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },

 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            new ALISError()
            {
                Code = "C001",
                Message = "Нельзя выдать более 5 электронных книг.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C002",
                Message = "Эта электронная копия уже выдана вам",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C003",
                Message = "Нельзя выдать электронных экземпляров больше чем бумажных, так как это нарушит авторское право",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C004",
                Message = "Вы не можете заказать эту электронную копию, поскольку запрещено заказывать ту же копию, если не прошли сутки с момента её возврата.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C005",
                Message = "Невозможно заказать книгу, так как все экземпляры выданы.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C006",
                Message = "Эта книга уже выдана вам. Нельзя заказать книгу второй раз.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C007",
                Message = "Действий для заказа не предусмотрено",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C008",
                Message = "Недопустимый тип заказа",
                httpStatusCode = System.Net.HttpStatusCode.PreconditionFailed,
            },
            new ALISError()
            {
                Code = "C009",
                Message = "Свободных экземпляров для выдачи на дом нет. Попробуйте другой тип заказа.",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "C010",
                Message = "Свободных экземпляров для выдачи в помещении библиотеки нет. Попробуйте другой тип заказа.",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "C011",
                Message = "Заказ не найден",
                httpStatusCode = System.Net.HttpStatusCode.NotFound,
            },
            new ALISError()
            {
                Code = "C012",
                Message = "Заказ с таким статусом не может быть помещён в историю",
                httpStatusCode = System.Net.HttpStatusCode.PreconditionFailed,
            },
            new ALISError()
            {
                Code = "C013",
                Message = "Такой тип заказа не возможен для данной книги и данного читателя",
                httpStatusCode = System.Net.HttpStatusCode.PreconditionFailed,
            },
            new ALISError()
            {
                Code = "C014",
                Message = "Ошибка доступа к файловому хранилищу",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C015",
                Message = "Заказ с таким статусом нельзя продлить",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C016",
                Message = "Заказ уже был продлён. Заказы выданные на дом и электронные выдачи можно продлить только один раз.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C017",
                Message = "Бронеполку можно продлить только 3 раза.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C018",
                Message = "Неизвестная ошибка при продлении",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C019",
                Message = "У читателя нет прав бесплатного абонемента! Выдача на дом невозможна.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },

            new ALISError()
            {
                Code = "C020",
                Message = "У читателя закончился срок прав бесплатного абонемента. Выдача на дом невозможна.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },

            new ALISError()
            {
                Code = "C021",
                Message = "Эта книга на ДП. Её не нужно принимать в хранение.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C022",
                Message = "Книгу, взятую из открытого доступа, можно вернуть только в зале её местонахождения",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C023",
                Message = "Неверный штрихкод читателя",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C024",
                Message = "Читатель уже посетил этот зал сегодня",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C025",
                Message = "Все заказы на эту книгу завершены. Её не нужно принимать. Можно поставить книгу на место.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C026",
                Message = "Заказ на такую книгу не сформирован. Принимать такую книгу на кафедру не нужно.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C027",
                Message = "Заказ с таким статусом не нужно принимать на кафедру.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C028",
                Message = "Читатель не проходил через турникет. Выдача невозможна!",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C029",
                Message = "Невозможная ситуация при приёме книги от читателя",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C030",
                Message = "Нельзя выдать на дом более 10 книг",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C031",
                Message = "Уважаемые читатели! Нам небезразличны ваше здоровье и безопасность. Поэтому с 18 марта 2020 года мы временно приостанавливаем очное обслуживание читателей и посетителей, а также отменяем все наши мероприятия. Мы хотим, чтобы вы знали — в это непростое время мы остаемся на связи и всегда готовы ответить на ваши вопросы онлайн или по телефону, а также предоставить доступ ко всем нашим удаленным онлайн-ресурсам и оцифрованным изданиям. Будьте здоровы и берегите себя и близких! ",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C032",
                Message = "Уважаемые читатели! Временно литература на дом не выдаётся в связи с эпидемией. ",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C033",
                Message = "Уважаемый читатель! В связи с эпидемией, временно действуют ограничения на количество используемых книг. Вы можете заказать и/или взять не более 5 книг. ",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "C034",
                Message = "Уважаемый читатель! Зал, в котором находится книга ещё не открылся. ",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
//////////////////////////////////////////////////////////////////////Ошибки имидж каталога//////////////////////////
            new ALISError()
            {
                Code = "M001",
                Message = "Карточка с указанным номером не найдена",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "M002",
                Message = "Номер карты имеет неверный формат. Невозможно перевести в число.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "M003",
                Message = "Заказ на выбранную карточку уже существует",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "M004",
                Message = $"Максимальное число заказов ({ICLoader.MAX_ALLOWED_ORDERS_PER_READER}) из имидж каталога превышено.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "M005",
                Message = "Удалить заказ можно только со статусом \"Заказ сформирован\". ",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "M006",
                Message = "Удалённый читатель не может заказывать литературу из имидж-каталога ",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
//////////////////////////////////////////////////////////////////////Ошибки очереди на оцифровку//////////////////////////
            new ALISError()
            {
                Code = "Q001",
                Message = "Эта книга уже присутствует в очереди на оцифровку!",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "Q002",
                Message = "Нельзя добавить в очередь на оцифровку более двух изданий в сутки!",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "Q003",
                Message = "Очередь на оцифровку переполнена! Попробуйте позже.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },
            new ALISError()
            {
                Code = "Q004",
                Message = "Оцифровка книг возможна только для книг основного фонда.",
                httpStatusCode = System.Net.HttpStatusCode.BadRequest,
            },


        };
    }
}
