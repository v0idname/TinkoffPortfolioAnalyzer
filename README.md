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
1. Необходимо получить токен для доступа к инвест. портфелям (ИСС, брокерский). Для этого смотрим [инструкцию](https://tinkoffcreditsystems.github.io/invest-openapi/auth/).
1. Для запуска программы... (_пункт в разработке_)
1. Выбор токена в программе... (_пункт в разработке_)
