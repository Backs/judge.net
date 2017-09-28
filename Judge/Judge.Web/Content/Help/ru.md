### 1. Выбираем задачу

Чтобы увидеть список задач перейдите по ссылке [Список задач](Problems). 
Выберите интересующую вас задачу. 
Для тех, кто здесь впервые, рекомендуем начать с задачи [A+B](Problems/Statement/1). После того как определитесь с выбором задачи, приступайте к ее решению.

### 2. Решаем задачу и отправляем решение на проверку

Решением задачи является исходный код программы, написанный на одном из доступных языков программирования.

Пример решения задачи [A+B](Problems/Statement/1), в которой требуется считать два целых числа и вывести их сумму (на языке C++):

```
#include <iostream>
int main()
{
   int a, b;
   std::cin >> a >> b;
   std::cout << a + b << std::endl;
   return 0;
}
```

Чтобы автоматическая проверяющая система смогла протестировать ваше решение, оно должно отвечать следующим требованиям:

1. программа должна являться консольным приложением;
2. входные данные подаются программе в стандартном потоке ввода (ввод с клавиатуры). Программа должна выводить ответ в стандартный поток вывода (вывод на экран);
3. программа должна выводить только те данные, которые требует условие задачи. Выводить приглашение для ввода («Введите N:») не нужно. Также не нужно ожидать нажатия клавиши в конце работы программы;
4. входные данные в тестах всегда удовлетворяют ограничениям, описанным в условиях задач. Проверять эти ограничения в своих решениях не требуется.

В решениях задач запрещается:

1. работа с любыми файлами;
2. выполнение внешних программ и создание новых процессов;
3. работа с элементами графического интерфейса (окнами, диалогами и т.д.);
4. работа с внешними устройствами (принтером, звуковой картой и т.д.);
5. использование сетевых средств.

### 3. Как писать решения на определенном языке?

#### C++

```
#include <iostream>
int main()
{
   int a, b;
   std::cin >> a >> b;
   std::cout << a + b << std::endl;
   return 0;
}
```

#### C#
```
using System;

namespace Judge.Tests.TestSolutions
{
    class AB
    {
        public static void Main(string[] args)
        {
            var rows = Console.ReadLine().Split(' ');
            Console.WriteLine(int.Parse(rows[0]) + int.Parse(rows[1]));
        }
    }
}

```

#### Java

Основной класс должен иметь такое же название, как и файл. Например, следующий код содержится в файле `yield.java`

```
import java.io.*;
import java.util.StringTokenizer;

public class yield {

    public static void main(String[] args) throws Exception {
        BufferedReader in = new BufferedReader(new InputStreamReader(System.in));
        StringTokenizer st = new StringTokenizer(in.readLine());
        int a = Integer.parseInt(st.nextToken());
        int b = Integer.parseInt(st.nextToken());
        PrintWriter out = new PrintWriter(System.out);
        out.println(a + b);
        out.close();
    }
}
```

#### Pascal
```
var
   a, b: integer;
begin
   readln(a, b);
   writeln(a + b);
end.
```

#### JavaScript

```
var readline = require('readline');
var rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

rl.on('line', function(data){
    var line = data.split(' ');
    console.log(parseInt(line[0]) + parseInt(line[1]));
});
```

#### F#
```
open System

[<EntryPoint>]
let main argv = 
    let result = Console.ReadLine().Split(' ') |> Seq.map System.Int32.Parse |> Seq.sum
    printfn "%d" result
    0

```