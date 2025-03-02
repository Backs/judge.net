using FluentAssertions;
using Judge.Model.Entities;
using NUnit.Framework;

namespace Judge.Services.Tests;

[TestFixture]
public class FileNameResolverTests
{
    private static readonly FileNameResolver FileNameResolver = new FileNameResolver();

    [Test]
    public void ResolveFromSolutionTest()
    {
        var solution = @"import java.io.*;
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
}";
        var language = new Language
        {
            Id = 1,
            AutoDetectFileName = true,
            CompilerOptionsTemplate = "{FileName}.java"
        };


        var fileName = FileNameResolver.Resolve(solution, 1, new[] { language });
        fileName.Should().Be("yield.java");
    }

    [Test]
    public void ClassNotFoundTest()
    {
        var solution = @"import java.io.*;
import java.util.StringTokenizer;

    public static void main(String[] args) throws Exception {
        BufferedReader in = new BufferedReader(new InputStreamReader(System.in));
        StringTokenizer st = new StringTokenizer(in.readLine());
        int a = Integer.parseInt(st.nextToken());
        int b = Integer.parseInt(st.nextToken());
        PrintWriter out = new PrintWriter(System.out);
        out.println(a + b);
        out.close();
}";
        var language = new Language
        {
            Id = 1,
            AutoDetectFileName = true,
            CompilerOptionsTemplate = "{FileName}.java"
        };


        var fileName = FileNameResolver.Resolve(solution, 1, new[] { language });
        fileName.Should().Be("main.tmp");
    }

    [Test]
    public void ResolveDefaultFileNameTest()
    {
        var language = new Language
        {
            Id = 1,
            AutoDetectFileName = false,
            DefaultFileName = "main.cpp"
        };


        var fileName = FileNameResolver.Resolve("solution", 1, new[] { language });
        fileName.Should().Be("main.cpp");
    }
}