﻿# language: ru
Функционал: Тестирование входов 
@BadLogins
Сценарий: Негативные проверки входа
       Тогда открывается страница "Главная"
	   И пользователь в блоке "Контактная информация" (нажимает кнопку) "Адрес"
       И пользователь (нажимает кнопку) "Войти"
       Тогда открывается страница "Авторизация"
       И пользователь (заполняет поле) "Логин" значением "Хэй бэби"
       И пользователь (заполняет поле) "Пароль" значением "Лэтс мув"
       И пользователь (нажимает кнопку) "Войти"
       Тогда пользователь (проверяет значение элемента) "Текст ошибки" со значением "Не верный логин или пароль!"