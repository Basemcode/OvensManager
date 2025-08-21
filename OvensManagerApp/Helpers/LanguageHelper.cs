using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvensManagerApp.Helpers;

public class LanguageHelper
{
     public static Dictionary<string, string> OperatingModesInRus = new()
    {
         {"Stopped"                                ,"Стоп"                          }   // 0 – режим Стоп;
        ,{"Working"                                ,"Работа"                        }   // 1 – режим Работа;
        ,{"CriticalAccident"                       ,"Критическая Авария"            }   // 2 – режим Критическая Авария;
        ,{"ProgramIsCompleted"                     ,"Технолога завершена"           }   // 3 – программа технолога завершена;
        ,{"AutoTuneModeOfThePIDController"         ,"ПИД-регулятора"                }   // 4 – режим Автонастройка ПИД-регулятора;
        ,{"WaitingForAutoTuneModeToStart"          ,"Запуска режима Автонастройка"  }   // 5 – ожидание запуска режима Автонастройка;
        ,{"AutoTuningOfThePIDControllerIsCompleted","ПИД-регулятора завершена"      }   // 6 – автонастройка ПИД-регулятора завершена;
        ,{"SetupMode"                              ,"Настройка"                     }   // 7 – режим Настройка.
        ,{"Idle"                                   ,"Отключен"                      }
     };
}
