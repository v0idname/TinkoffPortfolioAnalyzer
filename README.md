# Tinkoff Portfolio Analyzer
[![.NET Core Desktop Build Testing](https://github.com/v0idname/TinkoffPortfolioAnalyzer/actions/workflows/build-testing.yml/badge.svg?branch=dev)](https://github.com/v0idname/TinkoffPortfolioAnalyzer/actions/workflows/build-testing.yml)

Проект для просмотра пользовательских инвест. портфелей для брокера Тинькофф Инвестиции.

## Technologies Used
- .NET 5.0
- WPF, MVVM

## Features
- Просмотр (+ сортировка по различным параметрам) всех бумаг во всех доступных портфелях.
- Мини-анализ портфеля путем расчета % доли каждой бумаги в портфеле и их отображения в виде круговой диаграммы.
- Выгрузка всех доступных для покупки бумаг и их сохранение в БД для последующего сравнения (какие бумаги добавили / убрали).

## Usage
1. Для доступа к портфелям или выгрузки доступных бумаг необходимо [получить токен](https://tinkoffcreditsystems.github.io/invest-openapi/auth/). Для выгрузки списка доступных бумаг будет достаточно sandbox токена, для просмотра содержимого портфеля нужен trading токен.
1. Для запуска программы... (_пункт в разработке_)
1. Добавить полученный токен в программу с помощью пункта меню "Управление токенами".
1. Enjoy)
