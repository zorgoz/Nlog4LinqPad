# Nlog4LinqPad
NLog logging support for LinqPad

Simple usage method:
1) Add `NLog` to your LinqPad query
2) Add this package from https://www.nuget.org/packages/zorgoz.Nlog4LinqPad to your query
3) Add `use zorgoz.Nlog4LinqPad;` to your code (or use LinqPad's tools for this)
4) Add `Nlog4LinqPad.LogToHtmlResults();` or `Nlog4LinqPad.LogToConsoleResults();` to your code (you could add both, but would make little sense)
5) If needed create a logger instance
6) Start logging
7) Enjoy the results with default settings

_The output is highly customisable, but a complete documntation is yet to come..._
