# Задача 2
## Вариант 4
Создать иерархию классов, состоящую из одного базового класса и двух классов-наследников. 
На что следует обратить внимание (за невыполнение оценка будет снижена): 
- Обязательно классы реализуются в отдельном модуле. 
- Программа оформляется в соответствии с требованиями к разработке ПО (отступы, названия переменных и т.п.). 
- В конструкторе класса могут и должны быть настроены значения полей и свойств класса, но инициализация значений должна быть вне класса. Например, не следует в конструкторе класса задавать в списке какие-то значения, или какое-то имя студента, если они не передаются в конструктор через параметр.
- Должен быть хотя-бы один вирутальный или абстрактный метод или свойство и их перекрытие.
- Программу желательно сделать с использованием форм (WPF).
- Необходимо придерживаться модели MVVM.
  
**Базовый класс:** элемент файловой системы (свойства название, свойство на папку, в которой он находится (если корневой, то значение null), свойство указывающее местоположение (вычисляемое), статические методы копирования и переноса объекта в другую папку, абстрактное свойство, определяющее тип элемента (сделать перечисление: папка, файл), абстрактное свойство, возвращающее размер).
**Производные классы:** папка, файл. Папка должна содержать список элементов файловой системы. В производных классах реализовать свойство, определяющее их назначение (папка или файл), и размер (для папки он вычисляется по размеру элементов, которые она содержит, для файла задаётся при создании в конструкторе).
