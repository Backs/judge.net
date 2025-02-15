import React from "react";
import {Divider} from 'antd';
import {Link} from "react-router-dom";

export const Help: React.FC = () => {

    return (<>
            <Divider>Select problem</Divider>
            Open <Link to="/problems">Problems</Link> to see whole list of problems. Select the task you are interested
            in.
            For the first time, I recommend to start with the problem <Link to="/problems/1">A+B</Link>.

            <Divider>Solve a problem and submit a solution</Divider>
            Solution of every problem is a code written in an available programming language

            Example of solving problem <Link to="/problems/1">A+B</Link>, which requires counting sum of two integers
            (in C++):
            <pre>
              <code>
                  {`
#include <iostream>
int main()
{
  int a, b;
  std::cin >> a >> b;
  std::cout << a + b << std::endl;
  return 0;
}
`}
              </code>
            </pre>

            In order for the automated testing system to be able to test your solution, it must meet the following
            requirements:
            <ul>
                <li>the program must be a console application;</li>
                <li>input data is fed to the program in the standard input stream (console input). The program must
                    output the response to the standard output stream (console output);
                </li>
                <li>the program should output only the data that the problem condition requires. There is no need to
                    output a prompt for input ("Enter N:"). There is also no need to wait for a key to be pressed at the
                    end of the program;
                </li>
                <li>the input data in tests always satisfy the constraints described in the problem conditions. There is
                    no need to check these constraints in your solutions.
                </li>
            </ul>

            Limitations for solutions:
            <ul>
                <li>forbidden to work with files;</li>
                <li>execution of external programs and creation of new processes is prohibited;</li>
                <li>working with graphical interface elements (windows, dialogs, etc.) is prohibited;</li>
                <li>working with external devices (printer, sound card, etc.) is prohibited;</li>
                <li>use of network calls is prohibited.</li>
            </ul>
            <Divider>How to write solutions in a specific language?</Divider>

            <h4>C++</h4>
            <pre>
              <code>
                  {`
#include <iostream>
int main()
{
  int a, b;
  std::cin >> a >> b;
  std::cout << a + b << std::endl;
  return 0;
}
`}
              </code>
            </pre>

            <h4>C#</h4>
            <pre>
              <code>
                  {`
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
`}
              </code>
            </pre>
            <h4>Java</h4>
            The main class must have the same name as the file name. For example, the following code is contained in the
            file <code>yield.java</code>:
            <pre>
              <code>
                  {`
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
`}
              </code>
            </pre>

            <h4>Pascal</h4>
            <pre>
              <code>
                  {`
var
   a, b: integer;
begin
   readln(a, b);
   writeln(a + b);
end.
`}
              </code>
            </pre>

            <h4>JavaScript</h4>
            <pre>
              <code>
                  {`
var readline = require('readline');
var rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

rl.on('line', function(data){
    var line = data.split(' ');
    console.log(parseInt(line[0]) + parseInt(line[1]));
});
`}
              </code>
            </pre>

            <h4>F#</h4>
            <pre>
              <code>
                  {`
open System

[<EntryPoint>]
let main argv = 
    let result = Console.ReadLine().Split(' ') |> Seq.map System.Int32.Parse |> Seq.sum
    printfn "%d" result
    0
`}
              </code>
            </pre>

            <h4>Python 3</h4>
            <pre>
              <code>
                  {`
print(sum(int(x) for x in input().split(' ')))
`}
              </code>
            </pre>

            <h4>Kotlin</h4>
            <pre>
              <code>
                  {`
fun main(args: Array<String>) {
   val (x, y) = readLine()!!.split(' ').map(String::toInt)
   println(x + y)
`}
              </code>
            </pre>

            <h4>Go</h4>
            <pre>
              <code>
                  {`
package main
import "fmt"

func main() {
   var a, b int
   fmt.Scan(&a)
   fmt.Scan(&b)
   
`}
              </code>
            </pre>
        </>
    );
}