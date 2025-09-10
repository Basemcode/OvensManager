# OvensManagerApp

OvensManagerApp is a WPF desktop application built with C# using the MVVM pattern. It provides a real-time dashboard to monitor and control multiple ovens. Each oven is represented as a card showing its number, temperature, runtime, and status, with dynamic color updates based on temperature.

---

## Features

- **Real-time updates:** The oven data updates automatically every few seconds.
- **MVVM architecture:** Clean separation of UI, business logic, and data.
- **Custom OvenCard control:** Each oven is displayed as a reusable, customizable card.
- **Dynamic UI:** Oven cards change background color based on temperature.
- **Animation-ready:** Background and data transitions are smooth and can be animated.
- **Supports multiple ovens:** Designed for dashboards up to 4x4 grid (16 ovens) by default.
- **Expandable:** Easily add more ovens or change polling intervals.

---

## Technologies Used

- **C#**
- **.NET 8**
- **WPF**
- **MVVM Design Pattern**
- **ObservableCollection & INotifyPropertyChanged** for real-time UI updates
- **Serial communication** (RS-485 / Owen protocol) for connecting with ovens

---

## Project Structure

